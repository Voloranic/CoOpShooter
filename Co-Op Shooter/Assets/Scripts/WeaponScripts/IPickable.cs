
using UnityEngine;

public interface IPickable
{
    void Pickup(WeaponHolder weaponHolder);

    void ShowInfo();

    void HideInfo();
}