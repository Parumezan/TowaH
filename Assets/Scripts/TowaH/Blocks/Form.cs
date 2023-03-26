using UnityEngine;

namespace TowaH.Blocks {
    [CreateAssetMenu(fileName = "New Form", menuName = "TowaH/Blocks/Form")]
    public class Form : ScriptableObject {
        public Sprite sprite;
        public GameObject prefab;
    }
}
