using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class WeaponAmmoDisplay : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI text;
    [SerializeField] Image image;
    WeaponSystem weaponSystem;
    [SerializeField] GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        weaponSystem = GameManager.instance.player.GetComponent<WeaponSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponSystem.CurrentWeapon == null)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
            text.text = weaponSystem.CurrentWeapon.weaponData.currentMagzineCount.ToString();
            image.sprite = weaponSystem.CurrentWeapon.weaponData.icon;
        }
    }
}
