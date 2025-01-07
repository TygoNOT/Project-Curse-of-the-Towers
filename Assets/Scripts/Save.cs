using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public PlayerStats stats;
    private void Start()
    {
        stats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    public void SaveInventory()
    {
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager не найден, невозможно сохранить данные.");
            return;
        }
        if (stats == null)
        {
            Debug.LogError("StatsManager не найден, невозможно сохранить данные.");
            return;
        }
        List<SerializedSlot> equipmentSlots = new();
        List<SerializedSlot> itemSlots = new();
        List<SerializedSlot> petSlots = new();
        List<SerializedEquippableSlot> equippableSlots = new List<SerializedEquippableSlot>();
        List<PlayerStatsSave> playerStats = new();
        // Сохраняем данные из всех массивов
        SaveEquipmentSlots(inventoryManager.equipmentSlot, equipmentSlots);
        SaveItemSlots(inventoryManager.itemSlot, itemSlots);
        SavePetSlots(inventoryManager.petSlot, petSlots);
        SaveEquippedSlots(inventoryManager.equippedSlot, equippableSlots);
        SavePlayerStat(stats, playerStats);
        // Сериализуем данные в JSON
        string jsonEquip = JsonUtility.ToJson(new Wrapper<List<SerializedSlot>> { data = equipmentSlots });
        string jsonItem = JsonUtility.ToJson(new Wrapper<List<SerializedSlot>> { data = itemSlots });
        string jsonPet = JsonUtility.ToJson(new Wrapper<List<SerializedSlot>> { data = petSlots });
        string jsonEquippable = JsonUtility.ToJson(new Wrapper<List<SerializedEquippableSlot>> { data = equippableSlots });
        string jsonStats = JsonUtility.ToJson(new Wrapper<List<PlayerStatsSave>> { data = playerStats });


        // Сериализация и сохранение данных
        PlayerPrefs.SetString("PlayerStatsSaves", jsonStats);
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
    private void SavePlayerStat(PlayerStats playerStats, List<PlayerStatsSave> allSlots)
    {

        allSlots.Add(new PlayerStatsSave
        {
            hp = playerStats.hp,
            attack = playerStats.attack,
            speed = playerStats.speed,
            critChance = playerStats.critChance,
            critDmg = playerStats.critDmg,
            attribute = playerStats.attribute.ToString()
        });
    }
    private void SaveEquippedSlots(EquippedSlot[] slots, List<SerializedEquippableSlot> allSlots)
    {
        foreach (var slot in slots)
        {
            if (slot.slotInUse)
            {
                allSlots.Add(new SerializedEquippableSlot
                {
                    isEquipped = slot.slotInUse,
                    itemName = slot.itemName,
                    itemDescription = slot.itemDescription,
                    attribute = slot.attribute.ToString(),
                    itemType = slot.itemType.ToString()
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
                    isFull = slot.isFull,
                    isHealthPotion=slot.isHealthPotion,
                    isTeleportationScroll = slot.isTeleportationScroll,
                    isBandage = slot.isBandage
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
