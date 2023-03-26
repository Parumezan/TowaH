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
        
        private void Start() {
            characterSelectors[0].onCharacterSelected.RemoveAllListeners();
            characterSelectors[0].onCharacterSelected.AddListener(characterIndex => {
                TowaHGameManager.instance.players[0].CharacterIndex = characterIndex;
            });
            
            characterSelectors[1].onCharacterSelected.RemoveAllListeners();
            characterSelectors[1].onCharacterSelected.AddListener(characterIndex => {
                TowaHGameManager.instance.players[1].CharacterIndex = characterIndex;
            });
        }

        public void OnBackButton() {
            mainMenu.Show();
            Hide();
        }

        public void OnReadyButton() {
            Hide();
            TowaHGameManager.instance.StartGame();
        }

        public void Show() {
            panel.SetActive(true);
        }
        
        public void Hide() {
            panel.SetActive(false);
        }
    }
}
