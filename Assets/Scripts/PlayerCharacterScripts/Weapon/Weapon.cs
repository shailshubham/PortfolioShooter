using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator anim;

    [SerializeField] InputData inputData;
    public WeaponData weaponData;
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
    bool flameActive = false;
    ParticleSystem flame;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        if(weaponData.weaponType != WeaponData.WeaponType.flameThrower)
        {
            muzzle.SetActive(false);
            Reloading = false;
            laser.SetActive(false);
        }
        {
            muzzle.SetActive(true);
            flame = muzzle.GetComponent<ParticleSystem>();
            flame.Stop();
        }

        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        canShoot = true;

    }

    private void Update()
    {
        if (weaponData.weaponType == WeaponData.WeaponType.flameThrower && flameActive&&!inputData.shoot)
        {
            flame.Pause();
            flame.loop = false;
            flame.Play();
            flameActive = false;
        }
    }

    public void Shoot()
    {
        if (!(inputData.Aim && canShoot && !Reloading))
            return;
        if (weaponData.currentMagzineCount>0)
        {
            if (inputData.shoot)
            {
                if (weaponData.weaponType != WeaponData.WeaponType.flameThrower)
                {
                    Invoke("ResetCanShoot", 1 / weaponData.fireRate);
                    audioSource.clip = fireAudioClip;
                    audioSource.Play();
                    weaponData.currentMagzineCount--;
                    laser.SetActive(false);
                    muzzle.SetActive(true);
                    canShoot = false;
                    anim.SetTrigger("Shoot");
                    RaycastHit hit;
                    if (Physics.Raycast(laser.transform.position, laser.transform.forward, out hit, weaponData.range, damageLayer))
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
                    if(!flameActive)
                    {
                        flame.loop = true;
                        flame.Play();
                        flameActive = true;
                    }

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
