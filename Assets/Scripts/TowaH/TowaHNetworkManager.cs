using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace TowaH {
    public enum NetworkState { Offline, Handshake, Lobby, Game }
    
    [Serializable] public class UnityEventNetworkConnection : UnityEvent<NetworkConnection> {}
    
    public class TowaHNetworkManager : NetworkManager {
        [Header("Events")]
        public UnityEvent onStartClient;
        public UnityEvent onStopClient;
        public UnityEvent onStartServer;
        public UnityEvent onStopServer;
        public UnityEventNetworkConnection onClientConnect;
        public UnityEventNetworkConnection onClientDisconnect;
        public UnityEventNetworkConnection onServerConnect;
        public UnityEventNetworkConnection onServerDisconnect;
        
        #region Client
        
        // Current network manager state on client
        public NetworkState state = NetworkState.Offline;

        public override void OnStartClient() {
            // Setup handlers
            NetworkClient.RegisterHandler<ErrorMsg>(OnClientError, false);
            
            onStartClient?.Invoke();
        }
        
        public override void OnStopClient() {
            onStopClient?.Invoke();
        }
        
        public override void OnClientConnect() {
            onClientConnect?.Invoke(NetworkClient.connection);
        }

        public override void OnClientDisconnect() {
            onClientDisconnect?.Invoke(NetworkClient.connection);
        }

        void OnClientError(ErrorMsg message) {
            Debug.Log($"Client error: {message.text}");
            
            // TODO: Show error message in UI
            
            // Disconnect if it was an important network error
            // (this is needed because the login failure message doesn't disconnect
            // the client immediately (only after timeout))
            if (message.causesDisconnect) {
                NetworkClient.connection.Disconnect();

                // also stop the host if running as host
                // (host shouldn't start server but disconnect client for invalid
                //  login, which would be pointless)
                if (NetworkServer.active) {
                    StopHost();
                }
            }
        }

        public void ConnectToServer(string ip, short port) {
            Debug.Log($"Connecting to {ip}:{port}");
        }
        
        #endregion
        
        #region Server
        
        public Dictionary<NetworkConnection, string> lobby = new Dictionary<NetworkConnection, string>();

        public override void OnStartServer() {
            onStartServer?.Invoke();
        }

        public override void OnStopServer() {
            onStopServer?.Invoke();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn) {
            onServerConnect?.Invoke(conn);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn) {
            onServerDisconnect?.Invoke(conn);
        }

        public void ServerSendError(NetworkConnection conn, string error, bool disconnect) {
            conn.Send(new ErrorMsg{text=error, causesDisconnect=disconnect});
        }

        #endregion
    }
}
