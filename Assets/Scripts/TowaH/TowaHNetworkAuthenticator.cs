using System;
using Mirror;
using UnityEngine;

namespace TowaH {
    public class TowaHNetworkAuthenticator : NetworkAuthenticator {
        private TowaHNetworkManager manager;

        private void Awake() {
            manager = GetComponent<TowaHNetworkManager>();
        }
        
        #region Client
        
        public override void OnStartClient() {
            NetworkClient.RegisterHandler<LoginSuccessMsg>(OnClientLoginSuccess, false);
        }

        public override void OnClientAuthenticate() {
            Debug.Log("OnClientAuthenticate");
            
            // Set state
            manager.state = NetworkState.Handshake;
        }
        
        void OnClientLoginSuccess(LoginSuccessMsg msg) {
            Debug.Log("OnClientLoginSuccess");
            // Authenticated successfully. OnClientConnected will be called.
            OnClientAuthenticated.Invoke();
        }
        
        #endregion
        
        #region Server

        public override void OnStartServer() {
            NetworkServer.RegisterHandler<LoginMsg>(OnServerLogin, false);
        }

        public override void OnServerAuthenticate(NetworkConnectionToClient conn) {
            Debug.Log("OnServerAuthenticate");
            // Wait for LoginMsg from client
        }

        bool PlayerLoggedIn(string playerId) {
            return manager.lobby.ContainsValue(playerId);
        }

        void OnServerLogin(NetworkConnectionToClient conn, LoginMsg msg) {
            Debug.Log("OnServerLogin");
            
            // Correct version?
            if (msg.version != Application.version) {
                manager.ServerSendError(conn, "outdated version", true);
                return;
            }
            
            // Generate UUID
            string playerId = Guid.NewGuid().ToString();
            
            // Player already logged in? (shouldn't happen)
            if (PlayerLoggedIn(playerId)) {
                manager.ServerSendError(conn, "already logged in", true);
                return;
            }
            
            // Login successful
            Debug.Log("Login successful: " + playerId);
            
            // Add to logged in players
            manager.lobby[conn] = playerId;
            
            // Notify client about successful login.
            conn.Send(new LoginSuccessMsg());
            
            // Authenticate on server
            OnServerAuthenticated.Invoke(conn);
        }

        #endregion
    }
}
