using UnityEngine;

namespace TowaH {
    public class Player : MonoBehaviour {
        [SerializeField] private Vector3 spawnBlockPosition;
        [SerializeField] private Quaternion spawnBlockRotation;

        public void SpawnRandomBlock() {
            Debug.Log("SpawnRandomBlock");

            int whichItem = Random.Range(0, TowaHGameManager.instance.AvailableBlockPrefabs.Length);
            GameObject blockPrefab = TowaHGameManager.instance.AvailableBlockPrefabs[whichItem];
            
            Instantiate(blockPrefab, transform.position + spawnBlockPosition, spawnBlockRotation);
        }
    }
}
