using UnityEngine;

namespace TowaH.Blocks {
    public class BlockPhysics : MonoBehaviour {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float fallingSpeed = 4f;
        
        private Rigidbody2D rb;
        private bool isFalling = true;
        private BlocksManager bn;

        private void Awake() {
            bn = GameObject.Find("GameMaster").GetComponent<BlocksManager>();
            Debug.Assert(bn != null, "BlockPhysics: BlocksManager not found");
            
            rb = gameObject.GetComponent<Rigidbody2D>();
            Debug.Assert(rb != null, "BlockPhysics: Rigidbody2D not found");
        }
    
        private void Update() {
            // Destroy block if it falls out of the screen
            if (transform.position.y < -10) {
                Destroy(gameObject);
                return;
            }

            if (!isFalling) {
                return;
            }

            if (gameObject.transform.position.x < -2) {
                gameObject.transform.position = new Vector2(-2, transform.position.y);
            }
            if (gameObject.transform.position.x > 2) {
                gameObject.transform.position = new Vector2(2, transform.position.y);
            }

            float x = Input.GetAxis("Horizontal");
            if (Input.GetAxis("Vertical") < 0) {
                rb.velocity = new Vector2(x * speed, -fallingSpeed * 2);
            } else {
                rb.velocity = new Vector2(x * speed, -fallingSpeed);
            }
        }
    
        private void StopFalling() {
            isFalling = false;
            rb.gravityScale = 1; 
        }

        //TODO: remove spawn, spawn it in server
        private void OnCollisionEnter2D(Collision2D collision) {
            if (!isFalling) {
                return;
            }
            
            StopFalling();
            bn.SpawnRandomObject();
        }
    }
}
