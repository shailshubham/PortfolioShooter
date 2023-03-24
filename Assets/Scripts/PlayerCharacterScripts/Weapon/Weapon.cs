using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator anim;

    [SerializeField] InputData inputData;
    [SerializeField] WeaponData weaponData;
    [SerializeField] GameObject muzzle;
    [SerializeField] float reloadTime = 1.5f;
    [SerializeField] AudioClip fireAudioClip;
    [SerializeField] AudioClip reloadingAudioClip;
    AudioSource audioSource;
    [HideInInspector] public bool Reloading = false;
    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        muzzle.SetActive(false);
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    public void Shoot()
    {
        if (!(inputData.Aim && inputData.shoot && canShoot && !Reloading))
            return;

        if (weaponData.currentMagzineCount>0)
        {
            anim.SetTrigger("Shoot");
            Invoke("ResetCanShoot", 1 / weaponData.fireRate);
            audioSource.clip = fireAudioClip;
            audioSource.Play();
            weaponData.currentMagzineCount--;
            muzzle.SetActive(true);
            canShoot = false;
        }
        else
        {
            Reload();
        }
    }
    private void ResetCanShoot()
    {
        canShoot = true;
        muzzle.SetActive(false);
    }

    public void Reload()
    {
        if (!(weaponData.currentMagzineCount < weaponData.defaultMagzineCount && inputData.reload && !Reloading))
            return;

        Reloading = true;
        anim.SetTrigger("Reload");
        audioSource.clip = reloadingAudioClip;
        audioSource.Play();
        weaponData.currentMagzineCount = weaponData.defaultMagzineCount;
        Invoke("ResetReloadingBool", reloadTime);
    }

    void ResetReloadingBool()
    {
        Reloading = false;
    }
}
