using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetWind : PetController
{
    [Header("Pet Attributes")]
    public int buffTurns = 3; 
    public float speedMultiplier = 1.5f;
    public int critChanceIncrease = 10;
    public float critDamageMultiplier = 2.0f;

    public override void UseAbility(PlayerController player)
    {
        Debug.Log("WindBuffPet: Applying wind buff.");
        player.ApplyWindBuff(buffTurns, speedMultiplier, critChanceIncrease, critDamageMultiplier);
        currentCooldownTurns = cooldownTurns;
    }


}
