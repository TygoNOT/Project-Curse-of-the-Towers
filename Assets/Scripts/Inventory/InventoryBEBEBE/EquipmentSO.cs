using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    public string itemName, id, setName;
    public int price, attack, hp, speed, critChance;
    public float critDmg;
    public Attribute attribute;
    public Rarity rarity;
    [SerializeField] public ItemType itemType;
    [SerializeField]
    Sprite itemSprite;

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
