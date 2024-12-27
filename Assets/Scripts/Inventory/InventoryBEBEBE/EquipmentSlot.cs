using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{

    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    public ItemType itemType;
    public Attribute attribute;
    public string slotName;
    public string[] SetCounter = new string[5];
    public Text setBonus;

    int Setattack = 0;
    int Sethp = 0;
    int Setspeed = 0;
    int SetcritChance = 0;
    int SetcritDmg = 0;

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private EquippedSlot headArmorSlot, chestArmorSlot, legsArmorSlot, footArmorSlot, weaponSlot;

    public GameObject selectedShader;
    public bool thisItemSelected;
    private InventoryManager inventoryManager;
    private EquipmentSOLibrary equipmentSOLibrary;
    public GameObject deleteAcceptance;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        equipmentSOLibrary = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSOLibrary>();
        setBonus = GameObject.Find("StatsManager").GetComponent<PlayerStats>().SetBonus;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    public void OnLeftClick()
    {
        if (isFull && !deleteAcceptance.activeSelf)
        {
            if (thisItemSelected)
            {
                EquipGear();
                CombatController combatController = FindObjectOfType<CombatController>();
                if (combatController != null)
                {
                    combatController.TogglePlayerTurn();
                }
            }
            else
            {
                inventoryManager.DeselectAllSlots();
                selectedShader.SetActive(true);
                thisItemSelected = true;
                for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
                {
                    if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
                        equipmentSOLibrary.equipmentSO[i].PreviewEquipment();
                }
            }
        }
        else
        {
            GameObject.Find("StatsManager").GetComponent<PlayerStats>().TurnOffPreviewStats();
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
        }
    }
    private void EquipGear()
    {
        if (itemType == ItemType.headArmor)
        {
            headArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
            for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
            {
                if (equipmentSOLibrary.equipmentSO[i].itemName == itemName)
                {
                    SetCounter[0] = equipmentSOLibrary.equipmentSO[i].setName;
                    AddSetBonus();
                }
            }
        }
        if (itemType == ItemType.chestArmor)
        {
            chestArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
            for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
            {
                if (equipmentSOLibrary.equipmentSO[i].itemName == itemName)
                {
                    SetCounter[1] = equipmentSOLibrary.equipmentSO[i].setName;
                    AddSetBonus();
                }
            }
        }
        if (itemType == ItemType.legsArmor)
        {
            legsArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
            for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
            {
                if (equipmentSOLibrary.equipmentSO[i].itemName == itemName)
                {
                    SetCounter[2] = equipmentSOLibrary.equipmentSO[i].setName;
                    AddSetBonus();
                }
            }
        }
        if (itemType == ItemType.footArmor)
        {
            footArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
            for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
            {
                if (equipmentSOLibrary.equipmentSO[i].itemName == itemName)
                {
                    SetCounter[3] = equipmentSOLibrary.equipmentSO[i].setName;
                    AddSetBonus();
                }
            }
        }
        if (itemType == ItemType.weapon)
        {
            weaponSlot.EquipGear(itemSprite, itemName, itemDescription);
            for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
            {
                if (equipmentSOLibrary.equipmentSO[i].itemName == itemName)
                {
                    SetCounter[4] = equipmentSOLibrary.equipmentSO[i].setName;
                    AddSetBonus();
                }
            }
        }

        EmptySlot();
    }

    private void EmptySlot()
    {
        itemImage.sprite = emptySprite;
        isFull = false;
    }

    public void OnRightClick()
    {
        //GameObject itemToDrop = new GameObject(itemName);
        //Item newItem = itemToDrop.AddComponent<Item>();
        //newItem.quantity = 1;
        //newItem.itemName = itemName;
        //newItem.sprite = itemSprite;
        //newItem.itemDescription = itemDescription;

        //SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        //sr.sprite = itemSprite;
        //sr.sortingOrder = 5;
        //sr.sortingLayerName = "Ground";

        //itemToDrop.AddComponent<BoxCollider2D>();

        //itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(1, 0, 0);
        //itemToDrop.transform.localScale = new Vector3(.5f, .5f, .5f);
        if (isFull)
        {
            for (int i = 0; i < inventoryManager.equipmentSlot.Length; i++)
            {
                if (inventoryManager.equipmentSlot[i].isFull && inventoryManager.equipmentSlot[i].selectedShader.activeSelf)
                {
                    slotName = inventoryManager.equipmentSlot[i].name;
                    deleteAcceptance.SetActive(true);
                    GameObject.Find("AcceptButton").GetComponent<DeleteAcceptButton>().slotName = slotName;
                }
            }
        }
    }
    public void dropItem()
    {
        this.quantity -= 1;
        if (this.quantity <= 0)
        {
            EmptySlot();
            deleteAcceptance.SetActive(false);
            GameObject.Find("StatsManager").GetComponent<PlayerStats>().TurnOffPreviewStats();
            inventoryManager.DeselectAllSlots();
            thisItemSelected = false;
        }
    }
    public void AddImage(Sprite itemSprite)
    {
        itemImage.sprite = itemSprite;
    }
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType, Attribute attribute)
    {
        if (isFull)
            return quantity;


        this.itemName = itemName;
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;
        this.itemDescription = itemDescription;
        this.itemType = itemType;
        this.attribute = attribute;


        this.quantity = 1;
        isFull = true;
        return 0;
    }

    public void AddSetBonus()
    {
        if (SetCounter[0] != null && SetCounter[0] == SetCounter[1] && SetCounter[0] == SetCounter[2] && SetCounter[0] == SetCounter[3] && SetCounter[0] == SetCounter[4])
        {

            PlayerStats playerStats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();

            if (SetCounter[0] == "Bebe Set")
            {
                Setattack = 2;
                Sethp = 0;
                Setspeed = 0;
                SetcritChance = 0;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +2 Attack";
            }
            if (SetCounter[0] == "Common Set2")
            {
                Setattack = 0;
                Sethp = 0;
                Setspeed = 2;
                SetcritChance = 0;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +2 Speed";
            }
            if (SetCounter[0] == "Common Set3")
            {
                Setattack = 0;
                Sethp = 2;
                Setspeed = 0;
                SetcritChance = 0;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +2 HP";
            }
            if (SetCounter[0] == "Common Set4")
            {
                Setattack = 2;
                Setspeed = 2;
                Sethp = 2;
                SetcritChance = 0;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +2 Attack,Speed,HP";
            }
            if (SetCounter[0] == "Rare Set1")
            {
                Setattack = 6;
                Sethp = 0;
                Setspeed = 6;
                SetcritChance = 0;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +6 Attack,Speed";
            }
            if (SetCounter[0] == "Rare Set2")
            {
                Setattack = 0;
                Sethp = 15;
                Setspeed = 0;
                SetcritChance = 0;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +15 HP";
            }
            if (SetCounter[0] == "Epic Set1")
            {
                Setattack = 0;
                Sethp = 11;
                Setspeed = 0;
                SetcritChance = 11;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +11 HP, +11% CritChance";
            }
            if (SetCounter[0] == "Common Set5")
            {
                Setattack = 5;
                Setspeed = 5;
                Sethp = 5;
                SetcritChance = 0;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +5 Attack,Speed,HP";
            }
            if (SetCounter[0] == "Golden Boy")
            {
                Setattack = -3;
                Sethp = 0;
                Setspeed = 10;
                SetcritChance = 0;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: -3 Attack, +10 Speed";
            }
            if (SetCounter[0] == "Demon Set")
            {
                Setattack = 13;
                Sethp = 0;
                Setspeed = 0;
                SetcritChance = 5;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +13 Attack, +5% CritChance";
            }
            if (SetCounter[0] == "Holy Demon Set")
            {
                Setattack = 0;
                Sethp = 13;
                Setspeed = 0;
                SetcritChance = 0;
                SetcritDmg = 1;
                setBonus.text = "Set bonus: +13 HP, +100% CritDamage";
            }
            if (SetCounter[0] == "Golden Wyvern")
            {
                Setattack = 15;
                Sethp = 0;
                Setspeed = 15;
                SetcritChance = 50;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +15 Attack, Speed, +50% CritChance";
            }
            if (SetCounter[0] == "Dark Knight")
            {
                Setattack = 20;
                Sethp = 20;
                Setspeed = 20;
                SetcritChance = 52;
                SetcritDmg = 0;
                setBonus.text = "Set bonus: +20 Attack,Speed,HP, +100% CritChance";
            }
            if (SetCounter[0] == "Holy Knight")
            {
                Setattack = 0;
                Sethp = 50;
                Setspeed = 0;
                SetcritChance = 22;
                SetcritDmg = 2;
                setBonus.text = "Set bonus: +50 HP, +22% CritChance, +200% CritDamage";
            }
            if (SetCounter[0] == "Void Dragon")
            {
                Setattack = 40;
                Sethp = 40;
                Setspeed = 40;
                SetcritChance = 100;
                SetcritDmg = 2;
                setBonus.text = "Set bonus: +40 Attack,Speed,HP, +100% CritChance, +200% CritDamage";
            }
            playerStats.attack += Setattack;
            playerStats.hp += Sethp;
            playerStats.speed += Setspeed;
            playerStats.critDmg += SetcritDmg;
            playerStats.critChance += SetcritChance;
            playerStats.UpdateEquipmentStats();

        }

    }

    public void RemoveSetBonus()
    {
        PlayerStats playerStats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
        playerStats.attack -= Setattack;
        playerStats.hp -= Sethp;
        playerStats.speed -= Setspeed;
        playerStats.critDmg -= SetcritDmg;
        playerStats.critChance -= SetcritChance;
        Setattack = 0;
        Sethp = 0;
        Setspeed = 0;
        SetcritChance = 0;
        SetcritDmg = 0;
        setBonus.text = "Set bonus: None";
        playerStats.UpdateEquipmentStats();
    }


}

