using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksPhysics : MonoBehaviour
{
    Rigidbody2D rb;
    private float speed = 0;
    private float falling_speed = 0;
    public bool falling;
    public BlocksManager bn;

    // Start is called before the first frame update
    void Start()
    {
        falling = false;
        bn = GameObject.Find("GameMaster").GetComponent<BlocksManager>();
        speed = 5;
        falling_speed = 4f;
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.Log("mlkjsd");
        rb.bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < -2)
            gameObject.transform.position = new Vector2(-2, transform.position.y);
        if (gameObject.transform.position.x > 2)
            gameObject.transform.position = new Vector2(2, transform.position.y);
        if (falling) {
            float x = Input.GetAxis("Horizontal");
            if (Input.GetAxis("Vertical") < 0)
                rb.velocity = new Vector2(x * speed, -falling_speed * 2);
            else
                rb.velocity = new Vector2(x * speed, -falling_speed);
        }
    }

    public void LaunchBlock()
    {
        gameObject.transform.position = new Vector2(0, 8);
        falling = true;
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void StopFalling()
    {
        falling = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (falling == true)
        {
            StopFalling();
            bn.SpawnRandomObject();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("death"))
        {
            StopFalling();
            bn.SpawnRandomObject();
            Debug.Log("azer");
            Destroy(gameObject);
        }
    }
}
