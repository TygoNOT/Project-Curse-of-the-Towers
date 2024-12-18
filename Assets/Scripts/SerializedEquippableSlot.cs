[System.Serializable]
public class SerializedEquippableSlot
{
    public string equipmentSOName; // ��� EquipmentSO ��� ��������������
    public string itemSpriteName;  // ��� �������
    public int quantity;           // ����������
    public bool isEquipped;        // ������������ �� ����
    public string itemName;        // �������� ��������
    public string itemDescription; // �������� ��������
    public Attribute attribute;    // ������� ��������
    public ItemType itemType;      // ��� ��������
}