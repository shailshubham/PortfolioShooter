using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName = "item name";
    public int itemID = 0;
    public enum ItemType { consoumable,non_consumable}
    public ItemType itemType;
    public Sprite sprite;
    public int amount = 0;
    public int amountLimitPerSlot = 50;
    public int amountOnObjectPick = 15;
    public bool isAvailable = false;
}
