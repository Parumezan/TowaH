using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace TowaH {
    public enum NetworkState { Offline, Handshake, Lobby, Game }
    
    [Serializable] public class UnityEventNetworkConnection : UnityEvent<NetworkConnection> {}
    
    public class TowaHNetworkManager : NetworkManager {
        [Header("Game Manager")]
        [SerializeField] private TowaHGameManager gameManager;
        
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
        }
        
        #region Client
        
        // Current network manager state on client
        public NetworkState state = NetworkState.Offline;

        public override void OnStartClient() {
            Debug.Log("OnStartClient");
            
            // Setup handlers
            NetworkClient.RegisterHandler<ErrorMsg>(OnClientError, false);
            NetworkClient.RegisterHandler<CharactersAvailableMsg>(OnClientCharactersAvailable);
            
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

            // TODO: Show error message in UI

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

        private void OnClientCharactersAvailable(CharactersAvailableMsg msg) {
            Debug.Log("OnClientCharactersAvailable");
            
            // Set state
            state = NetworkState.Lobby;
            
            // TODO: Show character selection UI
        }

        #endregion
        
        #region Server
        
        public Dictionary<NetworkConnection, string> lobby = new Dictionary<NetworkConnection, string>();
        public Dictionary<string, PlayerInfo> players = new Dictionary<string, PlayerInfo>();

        public override void OnStartServer() {
            Debug.Log("OnStartServer");
            
            // Setup handlers
            NetworkServer.RegisterHandler<EditPlayerUsernameMsg>(OnServerEditPlayerUsername);
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
            
            string playerId = lobby[conn];
            
            // Create player info
            var playerInfo = new PlayerInfo {
                id = playerId,
                username = "Player " + players.Count
            };
            players[playerId] = playerInfo;
            
            var msg = new CharactersAvailableMsg {
                // TODO: Set characters available
            };
            conn.Send(msg);
            
            onServerConnect?.Invoke(conn);
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
            
            base.OnServerDisconnect(conn);
        }

        public void ServerSendError(NetworkConnection conn, string error, bool disconnect) {
            conn.Send(new ErrorMsg{text=error, causesDisconnect=disconnect});
        }
        
        public bool IsAllowedUsername(string username) {
            // Not too long?
            // Only contains letters, number and underscore and not empty (+)?
            return username.Length <= playerUsernameMaxLength && Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
        }
        
        private void OnServerEditPlayerUsername(NetworkConnectionToClient conn, EditPlayerUsernameMsg msg) {
            Debug.Log("OnServerEditPlayerUsername");

            if (!lobby.ContainsKey(conn)) {
                Debug.Log("EditPlayerUsername: not in lobby" + conn);
                ServerSendError(conn, "EditPlayerUsername: not in lobby", true);
                return;
            }
            
            // Validate username
            if (!IsAllowedUsername(msg.username)) {
                ServerSendError(conn, "invalid username", false);
                return;
            }
            
            // Grab player info
            string playerId = lobby[conn];
            PlayerInfo playerInfo = players[playerId];
            
            // Set username
            playerInfo.username = msg.username;
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
