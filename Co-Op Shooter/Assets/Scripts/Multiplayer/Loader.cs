using Unity.Netcode;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        Ran_CharacterSelect,
        Ran_Game_Scene,
        Ran_Lobby
    }

    public static void NetworkLoad(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
}
