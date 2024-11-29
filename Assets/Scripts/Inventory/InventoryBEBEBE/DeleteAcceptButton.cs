using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAcceptButton : MonoBehaviour
{
    public string slotName;


    public EquipmentSlot equipmentSlot;


    public PetSlot petSlot;

    [SerializeField]
    public GameObject equipmentPanel;

    [SerializeField]
    public GameObject petMenu;

    public InventoryManager inventoryManager;

    public void OnMouseDown()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        if (equipmentPanel.activeSelf)
        {
            equipmentSlot = GameObject.Find(slotName).GetComponent<EquipmentSlot>();
            equipmentSlot.dropItem();
        }
        else if (petMenu.activeSelf)
        {
            foreach (PetSlot petSl in inventoryManager.petSlot)
            {
                if (petSl.name == slotName)
                {
                    petSlot = GameObject.Find(slotName).GetComponent<PetSlot>();
                    petSlot.dropItem();
                }
            }
        }
        
    }
}
