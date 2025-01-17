using Unity.Netcode;
using UnityEngine;

public class NetcodeUI : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();

        gameObject.SetActive(false);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();

        gameObject.SetActive(false);
    }
}
