using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemIcon : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    Image itemIcon;
    public ItemData itemData;
    [SerializeField] InputData input;
    Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [SerializeField] Text itemCountText;
    public int amount = 0;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = itemData.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        itemCountText.text = amount.ToString();
        if(itemData.itemType == ItemData.ItemType.non_consumable)
        {
            itemCountText.text = "";
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = input.pointerInputPosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
