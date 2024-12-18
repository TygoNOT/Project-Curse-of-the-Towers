[System.Serializable]
public class SerializedEquippableSlot
{
    public string equipmentSOName; // Имя EquipmentSO для восстановления
    public string itemSpriteName;  // Имя спрайта
    public int quantity;           // Количество
    public bool isEquipped;        // Используется ли слот
    public string itemName;        // Название предмета
    public string itemDescription; // Описание предмета
    public Attribute attribute;    // Атрибут предмета
    public ItemType itemType;      // Тип предмета
}