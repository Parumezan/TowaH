using System;
using UnityEngine;
using UnityEngine.UI;

namespace TowaH.UI {
    public class UIPopup : MonoBehaviour {
        public static UIPopup instance;
        
        [SerializeField] private GameObject panel;
        [SerializeField] private Text messageText;
        
        public UIPopup() {
            if (instance == null) {
                instance = this;
            }
        }

        private void Awake() {
            Debug.Assert(panel != null, "Panel is null");
            Debug.Assert(messageText != null, "Message text is null");
        }

        public void Show(string message) {
            if (panel.activeSelf) {
                messageText.text += ";\n" + message;
            } else {
                messageText.text = message;
            }
            
            panel.SetActive(true);
        }
    }
}
