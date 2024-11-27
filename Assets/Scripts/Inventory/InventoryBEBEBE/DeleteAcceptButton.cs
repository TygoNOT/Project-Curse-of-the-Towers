using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAcceptButton : MonoBehaviour
{
    public string slotName;
    [SerializeField]
    EquipmentSlot equipmentSlot;
    public void OnMouseDown()
    {
        equipmentSlot = GameObject.Find(slotName).GetComponent<EquipmentSlot>();
        equipmentSlot.dropItem();
    }
}
