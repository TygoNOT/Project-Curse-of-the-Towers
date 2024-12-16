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
            headArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.chestArmor)
            chestArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.legsArmor)
            legsArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.footArmor)
            footArmorSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.weapon)
            weaponSlot.EquipGear(itemSprite, itemName, itemDescription);

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


        this.quantity =1;
        isFull = true;
        return 0;
    }
}

