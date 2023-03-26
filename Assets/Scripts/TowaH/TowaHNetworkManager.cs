using System;
using System.Collections.Generic;
using Mirror;
using TowaH.UI;
using UnityEngine;
using UnityEngine.Events;

namespace TowaH {
    public enum NetworkState { Offline, Handshake, Lobby, Game }
    
    public enum GameState { Lobby, Game }
    
    [Serializable] public class UnityEventNetworkConnection : UnityEvent<NetworkConnection> {}
    
    public class TowaHNetworkManager : NetworkManager {
        [Header("Game Manager")]
        [SerializeField] private TowaHGameManager gameManager;
        
        [SerializeField] private UIPopup uiPopup;
        [SerializeField] private UILobby uiLobby;
        
        [Header("Events")]
        public UnityEvent onStartClient;
        public UnityEvent onStopClient;
        public UnityEvent onStartServer;
        public UnityEvent onStopServer;
        public UnityEventNetworkConnection onClientConnect;
        public UnityEventNetworkConnection onClientDisconnect;
        public UnityEventNetworkConnection onServerConnect;
        public UnityEventNetworkConnection onServerDisconnect;
        public UnityEvent onServerStartGame;
        public UnityEvent onServerEndGame;

        public override void Awake() {
            base.Awake();
            
            Debug.Assert(gameManager != null, "Game manager is null");
            Debug.Assert(uiPopup != null, "UI Popup is null");
            Debug.Assert(uiLobby != null, "UI Lobby is null");
        }
        
        #region Client
        
        // Current network manager state on client
        public NetworkState state = NetworkState.Offline;

        public override void OnStartClient() {
            Debug.Log("OnStartClient");
            
            // Register blocks prefabs
            foreach (GameObject prefab in gameManager.AvailableBlockPrefabs) {
                NetworkClient.RegisterPrefab(prefab);
            }
            
            // Setup handlers
            NetworkClient.RegisterHandler<ErrorMsg>(OnClientError, false);
            NetworkClient.RegisterHandler<PlayerCharactersMsg>(OnClientPlayerCharacters);
            NetworkClient.RegisterHandler<PlayerCharacterAuthorityMsg>(OnClientPlayerCharacterAuthority);
            NetworkClient.RegisterHandler<SelectPlayerCharacterMsg>(OnClientSelectPlayerCharacter);
            NetworkClient.RegisterHandler<PlayerJoinedMsg>(OnClientPlayerJoined);
            NetworkClient.RegisterHandler<PlayerLeftMsg>(OnClientPlayerLeft);
            NetworkClient.RegisterHandler<StartGameMsg>(OnClientStartGame);

            onStartClient?.Invoke();
        }

        public override void OnStopClient() {
            // Set state
            state = NetworkState.Offline;

            onStopClient?.Invoke();
        }

        public override void OnClientConnect() {
            onClientConnect?.Invoke(NetworkClient.connection);
        }

        public override void OnClientDisconnect() {
            onClientDisconnect?.Invoke(NetworkClient.connection);
        }

        public void ConnectToParty(string address) {
            Debug.Log($"Connecting to party at {address}");
            
            // Set address to mirror's network manager
            networkAddress = address;

            StartClient();
        }

        public void CreateParty() {
            Debug.Log("Creating party");
            
            StartHost();
        }
        
        public void LeaveParty() {
            Debug.Log("Leaving party");
            
            NetworkClient.connection.Disconnect();
            if (NetworkServer.active) {
                
                // also stop the host if running as host
                // (host shouldn't start server but disconnect client for invalid
                // login, which would be pointless)
                StopHost();
            }
        }

        private void OnClientError(ErrorMsg message) {
            Debug.Log($"Client error: {message.text}");
            
            // Show error message
            uiPopup.Show(message.text);

            // Disconnect if it was an important network error
            // (this is needed because the login failure message doesn't disconnect
            // the client immediately (only after timeout))
            if (message.causesDisconnect) {
                NetworkClient.connection.Disconnect();

                // also stop the host if running as host
                // (host shouldn't start server but disconnect client for invalid
                // login, which would be pointless)
                if (NetworkServer.active) {
                    StopHost();
                }
            }
        }

