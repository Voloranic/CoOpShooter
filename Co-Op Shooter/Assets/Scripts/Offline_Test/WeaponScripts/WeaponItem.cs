using UnityEngine;

public class WeaponItem : MonoBehaviour, IPickable
{
    [SerializeField] WeaponScriptableObject weaponScriptableObject;

    bool isShowingInfo = false;

    private void Start() { 
        gameObject.name = weaponScriptableObject.WeaponName() + "_Item";
    }

    public void Pickup(WeaponHolder weaponHolder)
    {
        weaponHolder.AddWeapon(weaponScriptableObject);
        
        Destroy(gameObject);
    }

    public void ShowInfo()
    {
        if (!isShowingInfo) {
            isShowingInfo = true;

            Debug.Log($"Weapon name: {weaponScriptableObject.WeaponName()}, Weapon type: {weaponScriptableObject.WeaponType()}");
        }
    }

    public void HideInfo()
    {
        if (isShowingInfo)
        {
            isShowingInfo = false;
            Debug.Log("Hide weapon info");
        }
    }
}
