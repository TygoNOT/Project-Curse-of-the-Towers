using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public Image slotImage;

    [SerializeField]
    public TMP_Text sloTName;

    [SerializeField] public EquipmentSO equippedItem;

    [SerializeField]
    public ItemType itemType = new ItemType();
    [SerializeField]
    public Attribute attribute = new Attribute();
    [SerializeField]
    public Sprite itemSprite;
    [SerializeField]
    public string itemName;
    [SerializeField]
    public string itemSet;
    [SerializeField]
    public string itemDescription;

    public InventoryManager inventoryManager;
    public EquipmentSOLibrary equipmentSOLibrary;
    public EquipmentSlot equipmentSlot;

    public GameObject petMenu;
    public GameObject equipmentPanel;

    public bool slotInUse;

    [SerializeField]
    public GameObject selectedShader;

    [SerializeField]
    public bool thisItemSelected;

    [SerializeField]
    public Sprite emptySprite;
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
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        equipmentSOLibrary = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSOLibrary>();
        equipmentSlot = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSlot>();
    }
    void OnLeftClick()
    {
        if (slotInUse)
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
            {
                if (slotInUse && this.itemName == equipmentSOLibrary.equipmentSO[i].itemName)
                    equipmentSOLibrary.equipmentSO[i].PreviewEquipment();
            }
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            GameObject.Find("StatsManager").GetComponent<PlayerStats>().TurnOffPreviewStats();
        }
    }
    void OnRightClick()
    {
        UnEquipGear();
    }
    public void EquipGear(Sprite itemSprite, string itemName, string itemDescription)
    {
        if (slotInUse)
            UnEquipGear();
        this.itemSprite = itemSprite;
        slotImage.sprite = this.itemSprite;
        sloTName.enabled = false;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
        {
            if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
            {
                equipmentSOLibrary.equipmentSO[i].EquipItem();
            }
        }
        slotInUse = true;

    }
    private void Update()
    {
        if (equipmentPanel.activeSelf)
        {
            for (int j = 0; j < inventoryManager.equipmentSlot.Length; j++)
            {
                if (inventoryManager.equipmentSlot[j].name != "" && !inventoryManager.equipmentSlot[j].isFull)
                {
                    inventoryManager.equipmentSlot[j].itemName = "";
                    inventoryManager.equipmentSlot[j].itemDescription = "";
                    inventoryManager.equipmentSlot[j].itemSprite = emptySprite;
                    inventoryManager.equipmentSlot[j].quantity = 0;
                    inventoryManager.equipmentSlot[j].isFull = false;
                    inventoryManager.equipmentSlot[j].thisItemSelected = false;
                }

            }
        }
        if (petMenu.activeSelf)
        {
            for (int j = 0; j < inventoryManager.petSlot.Length; j++)
            {
                if (inventoryManager.petSlot[j].name != "" && !inventoryManager.petSlot[j].isFull)
                {
                    inventoryManager.petSlot[j].itemName = "";
                    inventoryManager.petSlot[j].itemDescription = "";
                    inventoryManager.petSlot[j].itemSprite = emptySprite;
                    inventoryManager.petSlot[j].quantity = 0;
                    inventoryManager.petSlot[j].isFull = false;
                    inventoryManager.petSlot[j].thisItemSelected = false;
                }

            }
        }
    }
    public void AddImage(Sprite itemKartinka)
    {
        itemSprite = itemKartinka;
        slotImage.sprite = itemKartinka;
    }
    public void UnEquipGear()
    {
        if (slotInUse)
        {
            inventoryManager.DeselectAllSlots();

            inventoryManager.AddItem(itemName, 1, itemSprite, itemDescription, itemType, attribute);
            this.itemSprite = emptySprite;
            slotImage.sprite = this.emptySprite;
            sloTName.enabled = true;

            for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
            {
                if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
                { 
                    equipmentSOLibrary.equipmentSO[i].UnEquipItem();
                    equipmentSlot.RemoveSetBonus();
                    for (int z = 0; z < equipmentSOLibrary.equipmentSO.Length; z++)
                    {
                        if (equipmentSOLibrary.equipmentSO[z].itemName == itemName)
                        {
                            itemSet = equipmentSOLibrary.equipmentSO[z].setName;
                        }
                    }
                    for (int x = 0; x < equipmentSlot.SetCounter.Length; x++)
                    {
                        if (equipmentSlot.SetCounter[x] == itemSet)
                        {
                            equipmentSlot.SetCounter[x] = null;
                            break;
                        }
                    }
                }

            }
            slotInUse = false;

            GameObject.Find("StatsManager").GetComponent<PlayerStats>().TurnOffPreviewStats();
        }
    }
}

