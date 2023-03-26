using UnityEngine;

namespace TowaH {
    public class CharacterAnimation : MonoBehaviour {
        private const float RotateSpeed = 5f;
        private const float Radius = 0.15f;
        
        private float _angle = 0f;
        private Vector2 _offset;
        private Vector2 _center;

        private void Awake() {
            _center = transform.position;
        }
    
        void Update() {
            _angle += RotateSpeed * Time.deltaTime;
            _offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = _center + _offset;
        }
    }
}
