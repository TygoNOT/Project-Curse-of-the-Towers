[System.Serializable]
public class SerializedSlot
{
    public string itemName;
    public string itemDescription;
    public string itemSpriteName; // ��� ����� ������� �� Resources
    public int quantity;
    public string itemType; // ������� ��� ������
    public string attribute; // ������� ��� ������
    public bool isFull;
    public bool isTeleportationScroll = false;
    public bool isHealthPotion = false;
    public bool isBandage = false;
}
