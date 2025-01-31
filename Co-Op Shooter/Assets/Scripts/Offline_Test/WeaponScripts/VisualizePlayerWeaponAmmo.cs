using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class VisualizePlayerWeaponAmmo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI magazineText;
    [SerializeField] TextMeshProUGUI clipText;

    [SerializeField] Color32 defaultTextColor = Color.white;
    [SerializeField] Color32 disabledTextColor = Color.gray;

    [SerializeField] WeaponHolder weaponHolder;

    private void Start()
    {
        if (weaponHolder != null)
        {
            weaponHolder.OnChangeWeapon += EventUpdateAmmo;

            weaponHolder.OnWeaponFire += EventUpdateAmmo;

            weaponHolder.OnStartReload += (object sender, EventArgs e) => {
                DisableAmmoTextColor();
            };

            weaponHolder.OnStopReload += (object sender, EventArgs e) => {
                magazineText.color = defaultTextColor;
                clipText.color = defaultTextColor;

                UpdateAmmo();
            };

            UpdateAmmo();
        }
        else
        {
            Debug.Log("weaponHolder is null");
        }

        magazineText.color = defaultTextColor;
        clipText.color = defaultTextColor;
    }

    private void UpdateAmmo()
    {
        StartCoroutine(UpdateAmmoRoutine());
    }
    private void EventUpdateAmmo(object sender, EventArgs e)
    {
        UpdateAmmo();
    }

    IEnumerator UpdateAmmoRoutine()
    {
        //Wait a frame for the ammo to update at the weapon script
        yield return new WaitForEndOfFrame();

        if (weaponHolder.CurrentWeaponScript() != null)
        {
            magazineText.text = weaponHolder.CurrentWeaponScript().MagazineBullets().ToString();
            clipText.text = weaponHolder.CurrentWeaponScript().ClipBullets().ToString();

            if (weaponHolder.CurrentWeaponScript().MagazineBullets() == 0)
            {
                DisableAmmoTextColor();
            }
        }
    }

    private void DisableAmmoTextColor()
    {
        magazineText.color = disabledTextColor;
        clipText.color = disabledTextColor;
    }
}
