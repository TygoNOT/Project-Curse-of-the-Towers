using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAcceptButton : MonoBehaviour
{
    public string slotName;

    public EquipmentSlot equipmentSlot;

    public PetSlot petSlot;

    public ItemSlot itemSlot;

    [SerializeField]
    public GameObject equipmentPanel;

    [SerializeField]
    public GameObject petMenu;

    [SerializeField]
    public GameObject inventoryMenu;

    public InventoryManager inventoryManager;

    public void OnMouseDown()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        if (equipmentPanel.activeSelf)
        {
            Debug.Log("eqip inventory is opened");
            for (int i=0; inventoryManager.equipmentSlot.Length>i; i++)
            {
                if(inventoryManager.equipmentSlot[i].slotName == slotName)
                    equipmentSlot = inventoryManager.equipmentSlot[i];
            }
            //equipmentSlot = GameObject.Find(slotName).GetComponent<EquipmentSlot>();
            equipmentSlot.dropItem();
        }
        else if (petMenu.activeSelf)
        {
            Debug.Log("eqip inventory is opened");
            foreach (PetSlot petSl in inventoryManager.petSlot)
            {
                if (petSl.name == slotName)
                {
                    petSlot = GameObject.Find(slotName).GetComponent<PetSlot>();
                    petSlot.dropItem();
                }
            }
        }
        else if (inventoryMenu.activeSelf)
        {
            Debug.Log("eqip inventory is opened");
            foreach (ItemSlot itemSl in inventoryManager.itemSlot)
            {
                if (itemSl.name == slotName)
                {
                    itemSlot = GameObject.Find(slotName).GetComponent<ItemSlot>();
                    itemSlot.dropItem();
                }
            }
        }


    }
}
