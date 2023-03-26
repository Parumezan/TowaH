using System;
using UnityEngine;

namespace TowaH.Blocks {
    public class BlockPhysics : MonoBehaviour {
        [SerializeField] private float speed = 0.05f;

        private Rigidbody2D rb;
        private bool isFalling = true;
        private Vector3 _startPosition;
        private Player _player;
        private float _timer = 1f;

        private void Awake() {
            rb = gameObject.GetComponent<Rigidbody2D>();
            Debug.Assert(rb != null, "BlockPhysics: Rigidbody2D not found");
            _startPosition = transform.position;
        }
        
        private void FixedUpdate()
        {
            // destroy the block if it's too down
            if (transform.position.y < -240) {
                Destroy(gameObject);
                if (isFalling) _player.SpawnRandomBlock();
                return;
            }

            if (!isFalling) return;

            // set the velocity constant (bad to do here, but it works)
            rb.velocity = new Vector2(0, -1.5f);

            // check the limit of the block
            if (transform.position.x < _startPosition.x - 2)
                transform.position = new Vector3(_startPosition.x - 2, transform.position.y, transform.position.z);
            if (transform.position.x > _startPosition.x + 2)
                transform.position = new Vector3(_startPosition.x + 2, transform.position.y, transform.position.z);
            
            // trigger all inputs
            if (_player.id == 0 && Input.GetKey(KeyCode.D))
                transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            if (_player.id == 0 && Input.GetKey(KeyCode.Q))
                transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
            if (_player.id == 0 && Input.GetKey(KeyCode.Z) && _timer <= 0) {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90);
                _timer = 0.1f;
            }
            if (_player.id == 0 && Input.GetKey(KeyCode.S) && _timer <= 0)
                rb.velocity = new Vector2(0, -5);
            if (_player.id == 1 && Input.GetKey(KeyCode.RightArrow))
                transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            if (_player.id == 1 && Input.GetKey(KeyCode.LeftArrow))
                transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
            if (_player.id == 1 && Input.GetKey(KeyCode.UpArrow) && _timer <= 0) {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90);
                _timer = 0.1f;
                
            }
            if (_player.id == 1 && Input.GetKey(KeyCode.DownArrow) && _timer <= 0)
                rb.velocity = new Vector2(0, -5);

            // reset timer
            if (_timer > 0) _timer -= Time.deltaTime;
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
            // rb.inertia = 0.1f;
            if (!isFalling) return;
            StopFalling();
        }
    }
}
