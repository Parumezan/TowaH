using UnityEngine;
using System.IO;

namespace TowaH {
    public class GameConsoleHUD : MonoBehaviour {
        [Header("Console")]
        [SerializeField] private float offsetX = 200;
        [SerializeField] private float offsetY = 290;
        [SerializeField] private float windowX = 700;
        [SerializeField] private float windowY = 200;
        public static GameConsoleHUD instance;
        private const int MAX_LOG_SIZE = 1024;
        private bool IS_CONSOLE = false;
        private string command;
        private string debugLog;
        private Vector2 scrollPos;
        private Rect WindowRect;

        private void Awake() {
            instance = this;
            WindowRect = new Rect(offsetX, offsetY, windowX, windowY);
        }

        private void OnGUI() {
            if (IS_CONSOLE) DrawGui();
        }

        private void proceedCommand(string command) {
            switch (command) {
                case "/help":
                    string help = "Commands:\n";
                    help += "/help - show this message\n";
                    help += "/clear - clear console\n";
                    help += "/quit - quit game\n";
                    debugLog += help;
                    break;
                case "/clear":
                    debugLog = "";
                    break;
                case "/quit":
                    Application.Quit();
                    break;
                default:
                    debugLog += "Unknown command: " + command;
                    break;
            }
            debugLog += "\n";
        }

        private bool IsMouseOverWindow() {
            Vector2 MousePos = Event.current.mousePosition;
            if (MousePos.x > 0 && MousePos.x < WindowRect.width && MousePos.y > 0 && MousePos.y < WindowRect.height)
                return true;
            return false;
        }

        private void DrawGui() {
            GUILayout.BeginArea(WindowRect, new GUIStyle(GUI.skin.box) {
                normal = {
                    background = Texture2D.grayTexture
                }
            });

            GUILayout.Label("Console", new GUIStyle(GUI.skin.label) {
                alignment = TextAnchor.MiddleCenter
            });

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.TextArea(debugLog);
            GUILayout.EndScrollView();

            command = GUILayout.TextField(command);

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return) {
                proceedCommand(command);
                command = "";
            }

            GUILayout.EndArea();
        }   

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F12)) {
                IS_CONSOLE = !IS_CONSOLE;
            }
            if (IS_CONSOLE) {
                WindowRect.x = offsetX;
                WindowRect.y = offsetY;
                WindowRect.width = windowX;
                WindowRect.height = windowY;
            }
        }

        public static void Debug(string message) {

        }
    }
}