        private void OnClientPlayerCharacters(PlayerCharactersMsg msg) {
            // Set state
            state = NetworkState.Lobby;

            foreach (PlayerCharactersMsg.CharacterPreview character in msg.characters) {
                Debug.Log($"Player {character.playerId} selected character {character.characterIndex}");
                
                uiLobby.OnJoinPlayer(character.playerId);
                uiLobby.OnPlayerCharacterSelected(character.playerId, character.characterIndex);   
            }
        }
        
        private void OnClientPlayerCharacterAuthority(PlayerCharacterAuthorityMsg msg) {
            uiLobby.OnPlayerCharacterAuthority(msg.playerId);
        }
        
        private void OnClientSelectPlayerCharacter(SelectPlayerCharacterMsg msg) {
            uiLobby.OnPlayerCharacterSelected(msg.playerId, msg.characterIndex);
        }
        
        private void OnClientPlayerJoined(PlayerJoinedMsg msg) {
            uiLobby.OnJoinPlayer(msg.playerId);
            // Set default character
            uiLobby.OnPlayerCharacterSelected(msg.playerId, 0);
        }
        
        private void OnClientPlayerLeft(PlayerLeftMsg msg) {
            uiLobby.OnLeavePlayer(msg.playerId);
        }
        
        private void OnClientStartGame(StartGameMsg msg) {
            Debug.Log("OnClientStartGame");
            
            // Set state
            state = NetworkState.Game;
        }

        #endregion
        
        #region Server
        
        public GameState gameState = GameState.Lobby;
        
        public Dictionary<NetworkConnection, string> lobby = new Dictionary<NetworkConnection, string>();
        public Dictionary<string, PlayerInfo> players = new Dictionary<string, PlayerInfo>();

        public override void OnStartServer() {
            // Setup handlers
            NetworkServer.RegisterHandler<SelectPlayerCharacterMsg>(OnServerSelectPlayerCharacter);
            NetworkServer.RegisterHandler<PlayerReadyMsg>(OnServerPlayerReady);
            
            onStartServer?.Invoke();
        }

        public override void OnStopServer() {
            onStopServer?.Invoke();
        }

        // Called on the server if a client connects after successful auth
        public override void OnServerConnect(NetworkConnectionToClient conn) {
            // Check if party is full (maxConnections)
            if (lobby.Count >= maxConnections) {
                ServerSendError(conn, "party is full", true);
                return;
            }
            
            if (gameState != GameState.Lobby) {
                ServerSendError(conn, "party is already started", true);
                return;
            }
            
            string playerId = lobby[conn];
            
            // Create player info
            var playerInfo = new PlayerInfo {
                uniqueId = playerId,
                id = generatePlayerId(),
                selectedCharacterIndex = 0,
                isReady = false,
                connection = conn
            };
            players[playerId] = playerInfo;
            
            conn.Send(MakePlayerUpdateCharacters());
            
            conn.Send(new PlayerCharacterAuthorityMsg {
                playerId = playerInfo.id
            });
            
            // Send player joined message to all clients
            var playerJoinedMsg = new PlayerJoinedMsg {
                playerId = playerInfo.id
            };
            foreach (NetworkConnectionToClient c in NetworkServer.connections.Values) {
                if (c != conn) {
                    c.Send(playerJoinedMsg);
                }
            }
            
            onServerConnect?.Invoke(conn);
        }

        private int generatePlayerId() {
            int id = 0;
            
            foreach (PlayerInfo player in players.Values) {
                if (player.id == id) {
                    ++id;
                }
            }
            return id;
        }
        
        // TODO: Rework this
        private PlayerCharactersMsg MakePlayerUpdateCharacters() {
            var playerToUpdateCharacters = new PlayerCharactersMsg.CharacterPreview[players.Count];
            
            int i = 0;
            foreach (PlayerInfo player in players.Values) {
                var preview = new PlayerCharactersMsg.CharacterPreview {
                    playerId = player.id,
                    characterIndex = player.selectedCharacterIndex
                };
                playerToUpdateCharacters[i] = preview;
                ++i;
            }
            
            return new PlayerCharactersMsg {
                characters = playerToUpdateCharacters
            };
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn) {
            StartCoroutine(DoServerDisconnect(conn, 0.0f));
        }
        
