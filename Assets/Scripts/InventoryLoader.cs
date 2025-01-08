using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryLoader : MonoBehaviour
{
    public PlayerStats stats;
    public InventoryManager currentInventoryManager;
    public EquipmentSOLibrary equipmentSOLibrary;
    private void Start()
    {
        stats = null;
        stats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
        currentInventoryManager = null;
        equipmentSOLibrary = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSOLibrary>();
        currentInventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    private void Update()
    {
        currentInventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        stats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Присваиваем новые объекты
        equipmentSOLibrary = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSOLibrary>();
        currentInventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();
        stats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();



        // Загружаем данные из PlayerPrefs
        string jsonStat = PlayerPrefs.GetString("PlayerStatsSaves");
        string jsonEquipped = PlayerPrefs.GetString("EquippableSlotsData");
        string jsonEquip = PlayerPrefs.GetString("EquipmentSlotsData");
        string jsonItem = PlayerPrefs.GetString("ItemSlotsData");
        string jsonPet = PlayerPrefs.GetString("PetSlotsData");
        List<SerializedEquippableSlot> savedEquippableSlots = JsonUtility.FromJson<Wrapper<List<SerializedEquippableSlot>>>(jsonEquipped).data;
        List<SerializedSlot> savedEquipSlots = JsonUtility.FromJson<Wrapper<List<SerializedSlot>>>(jsonEquip).data;
        List<SerializedSlot> savedItemSlots = JsonUtility.FromJson<Wrapper<List<SerializedSlot>>>(jsonItem).data;
        List<SerializedSlot> savedPetSlots = JsonUtility.FromJson<Wrapper<List<SerializedSlot>>>(jsonPet).data;
        List<PlayerStatsSave> savedStats = JsonUtility.FromJson<Wrapper<List<PlayerStatsSave>>>(jsonStat).data;
        Debug.Log("Before Load");
        ApplyEquippedData(savedEquippableSlots, currentInventoryManager.equippedSlot, equipmentSOLibrary.equipmentSO);
        ApplyEquipData(savedEquipSlots, currentInventoryManager?.equipmentSlot, equipmentSOLibrary.equipmentSO);
        ApplyItemData(savedItemSlots, currentInventoryManager?.itemSlot, currentInventoryManager.itemSOs);
        ApplyPetData(savedPetSlots, currentInventoryManager?.petSlot);
        ApplyStatsData(savedStats, stats);
        Debug.Log("Данные инвентаря загружены!");
    }
    public void LoadInventory()
    {
        
    }

    private void ApplyEquipData(List<SerializedSlot> savedSlots, EquipmentSlot[] slots, EquipmentSO[] equipmentSO)
    {
        // Используем безопасную проверку, чтобы избежать выхода за пределы массива
        for (int i = 0; i < Mathf.Min(slots.Length, savedSlots.Count); i++)
        {
            var savedSlot = savedSlots[i];
            // Применение данных к слоту
            slots[i].itemName = savedSlot.itemName;
            slots[i].itemDescription = savedSlot.itemDescription;
            for (int j=0; j<equipmentSO.Length; j++)
            {
                if (equipmentSO[j].itemName == savedSlot.itemName)
                {
                    slots[i].itemSprite = equipmentSO[j].itemSprite;
                    slots[i].AddImage(equipmentSO[j].itemSprite);
                }
            }

            slots[i].quantity = savedSlot.quantity;

            // Преобразуем строковые значения в соответствующие типы
            if (Enum.TryParse(savedSlot.itemType, out ItemType itemType))
            {
                slots[i].itemType = itemType;
            }
            else
            {
            }

            if (Enum.TryParse(savedSlot.attribute, out Attribute attribute))
            {
                slots[i].attribute = attribute;
            }
            else
            {
            }

            slots[i].isFull = savedSlot.isFull;
        }
    }

    private void ApplyStatsData(List<PlayerStatsSave> savedStats, PlayerStats playerStats)
    {
        for (int i = 0; i < savedStats.Count; i++)
        {
            playerStats.attack = savedStats[i].attack;
            playerStats.hp = savedStats[i].hp;
            playerStats.speed = savedStats[i].speed;
            playerStats.critChance = savedStats[i].critChance;
            playerStats.critDmg = savedStats[i].critDmg;
            playerStats.UpdateEquipmentStats();
        }
    }
    private void ApplyItemData(List<SerializedSlot> savedSlots, ItemSlot[] slots, ItemSo[] itemSo)
    {
        for (int i = 0; i < Mathf.Min(slots.Length, savedSlots.Count); i++)
        {
            var savedSlot = savedSlots[i];

            // Применение данных к слоту
            slots[i].itemName = savedSlot.itemName;
            slots[i].itemDescription = savedSlot.itemDescription;

            // Загружаем спрайт из ресурсов и добавляем проверку на его наличие
            for (int j = 0; j < itemSo.Length; j++)
            {
                if (itemSo[j].itemName == savedSlot.itemName)
                {
                    slots[i].itemSprite = itemSo[j].itemSprite;
                    slots[i].AddImage(itemSo[j].itemSprite);
                }
            }
            slots[i].quantity = savedSlot.quantity;
            slots[i].quantityText.text = savedSlot.quantity.ToString();
            slots[i].quantityText.enabled = true;
            // Преобразуем строковые значения в соответствующие типы
            if (Enum.TryParse(savedSlot.itemType, out ItemType itemType))
            {
                slots[i].itemType = itemType;
            }
            else
            {
            }

            if (Enum.TryParse(savedSlot.attribute, out Attribute attribute))
            {
                slots[i].attribute = attribute;
            }
            else
            {
            }

            slots[i].isFull = savedSlot.isFull;
        }
    }
    private void LoadSlotData(EquippedSlot slot, EquipmentSO equipment, SerializedEquippableSlot savedSlot)
    {
        if (slot == null || equipment == null || savedSlot == null)
        {
            return;
        }

        // Установка основных данных слота
        slot.equippedItem = equipment;
        slot.slotInUse = true;
        slot.AddImage(equipment.itemSprite);

        //if (slot.sloTName != null)
        //{
        //    slot.sloTName.enabled = false;
        //}

        // Использование данных из SerializedEquippableSlot
        slot.sloTName.enabled = false;
        slot.itemName = savedSlot.itemName;
        slot.itemDescription = savedSlot.itemDescription;
        string savedSlotAttribute = savedSlot.attribute;
        slot.attribute = (Attribute)Enum.Parse(typeof(Attribute), savedSlotAttribute);
        string savedItemType = savedSlot.itemType;
        slot.itemType = (ItemType)Enum.Parse(typeof(ItemType), savedItemType);

    }
    private void ApplyEquippedData(List<SerializedEquippableSlot> savedSlots, EquippedSlot[] slots, EquipmentSO[] equipmentSOArray)
    {
        if (savedSlots == null || slots == null || equipmentSOArray == null)
        {
            return;
        }
        EquipmentSO loadedEquipment=null;
        foreach (var savedSlot in savedSlots)
        {
            for (int i = 0; i < equipmentSOArray.Length; i++)
            {
                if (savedSlot.itemName == equipmentSOArray[i].itemName)
                {
                    loadedEquipment = equipmentSOArray[i];
                }
            }

            switch (loadedEquipment.itemType)
            {
                case ItemType.weapon:
                    LoadSlotData(slots[4], loadedEquipment, savedSlot);
                    break;
                case ItemType.headArmor:
                    LoadSlotData(slots[0], loadedEquipment, savedSlot);
                    break;
                case ItemType.pet:
                    LoadSlotData(slots[5], loadedEquipment, savedSlot);
                    break;
                case ItemType.legsArmor:
                    LoadSlotData(slots[2], loadedEquipment, savedSlot);
                    break;
                case ItemType.chestArmor:
                    LoadSlotData(slots[1], loadedEquipment, savedSlot);
                    break;
                case ItemType.footArmor:
                    LoadSlotData(slots[3], loadedEquipment, savedSlot);
                    break;
                default:
                    continue;
            }

            // Передаём savedSlot в LoadSlotData
            
        }
    }
    private void ApplyPetData(List<SerializedSlot> savedSlots, PetSlot[] slots)
    {
        for (int i = 0; i < Mathf.Min(slots.Length, savedSlots.Count); i++)
        {
            var savedSlot = savedSlots[i];

            // Логирование для отладки

            // Применение данных к слоту
            slots[i].itemName = savedSlot.itemName;
            slots[i].itemDescription = savedSlot.itemDescription;

            // Загружаем спрайт из ресурсов и добавляем проверку на его наличие
            Sprite loadedSprite = string.IsNullOrEmpty(savedSlot.itemSpriteName) ? null : Resources.Load<Sprite>(savedSlot.itemSpriteName);
            if (loadedSprite != null)
            {
                slots[i].itemSprite = loadedSprite;
            }
            else
            {
                slots[i].itemSprite = null; // Присваиваем null, если спрайт не найден
            }

            slots[i].quantity = savedSlot.quantity;

            // Преобразуем строковые значения в соответствующие типы
            if (Enum.TryParse(savedSlot.itemType, out ItemType itemType))
            {
                slots[i].itemType = itemType;
            }
            else
            {
            }

            if (Enum.TryParse(savedSlot.attribute, out Attribute attribute))
            {
                slots[i].attribute = attribute;
            }
            else
            {
            }

            slots[i].isFull = savedSlot.isFull;
        }
    }
    private void OnEnable()
    {
        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Отписываемся от события загрузки сцены
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    [System.Serializable]
    private class Wrapper<T>
    {
        public T data;
    }
    
}