using UnityEngine;

namespace TowaH {
    [CreateAssetMenu(fileName = "CharacterProfile", menuName = "TowaH/Character Profile")]
    public class CharacterProfile : ScriptableObject {
        public string profileName;
        public Sprite image;
    }
}
