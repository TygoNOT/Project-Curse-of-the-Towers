using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSo : ScriptableObject
{
    public string itemName;
    public int id;
    public StatToChange statToChange= new StatToChange();
    public int amountToChangeStat;
    public AttributesToChange attributesToChange= new AttributesToChange();
    public int amountToChangeAttribute;


    public bool UseItem()
    {
        if (statToChange == StatToChange.health)
        {
            return true;
            
           // GameObject.Find("HealthManager").GetComponent<PlayerHealth>().ChangeHealth(amountToChangeStat);
        }
        return false;
    }





    public enum AttributesToChange
    {
        none,
        strength,
        defence,
        intelligence,
        agility
    };
    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina
    };
}
