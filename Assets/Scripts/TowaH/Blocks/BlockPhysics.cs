using System;
using UnityEngine;

namespace TowaH.Blocks {
    public class BlockPhysics : MonoBehaviour {
        [SerializeField] private float speed = 0.05f;

        private Rigidbody2D rb;
        private bool isFalling = true;
        private Vector3 _startPosition;
        private Player _player;

        private void Awake() {
            rb = gameObject.GetComponent<Rigidbody2D>();
            Debug.Assert(rb != null, "BlockPhysics: Rigidbody2D not found");
            _startPosition = transform.position;
        }
        
        private void FixedUpdate()
        {
            // Destroy block if it falls out of the screen
            if (transform.position.y < -250) {
                Destroy(gameObject);
                return;
            }

            if (!isFalling) {
                return;
            }

            if (transform.position.x < _startPosition.x - 2)
                transform.position = new Vector3(_startPosition.x - 2, transform.position.y, transform.position.z);
            if (transform.position.x > _startPosition.x + 2)
                transform.position = new Vector3(_startPosition.x + 2, transform.position.y, transform.position.z);
            
            if (_player.id == 0 && Input.GetKey(KeyCode.D))
                transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            if (_player.id == 0 && Input.GetKey(KeyCode.Q))
                transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
            if (_player.id == 1 && Input.GetKey(KeyCode.RightArrow))
                transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            if (_player.id == 1 && Input.GetKey(KeyCode.LeftArrow))
                transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }

        public void AssignPlayer(Player player)
        {
            this._player = player;
        }
    
        private void StopFalling() {
            isFalling = false;
            rb.gravityScale = 1;
            _player.SpawnRandomBlock();
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            rb.velocity = new Vector2(0, 0);
            if (!isFalling) {
                return;
            }
            StopFalling();
        }
    }
}
