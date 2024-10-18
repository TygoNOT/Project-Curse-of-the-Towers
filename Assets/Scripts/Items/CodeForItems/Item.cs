using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public byte id;
    public string itemName;
    public string quality;
    public string SetName;
    public Sprite icon;
    public long price;
    public int dmg;
    public int critChance;
    public int critDmg;
}