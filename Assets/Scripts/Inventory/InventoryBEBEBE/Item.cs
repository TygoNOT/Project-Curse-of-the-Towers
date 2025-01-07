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

    [SerializeField] public EffectType effectToRemove = EffectType.None;

    private InventoryManager inventoryManager;

    public ItemType itemType;
    public Attribute weaponAttribute;
    public bool isTeleportationScroll = false;
    public bool isHealthPotion = false;
    public bool isBandage = false;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnMouseDown()
    {
            int leftOverItems=inventoryManager.AddItem(itemName, quantity, sprite, itemDescription, itemType, weaponAttribute, isBandage, isHealthPotion, isTeleportationScroll);
        if(leftOverItems<=0)
            Destroy(gameObject);
        else 
            quantity = leftOverItems;
    }

}
