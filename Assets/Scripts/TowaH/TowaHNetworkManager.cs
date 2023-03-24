using Mirror;
using UnityEngine;

namespace TowaH {
    public class TowaHNetworkManager : NetworkManager {
        public void ConnectToServer(string ip, short port) {
            Debug.Log($"Connecting to {ip}:{port}");
        }
    }
}
