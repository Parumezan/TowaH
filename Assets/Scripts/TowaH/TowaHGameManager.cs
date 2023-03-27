using System;
using System.Collections.Generic;
using TMPro;
using TowaH.Blocks;
using UnityEditor;
using UnityEngine;

namespace TowaH {
    public class TowaHGameManager : MonoBehaviour {
        public static TowaHGameManager instance;
        public List<CharacterProfile> availableCharacters = new List<CharacterProfile>();
        [SerializeField] private GameObject[] availableBlockPrefabs;
        [SerializeField] private Player[] playerControllers;
        [SerializeField] public SpriteRenderer[] playerSprites;
        [SerializeField] private float timer;
        public PlayerInfo[] players = new PlayerInfo[2];
        public GameObject[] AvailableBlockPrefabs => availableBlockPrefabs;
        private bool isGameStarted = false;
        private bool JESUISUNTIMERLACONDEBOOLEAN = false;

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

        private void Update() {
            TextMeshPro timerText = GameObject.Find("Timer").GetComponent<TextMeshPro>();
            
            if (JESUISUNTIMERLACONDEBOOLEAN) JeSuisUnTimerLaConDeTaRace();
            if (!isGameStarted) return;
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = 0;
                int id = 0;
                float highest = 0;
                GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
                foreach (var block in blocks)
                {
                    BlockPhysics blockPhysics = block.GetComponent<BlockPhysics>();
                    if ((block.transform.position.y + 233.95) > highest && !blockPhysics.isFalling)
                    {
                        highest = block.transform.position.y;
                        id = blockPhysics._player.id;
                    }
                    else if (blockPhysics.isFalling)
                    {
                        Destroy(block);
                    }
                }
                timerText.text = "Player " + (id + 1) + " wins! Tower Height: " + (highest + 233.95) + "m";
                isGameStarted = false;
                return;
            }
            int newTimer = (int) timer;
            timerText.text = newTimer.ToString();
        }

        private void JeSuisUnTimerLaConDeTaRace() {
            RectTransform timerRect = GameObject.Find("Timer").GetComponent<RectTransform>();
            timerRect.position = new Vector3(timerRect.position.x, Mathf.Lerp(timerRect.position.y, -207, 0.03f), timerRect.position.z);
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = 90f;
                TextMeshPro timerText = GameObject.Find("Timer").GetComponent<TextMeshPro>();
                timerText.text = timer.ToString(); 
                isGameStarted = true;
                JESUISUNTIMERLACONDEBOOLEAN = false;
            }
        }

        public void StartGame() {
            Debug.Log("Starting game");
            foreach (var player in playerControllers)
                player.SpawnRandomBlock();
            timer = 3f;
            JESUISUNTIMERLACONDEBOOLEAN = true;
        }
    }
}
