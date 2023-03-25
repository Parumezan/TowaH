using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowMovement : MonoBehaviour
{
    public float speed = 15;
    public float timer = 0;
    public float cd = 0;
    private float h = 0;
    private float v = 0;
    // Start is called before the first frame update
    void Start()
    {
        h = Random.Range(0, 3);
        v = Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
