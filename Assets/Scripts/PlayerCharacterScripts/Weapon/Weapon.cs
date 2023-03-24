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
    bool Reloading = false;
    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inputData.Aim&&inputData.shoot&&canShoot&&!Reloading)
        {
            Shoot();
            canShoot = false;
        }
        if(weaponData.currentMagzineCount<weaponData.defaultMagzineCount && inputData.reload&&!Reloading)
        {
            Reload();
        }
    }

    public void Shoot()
    {
        if(weaponData.currentMagzineCount>0)
        {
            anim.SetTrigger("Shoot");
            Invoke("ResetCanShoot", 1 / weaponData.fireRate);
            audioSource.clip = fireAudioClip;
            audioSource.Play();
            weaponData.currentMagzineCount--;
        }
        else
        {
            Reload();
        }
    }
    private void ResetCanShoot()
    {
        canShoot = true;

    }

    public void Reload()
    {
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
