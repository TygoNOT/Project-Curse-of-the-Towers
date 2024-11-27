using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item2 : ScriptableObject
{
    public byte id;
    public string itemName;
    public string quality;
    public string SetName;
    public Sprite icon;
    public int price;
    public int dmg;
    public int critChance;
    public int critDmg;
}