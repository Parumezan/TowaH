using UnityEditor;
using UnityEngine;

namespace TowaH {
    public class TowaHGameManager : MonoBehaviour {
        public static void Quit() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
