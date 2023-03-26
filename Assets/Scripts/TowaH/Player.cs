using TowaH.Blocks;
using UnityEngine;

namespace TowaH {
    public class Player : MonoBehaviour {
        [SerializeField] private Vector3 spawnBlockPosition;
        [SerializeField] private Quaternion spawnBlockRotation;
        [SerializeField] public int id;

        public void SpawnRandomBlock() {
            int whichItem = Random.Range(0, TowaHGameManager.instance.AvailableBlockPrefabs.Length);
            GameObject blockPrefab = TowaHGameManager.instance.AvailableBlockPrefabs[whichItem];

            GameObject block = Instantiate(blockPrefab, transform.position + spawnBlockPosition, spawnBlockRotation);
            block.GetComponent<BlockPhysics>().AssignPlayer(this);
        }
    }
}
