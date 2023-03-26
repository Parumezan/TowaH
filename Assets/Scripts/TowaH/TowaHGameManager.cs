using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TowaH {
    public class TowaHGameManager : MonoBehaviour {
        public static TowaHGameManager instance;
        
        [SerializeField] private TowaHNetworkManager networkManager;
        public List<CharacterProfile> availableCharacters = new List<CharacterProfile>();
        [SerializeField] private GameObject[] availableBlockPrefabs;
        [SerializeField] private Player[] players;
        
        public GameObject[] AvailableBlockPrefabs => availableBlockPrefabs;

        public TowaHGameManager() {
            instance = this;
        }

        private void Awake() {
            Debug.Assert(networkManager != null, "Network manager is null");
            Debug.Assert(availableCharacters.Count > 0, "Available characters is empty");
            Debug.Assert(availableBlockPrefabs.Length > 0, "Available block prefabs is empty");
        }
        
        private void Start() {
            networkManager.onServerStartGame.RemoveAllListeners();
            networkManager.onServerStartGame.AddListener(OnServerStartGame);
        }
        
        public static void Quit() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        
        #region Server
        
        private void OnServerStartGame() {
            Debug.Log("TowaHGameManager: OnServerStartGame");

            Debug.Log("Assigning connections to players");
            foreach (PlayerInfo player in networkManager.players.Values) {
                players[player.id].AssignConnection(player);
            }
            
            players[0].SpawnRandomBlock();
        }
        
        #endregion
    }
}
