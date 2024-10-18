using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Attribute")]
    public string unitName;
    public int damage;
    public int MaxHP;
    public int currentHP;

    public bool TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            return true;
        else 
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > MaxHP) 
            currentHP = MaxHP;
    }
}
