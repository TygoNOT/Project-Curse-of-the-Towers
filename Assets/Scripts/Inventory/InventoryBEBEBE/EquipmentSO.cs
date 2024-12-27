using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[SerializeField] public enum Attribute
{
    None,
    Spirit, 
    Darkness, 
    Order, 
    Chaos, 
    Void 
}
[SerializeField] public enum Rarity
{
    common,
    rare,
    epic,
    legendary,
    mythic,
};

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    public Rarity rarity;
    public string itemName, id, setName;
    public string[] SetCounter = new string[5];
    public int price, attack, hp, speed, critChance;
    public float critDmg;
    public Attribute attribute;
    public Text SetBonus;
    [SerializeField] public ItemType itemType;
    [SerializeField]
    public Sprite itemSprite;
    [SerializeField]
    public string itemDescription;
    public void PreviewEquipment()
    {
        GameObject.Find("StatsManager").GetComponent<PlayerStats>().
            PreviewEquipmentStats(attack, hp, speed, critDmg, critChance, itemSprite, rarity, attribute);
    }

    public void EquipItem()
    {
        PlayerStats playerStats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
        playerStats.attack += attack;
        playerStats.hp += hp;
        playerStats.speed += speed;
        playerStats.critDmg += critDmg;
        playerStats.critChance += critChance;
        playerStats.attribute = attribute;
        playerStats.UpdateEquipmentStats();
    }


    public void UnEquipItem()
    {
        PlayerStats playerStats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
        playerStats.attack -= attack;
        playerStats.hp -= hp;
        playerStats.speed -= speed;
        playerStats.critDmg -= critDmg;
        playerStats.critChance -= critChance;
        playerStats.attribute -= attribute;
        playerStats.UpdateEquipmentStats();
    }
    public void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }
}
