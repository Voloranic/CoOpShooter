using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    //Inventory works strange with no weapons in it
    //When player throws a weapon he needs to equip the weapon before it

    //Update visuals
    public event EventHandler OnChangeWeapon;
    public event EventHandler OnWeaponFire;

    public event EventHandler OnStartReload;
    public event EventHandler OnStopReload;

    //Weapon functions
    public Action shootFunction;
    public Action stopShootFunction;
    public Action reloadFunction;

    [SerializeField] KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;
    //[SerializeField] KeyCode throwKeyCode = KeyCode.Z;

    [SerializeField] Weapon weaponPrefab;

    [SerializeField] List<WeaponScriptableObject> weaponScriptableObjectsInventory = new List<WeaponScriptableObject>();
    List<GameObject> weaponsInventory = new List<GameObject>();
    int currentWeaponIndex = -1;

    [SerializeField] bool enableWeaponKeyboardSwap = true;
    [SerializeField] bool enableWeaponScroll = true;

    public void Start()
    {
        foreach (WeaponScriptableObject weaponSO in weaponScriptableObjectsInventory)
        {
            AddWeapon(weaponSO, true);
        }

        ChangeWeapon(0);
    }

    private void Update()
    {
        HandleWeaponKeys();

        HandleWeaponSwap();
    }

    public void AddWeapon(WeaponScriptableObject weaponSO, bool setInventoryAtStart = false) {

        Weapon newWeaponScript = Instantiate(weaponPrefab, transform.root.position, new Quaternion(0f, 0f, 0f, 0f), transform);

        GameObject newWeapon = Instantiate(weaponSO.WeaponSpritePrefab(), newWeaponScript.transform).transform.parent.gameObject;
        newWeaponScript.SetWeapon(this, weaponSO);

        if (!setInventoryAtStart) { 
            weaponScriptableObjectsInventory.Add(weaponSO);
        }

        weaponsInventory.Add(newWeapon);

        newWeapon.SetActive(false);

        if (!setInventoryAtStart) { 
            ChangeWeapon(weaponsInventory.Count - 1);
        }
    }

    private void ChangeWeapon(int index)
    {
        if (index < 0 || index >= weaponScriptableObjectsInventory.Count)
        {
            Debug.Log("Weapon index out of weapons list's range");
            return;
        }

        if (index == currentWeaponIndex)
        {
            Debug.Log("Current Weapon");
            return;
        }

        if (currentWeaponIndex != -1)
        {
            //Set off current weapon
            CurrentWeaponScript().UnsubscribeFromWeaponHolder();

            weaponsInventory[currentWeaponIndex].SetActive(false);
        }

        //Update the weapon index
        currentWeaponIndex = index;

        //Activate new weapon
        weaponsInventory[index].SetActive(true);

        CurrentWeaponScript().SubscribeToWeaponHolder();

        //Fire the OnChangeWeapon event
        OnChangeWeapon?.Invoke(gameObject, EventArgs.Empty);
    }

    private void HandleWeaponKeys()
    {
        if (Input.GetKeyDown(fireKey))
        {
            shootFunction?.Invoke();
        }
        if (Input.GetKeyUp(fireKey))
        {
            stopShootFunction?.Invoke();
        }

        if (Input.GetKeyDown(reloadKey))
        {
            reloadFunction?.Invoke();
        }
    }

    private void HandleWeaponSwap()
    {
        if (enableWeaponKeyboardSwap)
        {
            for (int i = 0; i < weaponScriptableObjectsInventory.Count; i++)
            {
                //KeyCode.Alpha1 represents '1' and its numeric value is 49. The KeyCode after Alpha1 is Alpha2, which represents '2' and its numeric value is 50 and so on.
                if (Input.GetKeyDown(KeyCode.Alpha1 + i) && i != currentWeaponIndex)
                {
                    ChangeWeapon(i);
                }
            }
        }

        if (enableWeaponScroll)
        {
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if (scrollWheel == 0) return;

            int targetIndex = currentWeaponIndex + Math.Sign(scrollWheel);

            if (targetIndex < 0)
            {
                targetIndex += weaponScriptableObjectsInventory.Count;
            }
            else if (targetIndex >= weaponScriptableObjectsInventory.Count)
            {
                targetIndex -= weaponScriptableObjectsInventory.Count;
            }

            ChangeWeapon(targetIndex);
        }
    }

    public WeaponScriptableObject CurrentWeaponScriptableObject()
    {
        return weaponScriptableObjectsInventory[currentWeaponIndex];
    }
    public Weapon CurrentWeaponScript()
    {
        if (weaponsInventory.Count == 0)
        {
            Debug.Log("weaponInventory length is set to 0");
            return null;
        }

        return weaponsInventory[currentWeaponIndex].GetComponent<Weapon>();
    }

    public void OnWeaponFireEvent()
    {
        OnWeaponFire?.Invoke(gameObject, EventArgs.Empty);
    }
    public void OnStartReloadEvent()
    {
        OnStartReload?.Invoke(gameObject, EventArgs.Empty);
    }
    public void OnStopReloadEvent()
    {
        OnStopReload?.Invoke(gameObject, EventArgs.Empty);
    }
}
