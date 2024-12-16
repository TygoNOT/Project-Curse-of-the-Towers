using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[System.Serializable]
public class ShopProgressionLevel
{
    public int level; 
    public int[] rarityChances; 
    public List<string> allowedSets;
}

public class Shopcode : MonoBehaviour
{
    public Money money;
    public InventoryManager inventoryManager;
    public EquipmentSOLibrary itemDatabase;
    public EquipmentSO[] ShopItem;
    public ItemSo[] ConsShopItem;
    public EquipmentSO SelectedItem;
    public GameObject[] slots;
    public GameObject[] slotsConsumables;
    //public Text[] itemNames;
    public Text[] itemPrices;
    public Image[] itemImages;
    public Button buyButton;
    public Button consbuyButton;
    public Text RestockPrice;
    public int SelectedItemNumber;
    public int RestockPriceValue;
    public int currentProgressionLevel = 1;
    public List<ShopProgressionLevel> shopProgressionLevels;

    public Image DescriptionImage;
    public Text DescriptionName;
    public Text DescriptionType;
    public Text DescriptionText;

    public void Start()
    {
        if (money == null)
        {
            money = FindObjectOfType<Money>();
        }

        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }

        if (itemDatabase == null)
        {
            itemDatabase = FindObjectOfType<EquipmentSOLibrary>();
            if (itemDatabase == null)
            {
                Debug.LogError("EquipmentSOLibrary could not be found in the scene.");
                return;
            }
        }

