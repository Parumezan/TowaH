using System.Collections.Generic;
using TowaH.UI.Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace TowaH.UI {
    public class UILobby : MonoBehaviour {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text characterDescriptionText;
        [SerializeField] private List<UIPlayerCharacterSelector> characterSelectors = new List<UIPlayerCharacterSelector>();
        
        [SerializeField] private UIMainMenu mainMenu;

        private void Awake() {
            Debug.Assert(panel != null, "Panel is null");
            Debug.Assert(characterDescriptionText != null, "Character description text is null");
        }

        public void OnBackButton() {
            mainMenu.Show();
            Hide();
        }

        public void OnReadyButton() {
            Hide();
            TowaHGameManager.instance.StartGame();
        }

        public void OnPlayerCharacterSelected(int index, int characterIndex) {
            characterSelectors[index].SelectCharacter(characterIndex);
        }
        
        public void OnPlayerCharacterAuthority(int index) {
       
        }
        
        public void Show() {
            panel.SetActive(true);
        }
        
        public void Hide() {
            panel.SetActive(false);
        }
    }
}
