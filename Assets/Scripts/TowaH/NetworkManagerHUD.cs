using UnityEngine;

namespace TowaH {
    [DisallowMultipleComponent]
    [AddComponentMenu("TowaH/NetworkManager HUD")]
    [RequireComponent(typeof(TowaHNetworkManager))]
    public class NetworkManagerHUD : MonoBehaviour {
        private TowaHNetworkManager manager;
        
        [Header("GUI")]
        [SerializeField] private int offsetX = 10;
        [SerializeField] private int offsetY = 10;
        
        private string serverAddress = "localhost";

        private void Awake() {
            manager = GetComponent<TowaHNetworkManager>();
        }

        private void OnGUI() {
            if (manager.state == NetworkState.Offline) {
                DrawGUI();
            }
        }

        private void DrawGUI() {
            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 300));
            
            serverAddress = GUILayout.TextField(serverAddress);

            if (GUILayout.Button("Connect to party")) {
                manager.ConnectToParty(serverAddress);
            }
            
            if (GUILayout.Button("Host party")) {
                manager.CreateParty();
            }
            
            GUILayout.EndArea();
        }
    }
}
