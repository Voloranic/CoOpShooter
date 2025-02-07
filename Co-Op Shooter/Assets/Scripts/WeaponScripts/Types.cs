using UnityEngine;

public class Types : MonoBehaviour
{
    public static Types Instance;

    public enum WeaponTypes
    {
        Auto,
        Manual,
        Shotgun
    }

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
    }
}
