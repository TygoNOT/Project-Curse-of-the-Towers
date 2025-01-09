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
    [SerializeField] public PetController petPrefab;

    public void PreviewEquipment()
    {
        GameObject.Find("StatsManager").GetComponent<PlayerStats>().
            PreviewEquipmentStats(attack, hp, speed, critDmg, critChance, itemSprite, rarity, attribute);
    }

    public void mEquipIte()
    {
        PlayerStats playerStats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
        playerStats.attack += attack;
        playerStats.hp += hp;
        playerStats.speed += speed;
        playerStats.critDmg += critDmg;
        playerStats.critChance += critChance;
        playerStats.attribute = attribute;
        if (itemType == ItemType.pet)
        {
            playerStats.petprefab = petPrefab;
        }
        playerStats.UpdateEquipmentStats();
        for (int i = 0; i < SetCounter.Length; i++)
        {
            if (SetCounter[i] == null)
            {
                SetCounter[i] = setName;
                AddSetBonus();
                break;
            }
        }
    }

    public void AddSetBonus()
    {
        if (SetCounter[0] != null && SetCounter[0] == SetCounter[1] && SetCounter[0] == SetCounter[2] && SetCounter[0] == SetCounter[3] && SetCounter[0] == SetCounter[4])
        {

            PlayerStats playerStats = GameObject.Find("StatsManager").GetComponent<PlayerStats>();
            int Setattack = 0;
            int Sethp = 0;
            int Setspeed = 0;
            int SetcritChance = 0;
            int SetcritDmg = 0;
            if (SetCounter[0] == "Bebe Set")
            {
                Setattack = 2;
                SetBonus.text = "Set bonus: +2 Attack";
            }
            if (SetCounter[0] == "Common Set2")
            {
                Setspeed = 2;
                SetBonus.text = "Set bonus: +2 Speed";
            }
            if (SetCounter[0] == "Common Set3")
            {
                Sethp = 2;
                SetBonus.text = "Set bonus: +2 HP";
            }
            if (SetCounter[0] == "Common Set4")
            {
                Setattack = 2;
                Setspeed = 2;
                Sethp = 2;
                SetBonus.text = "Set bonus: +2 Attack,Speed,HP";
            }
            if (SetCounter[0] == "Rare Set1")
            {
                Setattack = 6;
                Setspeed = 6;
                SetBonus.text = "Set bonus: +6 Attack,Speed";
            }
            if (SetCounter[0] == "Rare Set2")
            {
                Sethp = 15;
                SetBonus.text = "Set bonus: +15 HP";
            }
            if (SetCounter[0] == "Epic Set1")
            {
                Sethp = 11;
                SetcritChance = 11;
                SetBonus.text = "Set bonus: +11 HP, +11% CritChance";
            }
            if (SetCounter[0] == "Common Set5")
            {
                Setattack = 5;
                Setspeed = 5;
                Sethp = 5;
                SetBonus.text = "Set bonus: +5 Attack,Speed,HP";
            }
            if (SetCounter[0] == "Golden Boy")
            {
                Setattack = -3;
                Setspeed = 10;
                SetBonus.text = "Set bonus: -3 Attack, +10 Speed";
            }
            if (SetCounter[0] == "Demon Set")
            {
                Setattack = 13;
                SetcritChance = 5;
                SetBonus.text = "Set bonus: +13 Attack, +5% CritChance";
            }
            if (SetCounter[0] == "Holy Demon Set")
            {
                Sethp = 13;
                SetcritDmg = 1;
                SetBonus.text = "Set bonus: +13 HP, +100% CritDamage";
            }
            if (SetCounter[0] == "Golden Wyvern")
            {
                Setattack = 15;
                Setspeed = 15;
                SetcritChance = 50;
                SetBonus.text = "Set bonus: +15 Attack, Speed, +50% CritChance";
            }
            if (SetCounter[0] == "Dark Knight")
            {
                Setattack = 20;
                Sethp = 20;
                Setspeed = 20;
                SetcritChance = 52;
                SetBonus.text = "Set bonus: +20 Attack,Speed,HP, +100% CritChance";
            }
            if (SetCounter[0] == "Holy Knight")
            {
                Sethp = 50;
                SetcritChance = 22;
                SetcritDmg = 2;
                SetBonus.text = "Set bonus: +50 HP, +22% CritChance, +200% CritDamage";
            }
            if (SetCounter[0] == "Void Dragon")
            {
                Setattack = 40;
                Sethp = 40;
                Setspeed = 40;
                SetcritChance = 100;
                SetcritDmg = 2;
                SetBonus.text = "Set bonus: +40 Attack,Speed,HP, +100% CritChance, +200% CritDamage";
            }
            playerStats.attack += Setattack;
            playerStats.hp += Sethp;
            playerStats.speed += Setspeed;
            playerStats.critDmg += SetcritDmg;
            playerStats.critChance += SetcritChance;
            playerStats.UpdateEquipmentStats();

        }
        
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
        playerStats.petprefab = petPrefab;
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
        for (int i = 0; i < SetCounter.Length; i++)
        {
            if (SetCounter[i] == setName)
            {
                SetCounter[i] = null;
                //RemoveSetBonus();
                break;
            }
        }
    }
    public void OnValidate()
    {
        //string path = AssetDatabase.GetAssetPath(this);
        //id = AssetDatabase.AssetPathToGUID(path);
    }
}
