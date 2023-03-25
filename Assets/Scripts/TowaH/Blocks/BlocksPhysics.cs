using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksPhysics : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 0;
    private float falling_speed = 0;
    bool falling = true;
    public BlocksManager bn;

    // Start is called before the first frame update
    void Start()
    {
        bn = GameObject.Find("GameMaster").GetComponent<BlocksManager>();
        speed = 5;
        falling_speed = 4f;
        falling = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (falling) {
            float x = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(x * speed, -falling_speed);
        }
    }

    void StopFalling()
    {
        falling = false;
        rb.gravityScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopFalling();
        bn.SpawnRandomObject();
    }
}
