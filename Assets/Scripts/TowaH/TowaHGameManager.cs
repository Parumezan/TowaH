using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TowaH {
    public class TowaHGameManager : MonoBehaviour {
        public static TowaHGameManager instance;
        
        public List<CharacterProfile> availableCharacters = new List<CharacterProfile>();
        [SerializeField] private GameObject[] availableBlockPrefabs;
        [SerializeField] private Player[] playerControllers;
        
        public PlayerInfo[] players = new PlayerInfo[2];
        
        public GameObject[] AvailableBlockPrefabs => availableBlockPrefabs;

        public TowaHGameManager() {
            instance = this;
            
            players[0] = new PlayerInfo(0);
            players[1] = new PlayerInfo(1);
        }

        private void Awake() {
            Debug.Assert(availableCharacters.Count > 0, "Available characters is empty");
            Debug.Assert(availableBlockPrefabs.Length > 0, "Available block prefabs is empty");
        }

        public static void Quit() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        
        public void StartGame() {
            Debug.Log("Starting game");
            foreach (var player in playerControllers)
            {
                player.SpawnRandomBlock();
            }
        }
    }
}
