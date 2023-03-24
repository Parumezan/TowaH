using UnityEngine;

namespace TowaH {
    [DisallowMultipleComponent]
    [AddComponentMenu("TowaH/NetworkManager HUD")]
    [RequireComponent(typeof(TowaHNetworkManager))]
    public class NetworkManagerHUD : MonoBehaviour {
        private TowaHNetworkManager manager;
        
        [Header("GUI")]
        [SerializeField] private int offsetX = 10;
        [SerializeField] private int offsetY = 40;
        
        private void Awake() {
            manager = GetComponent<TowaHNetworkManager>();
        }
    
        void OnGUI() {
            if (!manager.isNetworkActive) {
                DrawGUI();
            }
        }

        void DrawGUI() {
            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 300));

            GUILayout.BeginHorizontal();
            
            string ip = GUILayout.TextField("127.0.0.1");
            short port = short.Parse(GUILayout.TextField("25565"));

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Connect")) {
                manager.ConnectToServer(ip, port);
            }

            GUILayout.EndArea();
        }
    }
}
