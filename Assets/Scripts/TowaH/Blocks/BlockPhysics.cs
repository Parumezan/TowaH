using UnityEngine;

namespace TowaH.Blocks {
    public class BlockPhysics : MonoBehaviour {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float fallingSpeed = 1f;

        private Rigidbody2D rb;
        private bool isFalling = true;
        private Vector3 _startPosition;
        private Player _player;

        private void Awake() {
            rb = gameObject.GetComponent<Rigidbody2D>();
            Debug.Assert(rb != null, "BlockPhysics: Rigidbody2D not found");
            _startPosition = transform.position;
        }
    
        private void Update() {
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
            
            float x = Input.GetAxis("Horizontal");
            if (Input.GetAxis("Vertical") < 0) {
                rb.velocity = new Vector2(x * speed, -fallingSpeed * 2);
            } else {
                rb.velocity = new Vector2(x * speed, -fallingSpeed);
            }
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
        
        private void OnCollisionEnter2D(Collision2D collision) {
            if (!isFalling) {
                return;
            }
            StopFalling();
        }
    }
}
