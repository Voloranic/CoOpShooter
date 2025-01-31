using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Types;

public class Weapon : MonoBehaviour
{
    WeaponHolder weaponHolder;

    WeaponScriptableObject weaponScriptableObject;

    int magazineBullets;
    int clipBullets;
    bool isReloading;
    bool isEquipping;
    float equipTimer;

    float fireRateTimer;
    bool isShooting = false;

    Vector2 originalPos => weaponHolder.transform.position;

    //When the weapon is thrown it stops reloading but doesn't complete reloading

    public void SetWeapon(WeaponHolder holder, WeaponScriptableObject weaponSO)
    {
        weaponHolder = holder;

        //Set the weapon data
        weaponScriptableObject = weaponSO;

        //Set the bullets according to the data
        magazineBullets = weaponSO.MagazineSize();
        clipBullets = weaponSO.MagazineClipSize();

        //Set the name for readability
        gameObject.name = weaponSO.WeaponName();
    }

    public void SubscribeToWeaponHolder()
    {
        if (weaponHolder == null)
        {
            Debug.Log("Weapon holder at " + gameObject.name + " is null.");
            return;
        }

        HandlePositionAndRotation();

        weaponHolder.shootFunction += StartShoot;
        weaponHolder.stopShootFunction += StopShoot;

        weaponHolder.reloadFunction += Reload;

        //Disable holder's ability to fire when equipped
        equipTimer = weaponScriptableObject.EquipTime();
        isEquipping = true;
        fireRateTimer = weaponScriptableObject.FireRate();
    }

    public void UnsubscribeFromWeaponHolder()
    {
        if (weaponHolder == null)
        {
            Debug.Log("Weapon holder at " + gameObject.name + " is null.");
            return;
        }

        weaponHolder.shootFunction -= StartShoot;
        weaponHolder.stopShootFunction -= StopShoot;

        weaponHolder.reloadFunction -= Reload;

        //Stop shooting
        StopShoot();
        //Cancel reloading
        StopReloading();
        //Cancel equipping
        isEquipping = false;
    }

    private void Update()
    {
        HandleShoot();
    }

    private void FixedUpdate()
    {
        HandlePositionAndRotation();
    }

    private void HandlePositionAndRotation()
    {
        transform.position = originalPos;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (worldMousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        transform.position += transform.right * weaponScriptableObject.DistanceFromHolder();

        float xRotation = worldMousePosition.x - originalPos.x < 0 ? 180f : 0f;

        Transform spritesParent = transform.GetChild(0);
        spritesParent.localRotation = new Quaternion(xRotation, 0f, 0f, 0f);

        Vector3 origin = transform.position + transform.right * 0.2f;
        Debug.DrawRay(origin, transform.right * weaponScriptableObject.MaxDistance(), Color.green);
    }

    private void HandleShoot()
    {
        if (isEquipping)
        {
            equipTimer -= Time.deltaTime;

            if (equipTimer < 0)
            {
                isEquipping = false;
            }
        }
        else
        {
            fireRateTimer += Time.deltaTime;

            if (isShooting && fireRateTimer > weaponScriptableObject.FireRate())
            {
                fireRateTimer = 0f;
                Fire();

                if (weaponScriptableObject.WeaponType() == WeaponTypes.Manual)
                {
                    StopShoot();
                }
            }
        }
    }

    private void Fire()
    {
        if (magazineBullets <= 0)
        {
            Reload();
            return;
        }

        weaponHolder.OnWeaponFireEvent();

        magazineBullets--;

        Vector3 origin = transform.position + transform.right * 0.2f;
        RaycastHit2D shootRay = Physics2D.Raycast(origin, transform.right, weaponScriptableObject.MaxDistance());

        if (shootRay.collider != null) {
            Debug.Log(shootRay.transform.name);
        }
    }

    private void StartShoot()
    {
        if (isReloading || isEquipping) return;

        if (magazineBullets > 0)
        {
            if (weaponScriptableObject.WeaponType() == WeaponTypes.Manual)
            {
                //If the holder tried to shoot while the fire rate is active it will not trigger the isShooting
                if (fireRateTimer > 0.3f)
                {
                    isShooting = true;
                }
            }
            else
            {
                isShooting = true;
            }
        }
        else
        {
            Reload();
        }

    }

    private void StopShoot()
    {
        isShooting = false;
    }

    private void Reload()
    {
        if (isReloading || magazineBullets >= weaponScriptableObject.MagazineSize() || clipBullets <= 0) return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        weaponHolder.OnStartReloadEvent();

        yield return new WaitForSeconds(weaponScriptableObject.ReloadTime());

        //If there are less bullets in the weapon than the magazine size
        if (clipBullets + magazineBullets < weaponScriptableObject.MagazineSize())
        {
            //Get all the bullets from the clip and move them to the current magazine
            magazineBullets += clipBullets;
            clipBullets = 0;
        }
        else
        {
            //Remove from the clip the bullets needed to complete the magazine and set the magazine to its fullest
            clipBullets -= (weaponScriptableObject.MagazineSize() - magazineBullets);
            magazineBullets = weaponScriptableObject.MagazineSize();
        }

        StopReloading();
    }

    private void StopReloading()
    {
        isReloading = false;
        weaponHolder.OnStopReloadEvent();
    }

    public int MagazineBullets()
    {
        return magazineBullets;
    }
    public int ClipBullets()
    {
        return clipBullets;
    }

}
