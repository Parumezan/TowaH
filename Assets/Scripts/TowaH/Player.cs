using UnityEngine;

namespace TowaH {
    public class Player : MonoBehaviour {
        [SerializeField] private Vector3 spawnBlockPosition;
        [SerializeField] private Quaternion spawnBlockRotation;
        
        private PlayerInfo playerInfo;

        public void SpawnRandomBlock() {
            Debug.Log("SpawnRandomBlock");
            Debug.Assert(playerInfo != null, "playerConnection != null");
            
            int whichItem = Random.Range(0, TowaHGameManager.instance.AvailableBlockPrefabs.Length);
            GameObject blockPrefab = TowaHGameManager.instance.AvailableBlockPrefabs[whichItem];
            
            Instantiate(blockPrefab, transform.position + spawnBlockPosition, spawnBlockRotation);
        }
    }
}
