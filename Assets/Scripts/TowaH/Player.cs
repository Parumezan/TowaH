using Mirror;
using UnityEngine;

namespace TowaH {
    public class Player : MonoBehaviour {
        [SerializeField] private bool hasAuthority;
        [SerializeField] private Vector3 spawnBlockPosition;
        [SerializeField] private Quaternion spawnBlockRotation;
        
        private PlayerInfo playerInfo;
        private TowaHGameManager gameManager;
        
        private void Awake() {
            gameManager = FindObjectOfType<TowaHGameManager>();
            Debug.Assert(gameManager != null, "TowaHGameManager != null");
        }
        
        public void AssignConnection(PlayerInfo playerInfo) {
            Debug.Assert(playerInfo != null, "playerInfo != null");
            this.playerInfo = playerInfo;
        }

        public void SpawnRandomBlock() {
            if (!hasAuthority) {
                return;
            }
            
            Debug.Log("SpawnRandomBlock");
            Debug.Assert(playerInfo != null, "playerConnection != null");
            
            int whichItem = Random.Range(0, gameManager.AvailableBlockPrefabs.Length);
            GameObject blockPrefab = gameManager.AvailableBlockPrefabs[whichItem];
            
            GameObject block = Instantiate(blockPrefab, transform.position + spawnBlockPosition, spawnBlockRotation);
            Debug.Log("Spawning block " + block.name + " at " + block.transform.position + " with rotation " + block.transform.rotation);

            NetworkServer.Spawn(block);
        }
    }
}
