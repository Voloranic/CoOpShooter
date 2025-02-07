using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Scriptable Objects/Weapon SO")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] string weaponName;

    [SerializeField] Types.WeaponTypes weaponType;

    [SerializeField] GameObject weaponSpritePrefab;

    [SerializeField] GameObject weaponItemPrefab;

    [SerializeField] int magazineSize;
    [SerializeField] int magazineClipSize;

    [SerializeField] float fireRate;
    [SerializeField] float reloadTime;
    [SerializeField] float equipTime;

    [SerializeField] float distanceFromHolder;

    [SerializeField] float maxDistance;

    public string WeaponName()
    {
        return weaponName;
    }

    public Types.WeaponTypes WeaponType()
    {
        return weaponType;
    }

    public GameObject WeaponSpritePrefab()
    {
        return weaponSpritePrefab;
    }

    public GameObject WeaponItemPrefab()
    {
        return weaponItemPrefab;
    }

    public int MagazineSize()
    {
        return magazineSize;
    }
    public int MagazineClipSize()
    {
        return magazineClipSize;
    }

    public float FireRate()
    {
        return fireRate;
    }
    public float ReloadTime()
    {
        return reloadTime;
    }
    public float EquipTime()
    {
        return equipTime;
    }

    public float DistanceFromHolder() { 
        return distanceFromHolder;
    }

    public float MaxDistance()
    {
        return maxDistance;
    }
}
