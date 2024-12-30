[System.Serializable]
public class SerializedSlot
{
    public string itemName;
    public string itemDescription;
    public string itemSpriteName; // Имя файла спрайта из Resources
    public int quantity;
    public string itemType; // Хранить как строку
    public string attribute; // Хранить как строку
    public bool isFull;
    public bool isTeleportationScroll = false;
    public bool isHealthPotion = false;
    public bool isBandage = false;
}
