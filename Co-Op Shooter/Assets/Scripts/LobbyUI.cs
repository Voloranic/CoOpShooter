using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button createButton;
    [SerializeField] Button joinButton;

    private void Awake()
    {
        createButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            Loader.NetworkLoad(Loader.Scene.Ran_CharacterSelect);
        } );

        joinButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }


}
