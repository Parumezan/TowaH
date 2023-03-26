using UnityEngine;

namespace TowaH.Blocks {
    public class BlocksPhysics : MonoBehaviour {
        private Rigidbody2D _rb;
        private float _speed = 5f;
        private float _falling_speed = 4f;
        private bool _falling = true;
        private BlocksManager _bn;

        private void Awake() {
            _bn = GameObject.Find("GameMaster").GetComponent<BlocksManager>();
            _rb = gameObject.GetComponent<Rigidbody2D>();
        }
    
        private void Update() {
            if (transform.position.y < -10) {
                Destroy(gameObject);
                return;
            }
            if (_falling) {
                if (gameObject.transform.position.x < -2) {
                    gameObject.transform.position = new Vector2(-2, transform.position.y);
                }
                if (gameObject.transform.position.x > 2) {
                    gameObject.transform.position = new Vector2(2, transform.position.y);
                }

                float x = Input.GetAxis("Horizontal");
                if (Input.GetAxis("Vertical") < 0) {
                    _rb.velocity = new Vector2(x * _speed, -_falling_speed * 2);
                } else {
                    _rb.velocity = new Vector2(x * _speed, -_falling_speed);
                }
            }
        }
    
        private void StopFalling() {
            _falling = false;
            _rb.gravityScale = 1; 
        }

        //TODO: remove spawn, spawn it in server
        private void OnCollisionEnter2D(Collision2D collision) {
            if (!_falling) {
                return;
            }
            StopFalling();
            _bn.SpawnRandomObject();
        }
    }
}
