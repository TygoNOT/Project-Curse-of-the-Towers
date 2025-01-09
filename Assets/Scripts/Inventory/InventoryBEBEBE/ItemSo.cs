using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public enum EffectType
{
    None,
    Paralysis,
    Poison,
    Burn,
    Freeze,
    Silence
}
[CreateAssetMenu]
public class ItemSo : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public int price;
    public int quantity;
    public int id;
    public StatToChange statToChange= new StatToChange();
    public int amountToChangeStat;
    public AttributesToChange attributesToChange= new AttributesToChange();
    public int amountToChangeAttribute;
    public ItemType itemType = new ItemType();

    [Header("Item Use")]
    public EffectType effectToRemove = EffectType.None;
    public Attribute attribute = Attribute.None;
    public bool isHealthPotion = false;
    public int hpRecovery = 10;

    public bool isBandage = false;
    public int smallheal = 10;

    public bool isTeleportationScroll = false;

    [TextArea]
    [SerializeField]
    public string itemDescription;
    [SerializeField]
    public Sprite sprite;

    public bool UseItem()
    {
        if (itemType == ItemType.consumable)
        {

            if (effectToRemove != EffectType.None)
            {
                PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                if (player != null)
                {
                    player.RemoveEffect(effectToRemove); 
                    Debug.Log($"{itemName} used. Effect {effectToRemove} removed!");
                    return true; 
                }
            }
            else if (isHealthPotion)
            {
                PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                    if(player != null)
                {
                    player.Heal(hpRecovery);
                    Debug.Log($"{itemName} used. {hpRecovery} HP restored ");
                    return true;
                }
            }
            else if (isBandage)
            {
                PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Heal(smallheal);
                    Debug.Log($"{itemName} used. {smallheal} HP restored ");
                    return true;
                }
            }
            else if (isTeleportationScroll)
            {
                PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TeleportationScroll();
                    return true;
                }
            }
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
