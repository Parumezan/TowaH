using System.Collections.Generic;
using Mirror;
using TowaH.UI.Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace TowaH.UI {
    public class UILobby : MonoBehaviour {
        [SerializeField] private UIPopup uiPopup;
        [SerializeField] private TowaHNetworkManager networkManager;
        [SerializeField] private GameObject panel;
        [SerializeField] private Text characterDescriptionText;
        [SerializeField] private List<UIPlayerCharacterSelector> characterSelectors = new List<UIPlayerCharacterSelector>();

        private void Awake() {
            Debug.Assert(uiPopup != null, "UI Popup is null");
            Debug.Assert(networkManager != null, "Network manager is null");
            Debug.Assert(panel != null, "Panel is null");
            Debug.Assert(characterDescriptionText != null, "Character description text is null");
            Debug.Assert(characterSelectors != null, "Character selectors is null");
            Debug.Assert(characterSelectors.Count > 0, "Character selectors is empty");
            
            // Hide all character selectors
            characterSelectors.ForEach(selector => selector.gameObject.SetActive(false));
        }

        private void Update() {
            panel.SetActive(networkManager.state == NetworkState.Lobby);
        }

        public void OnDisconnectButton() {
            networkManager.LeaveParty();
        }

        public void OnReadyButton() {
            NetworkClient.Send(new PlayerReadyMsg());
        }

        public void OnJoinPlayer(int index) {
            characterSelectors[index].gameObject.SetActive(true);
        }
        
        public void OnLeavePlayer(int index) {
            characterSelectors[index].gameObject.SetActive(false);
        }
        
        public void OnPlayerCharacterSelected(int index, int characterIndex) {
            characterSelectors[index].SelectCharacter(characterIndex);
        }
        
        public void OnPlayerCharacterAuthority(int index) {
            characterSelectors[index].SetAuthority();
            
            characterSelectors[index].onCharacterSelected.RemoveAllListeners();
            characterSelectors[index].onCharacterSelected.AddListener(characterIndex => {
                NetworkClient.Send(new SelectPlayerCharacterMsg {
                    playerId = index,
                    characterIndex = characterIndex
                });
            });
        }
    }
}
