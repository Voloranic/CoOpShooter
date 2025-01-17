using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ResetStaticData();
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    void ResetStaticData()
    {
        PlayerMovement.ResetStaticData();
    }
}
