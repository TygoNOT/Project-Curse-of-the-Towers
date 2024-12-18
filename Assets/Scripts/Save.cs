using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    public void SaveInventory()
    {
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager не найден, невозможно сохранить данные.");
            return;
        }
        List<SerializedSlot> equipmentSlots = new List<SerializedSlot>();
        List<SerializedSlot> itemSlots = new List<SerializedSlot>();
        List<SerializedSlot> petSlots = new List<SerializedSlot>();
        // Сохраняем данные из всех массивов
        SaveEquipmentSlots(inventoryManager.equipmentSlot, equipmentSlots);
        SaveItemSlots(inventoryManager.itemSlot, itemSlots);
        SavePetSlots(inventoryManager.petSlot, petSlots);

        // Сериализуем данные в JSON
        string jsonEquip = JsonUtility.ToJson(new Wrapper<List<SerializedSlot>> { data = equipmentSlots });
        string jsonItem = JsonUtility.ToJson(new Wrapper<List<SerializedSlot>> { data = itemSlots });
        string jsonPet = JsonUtility.ToJson(new Wrapper<List<SerializedSlot>> { data = petSlots });
        List<SerializedEquippableSlot> equippableSlots = new List<SerializedEquippableSlot>();
        SaveEquippedSlots(inventoryManager.equippedSlot, equippableSlots);

        // Сериализация и сохранение данных
        string jsonEquippable = JsonUtility.ToJson(new Wrapper<List<SerializedEquippableSlot>> { data = equippableSlots });
        PlayerPrefs.SetString("EquippableSlotsData", jsonEquippable);
        PlayerPrefs.SetString("EquipmentSlotsData", jsonEquip);
        PlayerPrefs.SetString("ItemSlotsData", jsonItem);
        PlayerPrefs.SetString("PetSlotsData", jsonPet);
        PlayerPrefs.Save();

        Debug.Log("Данные инвентаря сохранены!");
    }
    private void SaveEquipmentSlots(EquipmentSlot[] slots, List<SerializedSlot> allSlots)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.isFull)
            {
                allSlots.Add(new SerializedSlot
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSpriteName = slot.itemSprite ? slot.itemSprite.name : null, // Имя спрайта
                    quantity = slot.quantity,
                    itemType = slot.itemType.ToString(),
                    attribute = slot.attribute.ToString(),
                    isFull = slot.isFull
                });
            }
        }
    }
    private void SaveEquippedSlots(EquippedSlot[] slots, List<SerializedEquippableSlot> allSlots)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.equippedItem != null)
            {
                allSlots.Add(new SerializedEquippableSlot
                {
                    equipmentSOName = slot.equippedItem.name, // Сохраняем имя EquipmentSO
                    itemSpriteName = slot.equippedItem.itemSprite != null ? slot.equippedItem.itemSprite.name : null,
                    isEquipped = slot.slotInUse,
                    itemName = slot.equippedItem.itemName,
                    itemDescription = slot.equippedItem.itemDescription,
                    attribute = slot.equippedItem.attribute,
                    itemType = slot.equippedItem.itemType
                });
            }
        }
    }
    private void SaveItemSlots(ItemSlot[] slots, List<SerializedSlot> allSlots)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.quantity > 0)
            {
                allSlots.Add(new SerializedSlot
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSpriteName = slot.itemSprite ? slot.itemSprite.name : null, // Имя спрайта
                    quantity = slot.quantity,
                    itemType = slot.itemType.ToString(),
                    attribute = slot.attribute.ToString(),
                    isFull = slot.isFull
                });
            }
        }
    }
    private void SavePetSlots(PetSlot[] slots, List<SerializedSlot> allSlots)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.isFull)
            {
                allSlots.Add(new SerializedSlot
                {
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    itemSpriteName = slot.itemSprite ? slot.itemSprite.name : null, // Имя спрайта
                    quantity = slot.quantity,
                    itemType = slot.itemType.ToString(),
                    attribute = slot.attribute.ToString(),
                    isFull = slot.isFull
                });
            }
        }
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T data;
    }
}
