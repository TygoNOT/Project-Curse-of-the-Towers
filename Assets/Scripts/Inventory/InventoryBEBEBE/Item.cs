using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    public string itemName;

    [SerializeField]
    public int quantity;

    [SerializeField]
    public Sprite sprite;

    [TextArea]
    [SerializeField]
    public string itemDescription;

    private InventoryManager inventoryManager;

    public ItemType itemType;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnMouseDown()
    {
            int leftOverItems=inventoryManager.AddItem(itemName, quantity, sprite, itemDescription, itemType);
        if(leftOverItems<=0)
            Destroy(gameObject);
        else 
            quantity = leftOverItems;
    }
}
