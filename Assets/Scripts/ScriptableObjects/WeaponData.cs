using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyMenu/WeaponData")]
public class WeaponData : ScriptableObject
{
    public int index = 1;
    public string weaponName = "Hand Gun";
    public enum WeaponType { single,auto,boltAction,flameThrower}
    public WeaponType weaponType = WeaponType.single;
    public int damage = 10;
    public int defaultMagzineCount = 7;
    public int currentMagzineCount = 7;
    public float fireRate = 1.5f;
    public float range = 50f;
    public float verticleRecoil = 1f;
    public float horizontalRecoil = 1f;
}
