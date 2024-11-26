using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int attack, hp, speed, critDmg, critChance;

    [SerializeField]
    private TMP_Text attackText, hpText, speedText, critDmgText, critChanceText;

    [SerializeField]
    private TMP_Text attackPreText, hpPreText, speedPreText, critDmgPreText, critChancePreText;

    [SerializeField]
    private Image previewImage;

    [SerializeField]
    private GameObject selectedItemStats;

    [SerializeField]
    private GameObject selectedItemImage;
    void Start()
    {
        UpdateEquipmentStats();
    }

    public void UpdateEquipmentStats()
    {
        attackText.text = attack.ToString();
        hpText.text = hp.ToString();
        speedText.text = speed.ToString();
        critDmgText.text = critDmg.ToString();
        critChanceText.text = critChance.ToString();
    }

    public void PreviewEquipmentStats(int attack, int hp, int speed, int critDmg, int critChance, Sprite itemSprite)
    {
        attackPreText.text = attack.ToString();
        hpPreText.text = hp.ToString();
        speedPreText.text = speed.ToString();
        critDmgPreText.text = critDmg.ToString();
        critChancePreText.text = critChance.ToString();
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

