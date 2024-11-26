using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    public string itemName;
    public int attack, hp, speed, critDmg, critChance;

    [SerializeField]
    Sprite itemSprite;

    public void PreviewEquipment()
    {
        GameObject.Find("StatsManager").GetComponent<PlayerStats>().
            PreviewEquipmentStats(attack, hp, speed, critDmg, critChance, itemSprite);
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
}
