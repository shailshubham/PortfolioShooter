using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectibles : MonoBehaviour
{
    [SerializeField]ItemData itemData;
    bool checkEmptySlots = false;
    bool isSlotEmpty = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(checkEmptySlots)
        {
            isSlotEmpty = Inventory.instance.IsEmptySlotAvailable();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            checkEmptySlots = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkEmptySlots = false;
        }
    }
}
