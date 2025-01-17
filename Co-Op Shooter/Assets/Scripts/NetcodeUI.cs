using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetcodeUI : MonoBehaviour
{
    [SerializeField] int goToIndex = 1;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();

        GoToScene();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();

        GoToScene();
    }

    void GoToScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != goToIndex)
        {
            GameManager.Instance.ChangeScene(goToIndex);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
