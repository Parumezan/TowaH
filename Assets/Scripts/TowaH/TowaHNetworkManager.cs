using System;
using System.Collections.Generic;
using Mirror;
using TowaH.UI;
using UnityEngine;
using UnityEngine.Events;

namespace TowaH {
    public enum NetworkState { Offline, Handshake, Lobby, Game }
    
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
        
        [SerializeField] private int playerUsernameMaxLength = 16;

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
            
            // Setup handlers
            NetworkClient.RegisterHandler<ErrorMsg>(OnClientError, false);
            NetworkClient.RegisterHandler<PlayerUpdateCharactersMsg>(OnClientPlayerUpdateCharacters);
            
            onStartClient?.Invoke();
        }

        public override void OnStopClient() {
            Debug.Log("OnStopClient");

            // Set state
            state = NetworkState.Offline;

            onStopClient?.Invoke();
        }

        public override void OnClientConnect() {
            Debug.Log("OnClientConnect");
            onClientConnect?.Invoke(NetworkClient.connection);
        }

        public override void OnClientDisconnect() {
            Debug.Log("OnClientDisconnect");
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

        private void OnClientPlayerUpdateCharacters(PlayerUpdateCharactersMsg msg) {
            Debug.Log("OnClientPlayerUpdateCharacters");
            
            // Set state
            state = NetworkState.Lobby;
        }

        #endregion
        
        #region Server
        
        public NetworkState serverState = NetworkState.Offline;
        
        public Dictionary<NetworkConnection, string> lobby = new Dictionary<NetworkConnection, string>();
        public Dictionary<string, PlayerInfo> players = new Dictionary<string, PlayerInfo>();

        public override void OnStartServer() {
            Debug.Log("OnStartServer");
            
            // Setup handlers
            NetworkServer.RegisterHandler<SelectPlayerCharacterMsg>(OnServerSelectPlayerCharacter);
            NetworkServer.RegisterHandler<PlayerReadyMsg>(OnServerPlayerReady);
            
            onStartServer?.Invoke();
        }

        public override void OnStopServer() {
            Debug.Log("OnStopServer");
            onStopServer?.Invoke();
        }

        // Called on the server if a client connects after successful auth
        public override void OnServerConnect(NetworkConnectionToClient conn) {
            Debug.Log("OnServerConnect");
            
            // Check if party is full (maxConnections)
            if (lobby.Count >= maxConnections) {
                ServerSendError(conn, "party is full", true);
                return;
            }
            
            if (serverState == NetworkState.Game) {
                ServerSendError(conn, "party is already started", true);
                return;
            }
            
            string playerId = lobby[conn];
            
            // Create player info
            var playerInfo = new PlayerInfo {
                uniqueId = playerId,
                id = lobby.Count,
                selectedCharacterIndex = 0
            };
            players[playerId] = playerInfo;
            
            conn.Send(MakePlayerUpdateCharacters());
            
            onServerConnect?.Invoke(conn);
        }
        
        // TODO: Rework this
        private PlayerUpdateCharactersMsg MakePlayerUpdateCharacters() {
            var playerToUpdateCharacters = new PlayerUpdateCharactersMsg.CharacterPreview[players.Count];
            
            int i = 0;
            foreach (PlayerInfo player in players.Values) {
                var preview = new PlayerUpdateCharactersMsg.CharacterPreview {
                    playerId = player.id,
                    characterIndex = player.selectedCharacterIndex
                };
                playerToUpdateCharacters[i] = preview;
                ++i;
            }
            
            return new PlayerUpdateCharactersMsg {
                characters = playerToUpdateCharacters
            };
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn) {
            Debug.Log("OnServerDisconnect");
            
            StartCoroutine(DoServerDisconnect(conn, 0.0f));
        }
        
        private IEnumerator<WaitForSeconds> DoServerDisconnect(NetworkConnectionToClient conn, float delay) {
            yield return new WaitForSeconds(delay);
            
            onServerDisconnect.Invoke(conn);
            
            string playerId = lobby[conn];
            players.Remove(playerId);
            lobby.Remove(conn);
            
            // Update all clients
            conn.Send(MakePlayerUpdateCharacters());
            
            base.OnServerDisconnect(conn);
        }

        public void ServerSendError(NetworkConnection conn, string error, bool disconnect) {
            conn.Send(new ErrorMsg{text=error, causesDisconnect=disconnect});
        }

        private void OnServerSelectPlayerCharacter(NetworkConnectionToClient conn, SelectPlayerCharacterMsg msg) {
            Debug.Log("OnServerSelectPlayerCharacter");
            
            if (!lobby.ContainsKey(conn)) {
                Debug.Log("SelectPlayerCharacter: not in lobby" + conn);
                ServerSendError(conn, "SelectPlayerCharacter: not in lobby", true);
                return;
            }
            
            // TODO: Implement
        }
        
        private void OnServerPlayerReady(NetworkConnectionToClient conn, PlayerReadyMsg msg) {
            Debug.Log("OnServerPlayerReady");
            
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
            
            // TODO: Implement
        }

        #endregion
    }
}
