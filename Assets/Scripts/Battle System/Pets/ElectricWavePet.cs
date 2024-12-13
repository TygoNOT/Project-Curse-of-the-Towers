using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWavePet : PetController
{
    [Header("Pet Attributes")]
    public int paralysisTurns = 3;

    public override void UseAbility(PlayerController player)
    {
        if (IsReadyToUse())
        {
            ElectricWave(player);
            currentCooldownTurns = cooldownTurns;
        }

        else
        {
            Debug.Log($"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).");
        }
    }

    public void ElectricWave(PlayerController player)
    {
        Debug.Log("Pet uses Electric Wave!");
        var targetEnemy = player.combatController.enemies[Random.Range(0, player.combatController.enemies.Length)];

        targetEnemy.ApplyParalysis(paralysisTurns);
        Debug.Log($"{targetEnemy.EnemyName} is paralyzed!");
    }
}
