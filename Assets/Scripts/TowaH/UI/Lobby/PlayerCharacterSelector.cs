using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterSelector : MonoBehaviour {
    public Button leftButton;
    public Button rightButton;
    public Image characterImage;

    private void Awake() {
        Debug.Assert(leftButton != null, "Left button is null");
        Debug.Assert(rightButton != null, "Right button is null");
        Debug.Assert(characterImage != null, "Character image is null");
    }
}