        RestockShop();
        ResetRestockPrice();
    }

    public void ResetRestockPrice()
    {
        RestockPriceValue = 25;
        RestockPrice.text = "Price: " + RestockPriceValue.ToString();
    }

    public void IncreaseRestockPrice()
    {
        RestockPriceValue += 25;
        RestockPrice.text = "Price: " + RestockPriceValue.ToString();
    }

    public void SetShopProgression(int amount)
    {
        currentProgressionLevel = amount;
    }

    public void RestockShop()
    {
        if (money.PayGold(RestockPriceValue))
        {
            DeselectItem();
            IncreaseRestockPrice();
            for (int i = 0; i < ShopItem.Length; i++)
            {
                itemImages[i].gameObject.SetActive(true);
                ShopItem[i] = RandomizeItem();
                Item itemComponent = slots[i].GetComponent<Item>();
                if (itemComponent != null)
                {
                    itemComponent.itemName = ShopItem[i].itemName;
                    itemComponent.quantity = 1;
                    itemComponent.sprite = ShopItem[i].itemSprite;
                    itemComponent.itemDescription = ShopItem[i].itemDescription;
                    itemComponent.itemType = ShopItem[i].itemType;
                }
                if (ShopItem[i] != null)
                {
                    //itemNames[i].text = ShopItem[i].itemName;
                    itemPrices[i].text = ShopItem[i].price.ToString() + " Gold";
                    itemImages[i].sprite = ShopItem[i].itemSprite;
                    itemImages[i].enabled = true;
                }
                else
                {
                    //itemNames[i].text = "Empty";
                    itemPrices[i].text = "";
                    Debug.LogWarning("No item was generated to display in the shop.");
                }
            }
        }
    }

    public Rarity RandomizeRarity()
    {
        ShopProgressionLevel currentLevel = shopProgressionLevels.FirstOrDefault(p => p.level == currentProgressionLevel);
        if (currentLevel == null)
        {
            Debug.LogError("No progression level data found for level " + currentProgressionLevel);
            return Rarity.common;
        }

        int randomNumber = Random.Range(1, 101);
        int cumulativeChance = 0;

        for (int i = 0; i < currentLevel.rarityChances.Length; i++)
        {
            cumulativeChance += currentLevel.rarityChances[i];
            if (randomNumber <= cumulativeChance)
                return (Rarity)i;
        }

        return Rarity.common;
    }

    public EquipmentSO RandomizeItem()
    {
        if (itemDatabase == null || itemDatabase.equipmentSO == null || itemDatabase.equipmentSO.Length == 0)
        {
            Debug.LogError("EquipmentSOLibrary or its items are null or empty.");
            return null;
        }

        List<EquipmentSO> filteredItems;
        int attempts = 5;

        do
        {
            Rarity rarity = RandomizeRarity();
            ShopProgressionLevel currentLevel = shopProgressionLevels.FirstOrDefault(p => p.level == currentProgressionLevel);
            if (currentLevel == null) break;

            filteredItems = itemDatabase.equipmentSO
                .Where(item => item.rarity == rarity && currentLevel.allowedSets.Contains(item.setName))
                .ToList();

            if (filteredItems.Count > 0)
            {
                int randomIndex = Random.Range(0, filteredItems.Count);
                return filteredItems[randomIndex];
            }

            attempts--;

        } while (filteredItems.Count == 0 && attempts > 0);

        Debug.LogWarning("No items with the specified quality and set were found.");
        return null;
    }

    public void SelectItem(int n)
    {
        //ShopItem[n].PreviewEquipment();
        Item itemComponent = slots[n].GetComponent<Item>();
        DescriptionImage.sprite = itemComponent.sprite;
        DescriptionName.text = itemComponent.itemName;
        DescriptionType.text = itemComponent.itemType.ToString();
        DescriptionText.text = itemComponent.itemDescription;
        SelectedItem = ShopItem[n];
        SelectedItemNumber = n;
        buyButton.gameObject.SetActive(true);
        consbuyButton.gameObject.SetActive(false);
        DescriptionImage.gameObject.SetActive(true);
        DescriptionName.gameObject.SetActive(true);
        DescriptionType.gameObject.SetActive(true);
        DescriptionText.gameObject.SetActive(true);
    }

    public void SelectConsumableItem(int n)
    {
        ItemSo itemComponent = slotsConsumables[n].GetComponent<ItemSo>();
        DescriptionImage.sprite = itemComponent.itemSprite;
        DescriptionName.text = itemComponent.itemName;
        DescriptionType.text = itemComponent.itemType.ToString();
        DescriptionText.text = itemComponent.itemDescription;
        SelectedItemNumber = n;
        buyButton.gameObject.SetActive(false);
        consbuyButton.gameObject.SetActive(true);
        DescriptionImage.gameObject.SetActive(true);
        DescriptionName.gameObject.SetActive(true);
        DescriptionType.gameObject.SetActive(true);
        DescriptionText.gameObject.SetActive(true);
    }

    public void DeselectItem()
    {
        GameObject.Find("StatsManager").GetComponent<PlayerStats>().TurnOffPreviewStats();
        SelectedItem = null;
        buyButton.gameObject.SetActive(false);
        consbuyButton.gameObject.SetActive(false);
        DescriptionName.gameObject.SetActive(false);
        DescriptionImage.gameObject.SetActive(false);
        DescriptionType.gameObject.SetActive(false);
        DescriptionText.gameObject.SetActive(false);
    }

    public void BuyItem()
    {
        int ItemPriceValue = ShopItem[SelectedItemNumber].price;
        if (ShopItem[SelectedItemNumber] != null)
        {
            Item itemComponent = slots[SelectedItemNumber].GetComponent<Item>();
            if (itemComponent.itemType == ItemType.weapon || itemComponent.itemType == ItemType.headArmor || itemComponent.itemType == ItemType.chestArmor || itemComponent.itemType == ItemType.legsArmor || itemComponent.itemType == ItemType.footArmor)
            {
                for (int i = 0; i < inventoryManager.equipmentSlot.Length; i++)
                {
                    if (inventoryManager.equipmentSlot[i].isFull == false && inventoryManager.equipmentSlot[i].itemName == itemComponent.itemName || inventoryManager.equipmentSlot[i].quantity == 0)
                    {
                        if (money.PayGold(ItemPriceValue))
                        {
                            inventoryManager.AddItem(itemComponent.itemName, itemComponent.quantity, itemComponent.sprite, itemComponent.itemDescription, itemComponent.itemType);
                            //itemNames[SelectedItemNumber].text = "Sold!";
                            itemPrices[SelectedItemNumber].text = "Sold!";
                            ShopItem[SelectedItemNumber] = null;
                            DeselectItem();
                            itemImages[SelectedItemNumber].gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }
            if (itemComponent.itemType == ItemType.pet)
            {
                for (int i = 0; i < inventoryManager.petSlot.Length; i++)
                {
                    if (inventoryManager.petSlot[i].isFull == false && inventoryManager.petSlot[i].itemName == itemComponent.itemName || inventoryManager.petSlot[i].quantity == 0)
                    {
                        if (money.PayGold(ItemPriceValue))
                        {
                            inventoryManager.AddItem(itemComponent.itemName, itemComponent.quantity, itemComponent.sprite, itemComponent.itemDescription, itemComponent.itemType);
                            //itemNames[SelectedItemNumber].text = "Sold!";
                            itemPrices[SelectedItemNumber].text = "Sold!";
                            ShopItem[SelectedItemNumber] = null;
                            DeselectItem();
                            itemImages[SelectedItemNumber].gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }
        }
    }
    public void BuyConsumableItem()
    {
        int ItemPriceValue = ConsShopItem[SelectedItemNumber].price;
        ItemSo itemComponent = slotsConsumables[SelectedItemNumber].GetComponent<ItemSo>();
        if (itemComponent.itemType == ItemType.consumable)
        {
            for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
            {
                if (inventoryManager.itemSlot[i].isFull == false && inventoryManager.itemSlot[i].itemName == itemComponent.itemName || inventoryManager.itemSlot[i].quantity == 0)
                {
                    if (money.PayGold(ItemPriceValue))
                    {
                        inventoryManager.AddItem(itemComponent.itemName, itemComponent.quantity, itemComponent.itemSprite, itemComponent.itemDescription, itemComponent.itemType);
                        DeselectItem();
                        break;
                    }
                }
            }
        }
    }
}
