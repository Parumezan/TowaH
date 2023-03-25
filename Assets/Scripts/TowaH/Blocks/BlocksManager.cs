using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    public List<GameObject> blocks = new List<GameObject>();
    GameObject nextBlock;
    // Start is called before the first frame update
    void Start()
    {
        Object[] subListObjects = Resources.LoadAll("Prefabs", typeof(GameObject));
        foreach (GameObject subListObject in subListObjects)
        {
            GameObject lo = (GameObject)subListObject;

            blocks.Add(lo);
        }
        SpawnRandomObject();
        SpawnRandomObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SpawnRandomObject();
    }

    public void SpawnRandomObject()
    {
        int whichItem = Random.Range(0, 5);
        if (nextBlock != null)
            nextBlock.GetComponent<BlocksPhysics>().LaunchBlock();
        nextBlock = Instantiate(blocks[whichItem]) as GameObject;
        nextBlock.transform.position = gameObject.transform.position;
    }
}
