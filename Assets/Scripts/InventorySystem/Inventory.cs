using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance !=this)
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] InputData input;
    public ItemData selectedItem;
    [SerializeField] GameObject itemIconPref;
    public GameObject draggableItemParent;
    [SerializeField] InventoryItemSlot[] itemSlots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool UseConsumableItem(ItemData itemData,int count,out int amountAvailable)
    {
        bool returnBool = false;
        amountAvailable = 0;
        if (itemData.itemType == ItemData.ItemType.consumable)
        {
            int remainingItem = count;
            while (remainingItem != 0&&isItemAvailable(itemData))
            {
                InventoryItemIcon itemIcon;
                if (CheckLeastConsumableItemInInventory(itemData, out itemIcon,out var itemSlot))
                {
                    if(remainingItem<=itemIcon.amount)
                    {
                        itemIcon.amount -= remainingItem;
                        itemData.amount -= remainingItem;
                        amountAvailable += remainingItem;
                        returnBool = true;
                        if(itemData.amount == 0)
                        {
                            itemSlot.containsItem = false;
                            itemSlot.itemData = null;
                            itemSlot.itemIcon = null;
                            Destroy(itemIcon.gameObject);
                        }
                    }
                    else
                    {
                        itemData.amount -= itemIcon.amount;
                        amountAvailable += itemIcon.amount;

                        itemSlot.containsItem = false;
                        itemSlot.itemData = null;
                        itemSlot.itemIcon = null;
                        Destroy(itemIcon.gameObject);
                    }
                }
            }
        }
        return returnBool;
    }
    public void AddItemToInventory(ItemData itemData,int count)
    {
        if(itemData.itemType == ItemData.ItemType.consumable)
        {
            AddConsumableItem(itemData,count);
        }
        else
        {
            AddItemInEmptySlot(itemData, itemData.amountOnObjectPick);
        }
    }

    void AddConsumableItem(ItemData itemData,int count)
    {
        int remainingItem = count; 
        while(remainingItem!=0&&IsSpaceAvailable(itemData))
        {
            int space;
            InventoryItemIcon itemIcon;
            if(CheckLeastSpaceConsumableItemInInventory(itemData,out space,out itemIcon))
            {
                if(count<space)
                {
                    itemIcon.amount += count;
                    remainingItem -= count;
                    itemData.amount += count;
                }
                else
                {
                    itemIcon.amount += space;
                    remainingItem -= space;
                    itemData.amount += space;
                }
            }
            else
            {
                if(remainingItem>itemData.amountLimitPerSlot)
                {
                    AddItemInEmptySlot(itemData, itemData.amountLimitPerSlot);
                    remainingItem -= itemData.amountLimitPerSlot;
                    itemData.amount += itemData.amountLimitPerSlot;
                }
                else
                {
                    AddItemInEmptySlot(itemData, remainingItem);
                    remainingItem -= remainingItem;
                    itemData.amount += remainingItem;
                }
            }
        }
    }

    void AddItemInEmptySlot(ItemData itemData,int amount)
    {
        InventoryItemSlot itemSlot;
        if (IsThereASlotEmpty(out itemSlot))
        {
            InventoryItemIcon itemIcon = Instantiate(itemIconPref).GetComponent<InventoryItemIcon>();
            itemIcon.transform.SetParent(itemSlot.transform);
            itemIcon.itemData = itemData;
            itemIcon.amount = amount;
            itemSlot.itemData = itemData;
            itemSlot.containsItem = true;
            itemSlot.itemIcon = itemIcon;
        }
    }
    bool CheckLeastSpaceConsumableItemInInventory(ItemData itemData, out int itemSpace, out InventoryItemIcon ItemIconWithLeastSpace)
    {
        bool returnBool = false;
        int leastSpace = 0;
        ItemIconWithLeastSpace = null;
        foreach (InventoryItemSlot itemSlot in itemSlots)
        {
            InventoryItemIcon itemtWithLeastSpace = null;
            if (!itemSlot.containsItem)
                continue;
            if (itemSlot.itemData.itemID == itemData.itemID && itemSlot.itemIcon.amount < itemData.amountLimitPerSlot)
            {
                if (itemtWithLeastSpace == null)
                {
                    itemtWithLeastSpace = itemSlot.itemIcon;
                    leastSpace = itemData.amountLimitPerSlot - itemSlot.itemIcon.amount;
                    ItemIconWithLeastSpace = itemtWithLeastSpace;
                    returnBool = true;
                    continue;
                }

                if ((itemData.amountLimitPerSlot - itemSlot.itemIcon.amount) < leastSpace)
                {
                    leastSpace = itemData.amountLimitPerSlot - itemSlot.itemIcon.amount;
                    itemtWithLeastSpace = itemSlot.itemIcon;
                    ItemIconWithLeastSpace = itemtWithLeastSpace;
                    returnBool = true;
                }

            }
        }
        itemSpace = leastSpace;
        return returnBool;
    }
    bool CheckLeastConsumableItemInInventory(ItemData itemData,out InventoryItemIcon leastAmountItemIcon,out InventoryItemSlot slotWithLeastItem)
    {
        bool returnBool = false;
        int leastAmount = 0;
        leastAmountItemIcon = null;
        slotWithLeastItem = null;
        foreach (InventoryItemSlot itemSlot in itemSlots)
        {
            InventoryItemIcon itemIconWithLeastAmount = null;
            if (!itemSlot.containsItem)
                continue;
            if(itemSlot.itemData.itemID == itemData.itemID)
            {
                if(itemIconWithLeastAmount==null)
                {
                    itemIconWithLeastAmount = itemSlot.itemIcon;
                    leastAmount =itemSlot.itemIcon.amount;
                    leastAmountItemIcon = itemIconWithLeastAmount;
                    returnBool = true;
                    slotWithLeastItem = itemSlot;
                    continue;
                }

                if(itemSlot.itemIcon.amount<leastAmount)
                {
                    leastAmount = itemSlot.itemIcon.amount;
                    itemIconWithLeastAmount = itemSlot.itemIcon;
                    leastAmountItemIcon = itemIconWithLeastAmount;
                    returnBool = true;
                    slotWithLeastItem = itemSlot;
                }

            }
        }
        return returnBool;
    }

    bool IsThereASlotEmpty(out InventoryItemSlot itemSlot)
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].containsItem)
            {
                itemSlot = itemSlots[i];
                return true;
            } 
        }
        itemSlot = null;
        return false;
    }
    public bool IsSpaceAvailable(ItemData itemData)
    {
        if(itemData.itemType == ItemData.ItemType.non_consumable)
        {
            foreach (InventoryItemSlot slot in itemSlots)
            {
                if (!slot.containsItem)
                    return true;
            }
        }
        else
        {
            foreach (InventoryItemSlot slot in itemSlots)
            {
                if (!slot.containsItem)
                    return true;
            }

            foreach (InventoryItemSlot slot in itemSlots)
            {
                if (slot.itemData.itemID == itemData.itemID)
                {
                    if (slot.itemData.amount < itemData.amountLimitPerSlot)
                        return true;
                }
                    
            }
        }

        return false;
    }

    bool isItemAvailable(ItemData itemData)
    {
        foreach (InventoryItemSlot slot in itemSlots)
        {
            if (slot.containsItem)
            {
                if (slot.itemData.itemID == itemData.itemID)
                    return true;
            }
        }
        return false;
    }
}