        private IEnumerator<WaitForSeconds> DoServerDisconnect(NetworkConnectionToClient conn, float delay) {
            yield return new WaitForSeconds(delay);
            
            string playerId = lobby[conn];
            if (players.ContainsKey(playerId)) {
                var playerJoinedMsg = new PlayerLeftMsg {
                    playerId = players[playerId].id
                };
                foreach (NetworkConnectionToClient c in NetworkServer.connections.Values) {
                    if (c != conn) {
                        c.Send(playerJoinedMsg);
                    }
                }
            }

            onServerDisconnect.Invoke(conn);
            
            players.Remove(playerId);
            lobby.Remove(conn);
            
            // End game if only one player left
            if (gameState == GameState.Game && players.Count <= 1) {
                ServerEndGame();
            }

            base.OnServerDisconnect(conn);
        }

        public void ServerSendError(NetworkConnection conn, string error, bool disconnect) {
            conn.Send(new ErrorMsg{text=error, causesDisconnect=disconnect});
        }

        private void OnServerSelectPlayerCharacter(NetworkConnectionToClient conn, SelectPlayerCharacterMsg msg) {
            if (!lobby.ContainsKey(conn)) {
                Debug.Log("SelectPlayerCharacter: not in lobby" + conn);
                ServerSendError(conn, "SelectPlayerCharacter: not in lobby", true);
                return;
            }
            
            if (gameState != GameState.Lobby) {
                Debug.Log("SelectPlayerCharacter: game already started" + conn);
                ServerSendError(conn, "SelectPlayerCharacter: game already started", true);
                return;
            }
            
            string playerId = lobby[conn];
            PlayerInfo player = players[playerId];
            
            // Set character
            player.selectedCharacterIndex = msg.characterIndex;
            
            // Send update to all clients
            var selectPlayerCharacterMsg = new SelectPlayerCharacterMsg {
                playerId = player.id,
                characterIndex = msg.characterIndex
            };
            foreach (NetworkConnectionToClient c in NetworkServer.connections.Values) {
                if (c != conn) {
                    c.Send(selectPlayerCharacterMsg);
                }
            }
        }
        
        private void OnServerPlayerReady(NetworkConnectionToClient conn, PlayerReadyMsg msg) {
            if (!lobby.ContainsKey(conn)) {
                Debug.Log("PlayerReady: not in lobby" + conn);
                ServerSendError(conn, "PlayerReady: not in lobby", true);
                return;
            }
            
            // Check if we have enough players
            if (players.Count < 2) {
                ServerSendError(conn, "not enough players", false);
                return;
            }
            
            if (gameState != GameState.Lobby) {
                Debug.Log("PlayerReady: game already started" + conn);
                ServerSendError(conn, "PlayerReady: game already started", true);
                return;
            }
            
            string playerId = lobby[conn];
            PlayerInfo player = players[playerId];
            
            // Set ready
            player.isReady = true;
            
            // Check if all players are ready
            bool allReady = true;
            foreach (PlayerInfo p in players.Values) {
                if (!p.isReady) {
                    allReady = false;
                    break;
                }
            }
            
            if (allReady) {
                ServerStartGame();
            }
        }

        private void ServerStartGame() {
            Debug.Log("ServerStartGame");
            
            // Set state
            gameState = GameState.Game;
            
            // Send start game message to all clients
            var startGameMsg = new StartGameMsg();
            foreach (NetworkConnectionToClient c in NetworkServer.connections.Values) {
                c.Send(startGameMsg);
            }
            
            onServerStartGame?.Invoke();
        }
        
        private void ServerEndGame() {
            Debug.Log("ServerEndGame");
            
            // Set state
            gameState = GameState.Lobby;
            
            // Send end game message to all clients
            var endGameMsg = new EndGameMsg();
            foreach (NetworkConnectionToClient c in NetworkServer.connections.Values) {
                c.Send(endGameMsg);
            }
            
            onServerEndGame?.Invoke();
        }

        #endregion
    }
}
