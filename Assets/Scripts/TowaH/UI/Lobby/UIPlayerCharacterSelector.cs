using UnityEngine;
using UnityEngine.UI;

namespace TowaH.UI.Lobby {
    public class UIPlayerCharacterSelector : MonoBehaviour {
        [SerializeField] private Button leftSelectorButton;
        [SerializeField] private Button rightSelectorButton;
        [SerializeField] private Image characterImage;

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
            
            characterImage.sprite = null;
        }
        
        private void OnLeftSelectorButtonClicked() {
            Debug.Log("Left selector button clicked");
        }
        
        private void OnRightSelectorButtonClicked() {
            Debug.Log("Right selector button clicked");
        }
    }
}
