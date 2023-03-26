using UnityEngine;
using UnityEngine.UI;

namespace TowaH.UI {
    public class UIMainMenu : MonoBehaviour {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button playButton;
        
        [SerializeField] private UILobby lobby;

        private void Awake() {
            Debug.Assert(panel != null, "Panel is null");
            Debug.Assert(playButton != null, "Play button is null");
        }
        
        private void Start() {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() => {
                lobby.Show();
                Hide();
            });
        }

        public void Show() {
            panel.SetActive(true);
        }
        
        public void Hide() {
            panel.SetActive(false);
        }
    }
}
