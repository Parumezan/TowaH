using UnityEngine;

namespace TowaH.Blocks {
    [CreateAssetMenu(fileName = "New Block", menuName = "TowaH/Block")]
    public class Block : ScriptableObject {
        public Sprite sprite;
        public GameObject prefab;
    }
}
