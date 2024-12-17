using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int attack, hp, speed, critChance;
    public float critDmg;
    public Attribute attribute;
    [SerializeField]
    private TMP_Text attackText, hpText, speedText, critDmgText, critChanceText, attributeText;

    [SerializeField]
    private TMP_Text attackPreText, hpPreText, speedPreText, critDmgPreText, critChancePreText, attributePreText;

    [SerializeField]
    private Image previewImage;

    [SerializeField]
    private GameObject selectedItemStats;

    [SerializeField]
    private GameObject selectedItemImage;
    void Start()
    {
        //UpdateEquipmentStats();
    }

    public void UpdateEquipmentStats()
    {
        attackText.text = attack.ToString();
        hpText.text = hp.ToString();
        speedText.text = speed.ToString();
        critDmgText.text = critDmg.ToString();
        critChanceText.text = critChance.ToString();
        attributeText.text = attribute.ToString();
        
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.InitializeStats();
        }
    }

    public void PreviewEquipmentStats(int attack, int hp, int speed, float critDmg, int critChance, Sprite itemSprite, Rarity rarity, Attribute attribute)
    {
        if (rarity == Rarity.common)
        {
            attackPreText.color = Color.gray;
            hpPreText.color = Color.gray;
            speedPreText.color = Color.gray;
            critDmgPreText.color = Color.gray;
            critChancePreText.color = Color.gray;
        } 
        else if (rarity == Rarity.rare)
        {
            attackPreText.color = Color.blue;
            hpPreText.color = Color.blue;
            speedPreText.color = Color.blue;
            critDmgPreText.color = Color.blue;
            critChancePreText.color = Color.blue;
        }
        else if (rarity == Rarity.epic)
        {
            attackPreText.color = new Color32(128, 0, 128, 255); ;
            hpPreText.color = new Color32(128, 0, 128, 255); ;
            speedPreText.color = new Color32(128, 0, 128, 255); ;
            critDmgPreText.color = new Color32(128, 0, 128, 255); ;
            critChancePreText.color = new Color32(128, 0, 128, 255); ;
        }
        else if (rarity == Rarity.legendary)
        {
            attackPreText.color = Color.yellow;
            hpPreText.color = Color.yellow;
            speedPreText.color = Color.yellow;
            critDmgPreText.color = Color.yellow;
            critChancePreText.color = Color.yellow;
        }
        else if (rarity == Rarity.mythic)
        {
            attackPreText.color = Color.red;
            hpPreText.color = Color.red;
            speedPreText.color = Color.red;
            critDmgPreText.color = Color.red;
            critChancePreText.color = Color.red;
        }
        attackPreText.text = attack.ToString();
        hpPreText.text = hp.ToString();
        speedPreText.text = speed.ToString();
        critDmgPreText.text = critDmg.ToString();
        critChancePreText.text = critChance.ToString();
        attributePreText.text = attribute.ToString();
        previewImage.sprite = itemSprite;
        selectedItemImage.SetActive(true);
        selectedItemStats.SetActive(true);
    }

    public void TurnOffPreviewStats()
    {
        selectedItemImage.SetActive(false);
        selectedItemStats.SetActive(false);
    }
}

