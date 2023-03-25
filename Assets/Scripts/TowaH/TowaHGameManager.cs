using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TowaH {
    public class TowaHGameManager : MonoBehaviour {
        public static TowaHGameManager instance;
        
        public List<CharacterProfile> availableCharacters = new List<CharacterProfile>();
        
        public TowaHGameManager() {
            instance = this;
        }

        private void Awake() {
            Debug.Assert(availableCharacters.Count > 0, "Available characters is empty");
        }
        
        public static void Quit() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
