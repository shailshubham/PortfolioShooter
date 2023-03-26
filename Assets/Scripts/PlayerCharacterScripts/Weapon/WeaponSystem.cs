using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSystem : MonoBehaviour
{
    bool isWeaponEquipped = false;
    public bool IsWeaponEquipped
    {
        get { return isWeaponEquipped; }
        set { isWeaponEquipped = true; }
    }

    bool isHandsBusy = false;

    public bool IsHandsBusy
    {
        get { return isHandsBusy; }
        set { isHandsBusy = value; }
    }

    Animator currentWeaponAnim;
    public Animator CurrentWeaponAnim
    {
        get { return currentWeaponAnim; }
        set { currentWeaponAnim = value; }
    }

    Weapon currentWeapon;
    public Weapon CurrentWeapon
    {
        get { return currentWeapon; }
        set { currentWeapon = value; }
    }

    public GameObject[] Weapons;


    private void Awake()
    {
        InitialActiveWeaponCheck();
    }
    private void Update()
    {

    }

    public void ChangeWeaponSystem()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            DeActivateWeapon();
            currentWeapon = null;
            currentWeaponAnim = null;
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ActivateWeapon(0);
        }
    }

    void ActivateWeapon(int index)
    {
        for(int i = 0; i< Weapons.Length;i++)
        {
            if(i == index)
            {
                Weapons[i].SetActive(true);
                currentWeaponAnim = Weapons[i].GetComponent<Animator>();
                currentWeapon = Weapons[i].GetComponent<Weapon>();
                isWeaponEquipped = true;
            }
            else
            {
                Weapons[i].SetActive(false);
            }
        }
    }
    void DeActivateWeapon()
    {
        foreach (GameObject weapon in Weapons)
        {
            weapon.SetActive(false);
            isWeaponEquipped = false;
        }
    }

    private void InitialActiveWeaponCheck()
    {
        for(int i=0; i< Weapons.Length;i++)
        {
            if(Weapons[i].activeSelf)
            {
                currentWeaponAnim = Weapons[i].GetComponent<Animator>();
                currentWeapon = Weapons[i].GetComponent<Weapon>();
                isWeaponEquipped = true;
            }
        }
    }
}
