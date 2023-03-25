using System;
using Mirror;
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

        private void Update() {
            panel.SetActive(networkManager.state == NetworkState.Lobby);
        }

        public void OnDisconnectButton() {
            NetworkClient.Disconnect();
        }

        public void OnReadyButton() {
            Debug.Log("OnReadyButton");
        }
    }
}
