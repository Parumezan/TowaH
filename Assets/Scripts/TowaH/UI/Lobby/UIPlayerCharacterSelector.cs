using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TowaH.UI.Lobby {
    public class UIPlayerCharacterSelector : MonoBehaviour {
        [SerializeField] private Button leftSelectorButton;
        [SerializeField] private Button rightSelectorButton;
        [SerializeField] private Image characterImage;
        
        public UnityEvent<int> onCharacterSelected;

        private int characterIndex = 0;

        private void Awake() {
            Debug.Assert(leftSelectorButton != null, "Left selector button is null");
            Debug.Assert(rightSelectorButton != null, "Right selector button is null");
            Debug.Assert(characterImage != null, "Character image is null");
        }
        
        private void Start() {
            leftSelectorButton.onClick.RemoveAllListeners();
            leftSelectorButton.onClick.AddListener(OnLeftSelectorButtonClicked);
            
            rightSelectorButton.onClick.RemoveAllListeners();
            rightSelectorButton.onClick.AddListener(OnRightSelectorButtonClicked);
            
            SelectCharacter(0);
        }

        public void SelectCharacter(int index) {
            characterIndex = index;
            
            // Check if index is valid
            // if not, wrap around
            if (characterIndex < 0) {
                characterIndex = TowaHGameManager.instance.availableCharacters.Count - 1;
            } else if (characterIndex >= TowaHGameManager.instance.availableCharacters.Count) {
                characterIndex = 0;
            }
            
            CharacterProfile characterProfile = TowaHGameManager.instance.availableCharacters[characterIndex];
            characterImage.sprite = characterProfile.image;
            
            onCharacterSelected?.Invoke(characterIndex);
        }
        
        private void OnLeftSelectorButtonClicked() {
            SelectCharacter(characterIndex - 1);
        }
        
        private void OnRightSelectorButtonClicked() {
            SelectCharacter(characterIndex + 1);
        }
    }
}
