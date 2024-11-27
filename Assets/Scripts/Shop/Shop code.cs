using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shopcode : MonoBehaviour
{
    public Money money;
    public InventoryManager inventoryManager;
    public ItemDatabase itemDatabase;
    public Item[] ShopItem;
    public Text[] itemNames;
    public Text[] itemPrices;
    public Button[] buyButtons;
    public Text RestockPrice;
    public int RestockPriceValue;

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
            itemDatabase = FindObjectOfType<ItemDatabase>();
            if (itemDatabase == null)
            {
                Debug.LogError("ItemDatabase could not be found in the scene.");
                return;
            }
        }

        for (int i = 0; i < buyButtons.Length; i++)
        {
            int index = i;
            buyButtons[i].onClick.AddListener(() => BuyItem(index));
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
        RestockPriceValue = RestockPriceValue + 25;
        RestockPrice.text = "Price: " + RestockPriceValue.ToString();
    }

    public void RestockShop()
    {
        if (money.PayGold(RestockPriceValue) == true)
        {
            IncreaseRestockPrice();
            for (int i = 0; i < 5; i++)
            {
                ShopItem[i] = RandomizeItem();
                if (ShopItem[i] != null)
                {
                    itemNames[i].text = ShopItem[i].itemName;
                    itemPrices[i].text = ShopItem[i].price.ToString() + " Gold";
                }
                else
                {
                    Debug.LogWarning("No item was generated to display in the shop.");
                }
            }
        }
    }

    public string RandomizeRarity()
    {
        int randomNumber = Random.Range(1, 101);
        string rarity;

        if (randomNumber <= 50)
        {
            rarity = "Common";
        }
        else if (randomNumber <= 80)
        {
            rarity = "Rare";
        }
        else if (randomNumber <= 95)
        {
            rarity = "Epic";
        }
        else
        {
            rarity = "Legendary";
        }
        return rarity;
    }

    public Item RandomizeItem()
    {
        if (itemDatabase == null || itemDatabase.items == null || itemDatabase.items.Count == 0)
        {
            Debug.LogError("ItemDatabase or its items list is null or empty.");
            return null;
        }

        List<Item> filteredItems;
        int attempts = 5;

        do
        {
            string rarity = RandomizeRarity();
            filteredItems = itemDatabase.items.Where(item => item.quality == rarity).ToList();

            if (filteredItems.Count > 0)
            {
                int randomIndex = Random.Range(0, filteredItems.Count);
                return filteredItems[randomIndex];
            }

            attempts--;

        } while (filteredItems.Count == 0 && attempts > 0);

        Debug.LogWarning("No items with the specified quality were found after multiple attempts.");
        return null;
    }


    public void BuyItem(int n)
    {
        int ItemPriceValue = ShopItem[n].price;
        if (ShopItem[n] != null)
        {
            if (money.PayGold(ItemPriceValue) == true)
            {
                inventoryManager.Add(ShopItem[n]);
                itemNames[n].text = "Sold!";
                itemPrices[n].text = "";
                ShopItem[n] = null;

            }
        }
    }

}
