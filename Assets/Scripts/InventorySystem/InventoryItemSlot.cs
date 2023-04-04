using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemSlot : MonoBehaviour,IDropHandler
{
    [HideInInspector] public bool containsItem = false;
    public ItemData itemData;
    public InventoryItemIcon itemIcon;
    InventoryItemIcon droppedObjectIconTemp;
    GameObject dropObject;
    
    public void OnDrop(PointerEventData eventData)
    {
        dropObject = eventData.pointerDrag;
        droppedObjectIconTemp = dropObject.GetComponent<InventoryItemIcon>();
        if (!containsItem)
        {
            InventoryItemIcon droppedItemIcon = dropObject.GetComponent<InventoryItemIcon>();
            InventoryItemSlot droppedItemPreviousSlot = droppedItemIcon.parentAfterDrag.GetComponent<InventoryItemSlot>();

            droppedItemIcon.parentAfterDrag = transform;
            itemData = droppedItemIcon.itemData;
            itemIcon = droppedItemIcon;
            containsItem = true;

            droppedItemPreviousSlot.containsItem = false;
            droppedItemPreviousSlot.itemData = null;
            droppedItemPreviousSlot.itemIcon = null;
        }
        else if(itemData.itemID == droppedObjectIconTemp.itemData.itemID)
        {
           if(itemIcon.amount > droppedObjectIconTemp.amount)
           {
                InventoryItemSlot droppedItemPreviousSlot = droppedObjectIconTemp.parentAfterDrag.GetComponent<InventoryItemSlot>();
                int space = itemData.amountLimitPerSlot - itemIcon.amount;
                if(droppedObjectIconTemp.amount < space)
                {
                    itemIcon.amount += droppedObjectIconTemp.amount;
                    Destroy(droppedObjectIconTemp.gameObject);
                    droppedItemPreviousSlot.containsItem = false;
                    droppedItemPreviousSlot.itemData = null;
                    droppedItemPreviousSlot.itemIcon = null;
                }
                else
                {
                    itemIcon.amount += space;
                    droppedObjectIconTemp.amount -= space;
                    if(droppedObjectIconTemp.amount == 0)
                    {
                        Destroy(droppedObjectIconTemp.gameObject);
                        droppedItemPreviousSlot.containsItem = false;
                        droppedItemPreviousSlot.itemData = null;
                        droppedItemPreviousSlot.itemIcon = null;
                    }
                }
           }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
