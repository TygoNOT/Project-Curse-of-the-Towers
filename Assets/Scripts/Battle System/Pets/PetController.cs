using UnityEngine;

public class PetController : MonoBehaviour
{
    [Header("Pet Attributes")]
    public string petName;
    public int cooldownTurns = 3; 

    protected int currentCooldownTurns = 0;

    public bool IsReadyToUse()
    {
        return currentCooldownTurns <= 0;
    }

    public virtual void UseAbility(PlayerController player)
    {
        if (IsReadyToUse())
        {
            currentCooldownTurns = cooldownTurns;
        }
        else
        {
            Debug.Log($"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).");
        }
    }

    

    public void EndTurn()
    {
        if (currentCooldownTurns > 0)
        {
            currentCooldownTurns--;
        }
    }
}