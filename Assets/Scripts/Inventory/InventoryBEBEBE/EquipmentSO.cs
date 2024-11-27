using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[SerializeField] public enum Rarity
{
    common,
    uncommon,
    rare,
    epic,
    legendary,
    mythic,
};
[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    public string itemName, id, setName;
    public int price, attack, hp, speed, critDmg, critChance;
    
    public Rarity rarity;

    [SerializeField]
    Sprite itemSprite;

    public void PreviewEquipment()
    {
        GameObject.Find("StatsManager").GetComponent<PlayerStats>().
            PreviewEquipmentStats(attack, hp, speed, critDmg, critChance, itemSprite, rarity);
    }

    public void EquipItem()
    {
        PlayerStats playerStats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
        playerStats.attack += attack;
        playerStats.hp += hp;
        playerStats.speed += speed;
        playerStats.critDmg += critDmg;
        playerStats.critChance += critChance;

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

        playerStats.UpdateEquipmentStats();
    }
    public void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }
}
