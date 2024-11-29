using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    public ItemType itemType;
    public string slotName;

    [SerializeField]
    private int maxNumberOfItems;

    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;



    public Image ItemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;



    public GameObject selectedShader;
    public bool thisItemSelected;
    private InventoryManager inventoryManager;
    public GameObject deleteAcceptance;
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
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
        if (thisItemSelected)
        {

            bool usable = inventoryManager.UseItem(itemName);
            if (usable)
            {
                this.quantity -= 1;
                quantityText.text = this.quantity.ToString();
                if (this.quantity <= 0)
                {
                    EmptySlot();
                }
            }
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            ItemDescriptionImage.sprite = itemSprite;
            if (ItemDescriptionImage.sprite == null)
            {
                ItemDescriptionImage.sprite = emptySprite;
            }
        }
    }

    private void EmptySlot()
    {
        itemName = "";
        itemDescription = "";
        itemSprite = emptySprite;
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;

        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
        ItemDescriptionImage.sprite = emptySprite;
    }

    public void OnRightClick()
    {
        //GameObject itemToDrop = new GameObject(itemName);
        //Item newItem = itemToDrop.AddComponent<Item>();
        //newItem.quantity = 1;
        //newItem.itemName = itemName;
        //newItem.sprite = itemSprite;
        //newItem.itemDescription = itemDescription;

        //SpriteRenderer sr= itemToDrop.AddComponent<SpriteRenderer>();
        //sr.sprite = itemSprite;
        //sr.sortingOrder = 5;
        //sr.sortingLayerName = "Ground";

        //itemToDrop.AddComponent<BoxCollider2D>();

        //itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(1,0,0);
        //itemToDrop.transform.localScale = new Vector3(.5f,.5f,.5f);
        
        if (thisItemSelected)
        {
            for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
            {
                if (inventoryManager.itemSlot[i].quantity > 0 && inventoryManager.itemSlot[i].selectedShader.activeSelf)
                {
                    this.slotName = inventoryManager.itemSlot[i].name;
                    deleteAcceptance.SetActive(true);
                    GameObject.Find("AcceptButton").GetComponent<DeleteAcceptButton>().slotName = this.slotName;
                }
            }
        }
    }
    public void dropItem()
    {
        
        if (this.quantity > 0)
        {
            this.quantity -= 1;
            if (this.quantity <= 0)
            {
                EmptySlot();
                deleteAcceptance.SetActive(false);
                GameObject.Find("StatsManager").GetComponent<PlayerStats>().TurnOffPreviewStats();
                inventoryManager.DeselectAllSlots();
                thisItemSelected = false;
                quantityText.text = this.quantity.ToString();
            }
            else
            {
                quantityText.text = this.quantity.ToString();
            }
        }
    }
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
     {
        if (isFull)
            return quantity;

        
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;
        this.itemDescription = itemDescription;
        this.itemType = itemType;



        this.quantity += quantity;
        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = quantity.ToString();
            quantityText.enabled = true;
            isFull = true;


            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;

            return extraItems;
        }
        quantityText.text= this.quantity.ToString();
        quantityText.enabled = true;
        return 0;
     }
}
