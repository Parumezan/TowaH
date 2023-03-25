using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlocksManager : MonoBehaviour
{
    [SerializeField] private GameObject[] blocks;

    //TODO: remove spawn, spawn it in server
    private void Start() {
        SpawnRandomObject();
    }

    public void SpawnRandomObject()
    {
        int whichItem = Random.Range(0, blocks.Length);
        
        GameObject newBlock = Instantiate(blocks[whichItem], new Vector3(0, 8, 0), Quaternion.identity);
    }
}
