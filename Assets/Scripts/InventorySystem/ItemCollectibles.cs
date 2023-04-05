using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollectibles : MonoBehaviour
{
    [SerializeField]ItemData itemData;
    [SerializeField] int count = 10;
    bool canCollect = false;
    bool isSpaceAvailable = false;
    [SerializeField] GameObject collectUI;
    [SerializeField] GameObject cantCollectImage;
    // Start is called before the first frame update
    void Start()
    {
        collectUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canCollect)
        {
            isSpaceAvailable = Inventory.instance.IsSpaceAvailable(itemData);
            if(isSpaceAvailable)
            {
                cantCollectImage.SetActive(false);
                if(Input.GetKeyDown(KeyCode.F))
                {
                    Collect();
                }
            }
            else
            {
                cantCollectImage.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canCollect = true;
            collectUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect = false;
            collectUI.SetActive(false);
        }
    }

    void Collect()
    {
        Inventory.instance.AddItemToInventory(itemData, count);
        Destroy(gameObject);
    }

}
