using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] Button readyButton;

    void Start()
    {
        readyButton.onClick.AddListener(() => { CharacterSelect.Instance.SetPlayerReady(); });
    }
}
