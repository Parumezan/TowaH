using UnityEngine;
using UnityEngine.UI;

namespace TowaH.UI {
    public class UILobby : MonoBehaviour {
        [SerializeField] private UIPopup uiPopup;
        [SerializeField] private TowaHNetworkManager networkManager;
        [SerializeField] private GameObject panel;
        [SerializeField] private Text characterDescriptionText;
        
        private void Awake() {
            Debug.Assert(uiPopup != null, "UI Popup is null");
            Debug.Assert(networkManager != null, "Network manager is null");
            Debug.Assert(panel != null, "Panel is null");
            Debug.Assert(characterDescriptionText != null, "Character description text is null");
        }
        
        public void OnDisconnectButton() {
            // TODO: Implement
        }

        public void OnReadyButton() {
            // TODO: Implement
        }
    }
}
