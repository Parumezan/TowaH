using UnityEngine;
using UnityEngine.UI;

namespace TowaH.UI {
    public class UILogin : MonoBehaviour {
        [SerializeField] private UIPopup uiPopup;
        [SerializeField] private TowaHNetworkManager networkManager;
        [SerializeField] private GameObject panel;
        [SerializeField] private InputField serverAddressInputField;
        [SerializeField] private Button connectButton;
        [SerializeField] private Button hostButton;
        
        private void Awake() {
            Debug.Assert(uiPopup != null, "UI Popup is null");
            Debug.Assert(networkManager != null, "Network manager is null");
            Debug.Assert(panel != null, "Panel is null");
            Debug.Assert(serverAddressInputField != null, "Server address input field is null");
            Debug.Assert(connectButton != null, "Connect button is null");
            Debug.Assert(hostButton != null, "Host button is null");
        }
        
        private void Start() {
            connectButton.onClick.RemoveAllListeners();
            connectButton.onClick.AddListener(() => {
                networkManager.ConnectToParty(serverAddressInputField.text);
            });
            
            hostButton.onClick.RemoveAllListeners();
            hostButton.onClick.AddListener(() => {
                networkManager.CreateParty();
            });
        }
        
        private void Update() {
            panel.SetActive(networkManager.state == NetworkState.Offline);
        }
        
        public void Show() {
            panel.SetActive(true);
        }
        
        public void Hide() {
            panel.SetActive(false);
        }
    }
}
