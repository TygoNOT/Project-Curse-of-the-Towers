using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryLoader : MonoBehaviour
{
    public InventoryManager currentInventoryManager;
    public EquipmentSOLibrary equipmentSOLibrary;
    private void Start()
    {
        currentInventoryManager = null;
        equipmentSOLibrary = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSOLibrary>();
        currentInventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    private void Update()
    {
        currentInventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Присваиваем новые объекты
        equipmentSOLibrary = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSOLibrary>();
        currentInventoryManager = GameObject.Find("InventoryCanvas")?.GetComponent<InventoryManager>();
        
        

        
        // Загружаем данные из PlayerPrefs
        string jsonEp = PlayerPrefs.GetString("EquippableSlotsData");
        string jsonEq = PlayerPrefs.GetString("EquipmentSlotsData");
        string jsonIt = PlayerPrefs.GetString("ItemSlotsData");
        string jsonPe = PlayerPrefs.GetString("PetSlotsData");
        Debug.Log(jsonEp);
        List<SerializedEquippableSlot> savedEquippableSlots = JsonUtility.FromJson<Wrapper<List<SerializedEquippableSlot>>>(jsonEp).data;
        List<SerializedSlot> savedEquipSlots = JsonUtility.FromJson<Wrapper<List<SerializedSlot>>>(jsonEq).data;
        List<SerializedSlot> savedItemSlots = JsonUtility.FromJson<Wrapper<List<SerializedSlot>>>(jsonIt).data;
        List<SerializedSlot> savedPetSlots = JsonUtility.FromJson<Wrapper<List<SerializedSlot>>>(jsonPe).data;
        Debug.Log(savedEquippableSlots);
        Debug.Log("Before Load");
        ApplyEquippedData(savedEquippableSlots, currentInventoryManager.equippedSlot, equipmentSOLibrary.equipmentSO);
        ApplyEquipData(savedEquipSlots, currentInventoryManager?.equipmentSlot, equipmentSOLibrary.equipmentSO);
        ApplyItemData(savedItemSlots, currentInventoryManager?.itemSlot, currentInventoryManager.itemSOs);
        ApplyPetData(savedPetSlots, currentInventoryManager?.petSlot);
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

            // Логирование для отладки
            Debug.Log($"Применение данных к слоту {i}: itemName = {savedSlot.itemName}, itemSpriteName = {savedSlot.itemSpriteName}");

            // Применение данных к слоту
            slots[i].itemName = savedSlot.itemName;
            slots[i].itemDescription = savedSlot.itemDescription;
            for (int j=0; j<equipmentSO.Length; j++)
            {
                if (equipmentSO[j].itemName == savedSlot.itemName)
                {
                    Debug.Log("Item Name Correct!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
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
                Debug.LogWarning($"Не удалось преобразовать itemType: {savedSlot.itemType}");
            }

            if (Enum.TryParse(savedSlot.attribute, out Attribute attribute))
            {
                slots[i].attribute = attribute;
            }
            else
            {
                Debug.LogWarning($"Не удалось преобразовать attribute: {savedSlot.attribute}");
            }

            slots[i].isFull = savedSlot.isFull;
        }
    }
    private void ApplyItemData(List<SerializedSlot> savedSlots, ItemSlot[] slots, ItemSo[] itemSo)
    {
        for (int i = 0; i < Mathf.Min(slots.Length, savedSlots.Count); i++)
        {
            var savedSlot = savedSlots[i];

            // Логирование для отладки
            Debug.Log($"Применение данных к слоту {i}: itemName = {savedSlot.itemName}, itemSpriteName = {savedSlot.itemSpriteName}");

            // Применение данных к слоту
            slots[i].itemName = savedSlot.itemName;
            slots[i].itemDescription = savedSlot.itemDescription;

            // Загружаем спрайт из ресурсов и добавляем проверку на его наличие
            for (int j = 0; j < itemSo.Length; j++)
            {
                if (itemSo[j].itemName == savedSlot.itemName)
                {
                    Debug.Log("Item Name Correct!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    slots[i].itemSprite = itemSo[j].itemSprite;
                    slots[i].AddImage(itemSo[j].itemSprite);
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
                Debug.LogWarning($"Не удалось преобразовать itemType: {savedSlot.itemType}");
            }

            if (Enum.TryParse(savedSlot.attribute, out Attribute attribute))
            {
                slots[i].attribute = attribute;
            }
            else
            {
                Debug.LogWarning($"Не удалось преобразовать attribute: {savedSlot.attribute}");
            }

            slots[i].isFull = savedSlot.isFull;
        }
    }
    private EquipmentSO FindEquipmentSOByName(string equipmentName, EquipmentSO[] equipmentArray)
    {
        foreach (var equipment in equipmentArray)
        {
            if (equipment.itemName == equipmentName)
            {
                Debug.Log("Item Name Correct!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                return equipment;
            }
        }
        return null;
    }
    private void LoadSlotData(EquippedSlot slot, EquipmentSO equipment, SerializedEquippableSlot savedSlot)
    {
        if (slot == null || equipment == null || savedSlot == null)
        {
            Debug.LogWarning("Invalid input to LoadSlotData.");
            return;
        }

        // Установка основных данных слота
        slot.equippedItem = equipment;
        slot.slotInUse = true;
        slot.AddImage(equipment.itemSprite);

        if (slot.sloTName != null)
        {
            slot.sloTName.enabled = false;
        }

        // Использование данных из SerializedEquippableSlot
        slot.itemName = savedSlot.itemName;
        slot.itemDescription = savedSlot.itemDescription;
        slot.attribute = savedSlot.attribute;
        slot.itemType = savedSlot.itemType;

        Debug.Log($"Loaded slot data for: {equipment.itemName}, " +
                  $"Name: {savedSlot.itemName}, " +
                  $"Description: {savedSlot.itemDescription}, " +
                  $"Quantity: {savedSlot.quantity}, " +
                  $"Attribute: {savedSlot.attribute}, " +
                  $"Type: {savedSlot.itemType}");
    }
    private void ApplyEquippedData(List<SerializedEquippableSlot> savedSlots, EquippedSlot[] slots, EquipmentSO[] equipmentSOArray)
    {
        if (savedSlots == null || slots == null || equipmentSOArray == null)
        {
            Debug.LogError("Invalid input to ApplyEquippedData.");
            return;
        }

        foreach (var savedSlot in savedSlots)
        {
            var loadedEquipment = FindEquipmentSOByName(savedSlot.equipmentSOName, equipmentSOArray);
            if (loadedEquipment == null)
            {
                Debug.LogWarning($"EquipmentSO not found for: {savedSlot.equipmentSOName}");
                continue;
            }

            EquippedSlot slotToLoad;
            switch (loadedEquipment.itemType)
            {
                case ItemType.weapon:
                    slotToLoad = slots[4];
                    break;
                case ItemType.headArmor:
                    slotToLoad = slots[0];
                    break;
                case ItemType.pet:
                    slotToLoad = slots[5];
                    break;
                case ItemType.legsArmor:
                    slotToLoad = slots[2];
                    break;
                case ItemType.chestArmor:
                    slotToLoad = slots[1];
                    break;
                case ItemType.footArmor:
                    slotToLoad = slots[3];
                    break;
                default:
                    Debug.LogWarning($"Unknown item type: {loadedEquipment.itemType}");
                    continue;
            }

            // Передаём savedSlot в LoadSlotData
            LoadSlotData(slotToLoad, loadedEquipment, savedSlot);
        }
    }
    private void ApplyPetData(List<SerializedSlot> savedSlots, PetSlot[] slots)
    {
        for (int i = 0; i < Mathf.Min(slots.Length, savedSlots.Count); i++)
        {
            var savedSlot = savedSlots[i];

            // Логирование для отладки
            Debug.Log($"Применение данных к слоту {i}: itemName = {savedSlot.itemName}, itemSpriteName = {savedSlot.itemSpriteName}");

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
                Debug.LogWarning($"Не удалось загрузить спрайт: {savedSlot.itemSpriteName}");
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
                Debug.LogWarning($"Не удалось преобразовать itemType: {savedSlot.itemType}");
            }

            if (Enum.TryParse(savedSlot.attribute, out Attribute attribute))
            {
                slots[i].attribute = attribute;
            }
            else
            {
                Debug.LogWarning($"Не удалось преобразовать attribute: {savedSlot.attribute}");
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