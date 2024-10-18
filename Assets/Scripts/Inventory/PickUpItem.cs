using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Item item;
    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }
    private void OnMouseDown()
    {
        Pickup();
    }
}
