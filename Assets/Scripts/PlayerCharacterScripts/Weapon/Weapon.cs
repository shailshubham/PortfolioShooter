using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator anim;

    [SerializeField] InputData inputData;
    [SerializeField] WeaponData weaponData;
    [SerializeField] ItemData ammoData;
    [SerializeField] GameObject muzzle;
    [SerializeField] GameObject laser;
    [SerializeField] float reloadTime = 1.5f;
    [SerializeField] AudioClip fireAudioClip;
    [SerializeField] AudioClip reloadingAudioClip;
    [SerializeField] LayerMask damageLayer;
    AudioSource audioSource;
    Camera mainCamera;
    [HideInInspector] public bool Reloading = false;
    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        muzzle.SetActive(false);
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        canShoot = true;
        Reloading = false;
        laser.SetActive(false);
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
            laser.SetActive(false);
            muzzle.SetActive(true);
            canShoot = false;

            RaycastHit hit;
            if(Physics.Raycast(laser.transform.position, laser.transform.forward,out hit, weaponData.range, damageLayer))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.GetComponent<IHealth>().TakeDamage(weaponData.damage);
                    Debug.Log("ShotSuccessfully");
                }

            }

        }
        else
        {
            inputData.reload = true;
            Reload();
        }
    }
    private void ResetCanShoot()
    {
        canShoot = true;
        muzzle.SetActive(false);
        laser.SetActive(true);
    }

    public void Reload()
    {
        if (!(weaponData.currentMagzineCount<weaponData.defaultMagzineCount&& inputData.reload && !Reloading))
            return;
        if(Inventory.instance.UseConsumableItem(ammoData,weaponData.defaultMagzineCount-weaponData.currentMagzineCount,out int availableMagCount))
        {
            Reloading = true;
            anim.SetTrigger("Reload");
            laser.SetActive(false);
            audioSource.clip = reloadingAudioClip;
            audioSource.Play();
            weaponData.currentMagzineCount += availableMagCount;
            Invoke("ResetReloadingBool", reloadTime);
            inputData.reload = false;
        }

    }

    void ResetReloadingBool()
    {
        Reloading = false;
        laser.SetActive(true);
    }

    public GameObject Laser
    {
        get { return laser; }
    }
}
