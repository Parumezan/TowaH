using UnityEngine;
using UnityEngine.UI;

namespace TowaH.UI.Lobby {
    public class UIPlayerCharacterSelector : MonoBehaviour {
        [SerializeField] private Button leftSelectorButton;
        [SerializeField] private Button rightSelectorButton;
        [SerializeField] private Image characterImage;
        
        private TowaHGameManager gameManager;
        
        private int characterIndex = 0;

        private void Awake() {
            Debug.Assert(leftSelectorButton != null, "Left selector button is null");
            Debug.Assert(rightSelectorButton != null, "Right selector button is null");
            Debug.Assert(characterImage != null, "Character image is null");
            
            gameManager = FindObjectOfType<TowaHGameManager>();
            Debug.Assert(gameManager != null, "Game manager is null");
        }
        
        private void Start() {
            leftSelectorButton.onClick.RemoveAllListeners();
            leftSelectorButton.onClick.AddListener(OnLeftSelectorButtonClicked);
            
            rightSelectorButton.onClick.RemoveAllListeners();
            rightSelectorButton.onClick.AddListener(OnRightSelectorButtonClicked);
            
            // Set default character
            // select first character
            SelectCharacter(0);
            
            SetAuthority();
        }
        
        public void SetAuthority() {
            leftSelectorButton.gameObject.SetActive(true);
            rightSelectorButton.gameObject.SetActive(true);
        }
        
        public void SelectCharacter(int index) {
            characterIndex = index;
            
            // Check if index is valid
            // if not, wrap around
            if (characterIndex < 0) {
                characterIndex = gameManager.availableCharacters.Count - 1;
            } else if (characterIndex >= gameManager.availableCharacters.Count) {
                characterIndex = 0;
            }
            
            CharacterProfile characterProfile = gameManager.availableCharacters[characterIndex];
            characterImage.sprite = characterProfile.image;
        }
        
        private void OnLeftSelectorButtonClicked() {
            Debug.Log("Left selector button clicked");
            SelectCharacter(characterIndex - 1);
        }
        
        private void OnRightSelectorButtonClicked() {
            Debug.Log("Right selector button clicked");
            SelectCharacter(characterIndex + 1);
        }
    }
}
