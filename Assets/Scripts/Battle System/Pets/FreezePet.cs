using UnityEngine;

public class FreezePet : PetController
{
    [Header("Pet Attributes")]
    public int minSnowBallDamage;
    public int maxSnowBallDamage;
    public int freezeChance;
    public int freezeTurns = 3;
    public override void UseAbility(PlayerController player)
    {
        if (IsReadyToUse())
        {
            SnowballAbility(player);
            currentCooldownTurns = cooldownTurns;
        }

        else
        {
            Debug.Log($"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).");
        }
    }

    public void SnowballAbility(PlayerController player)
    {
        Debug.Log("Pet is using Snowball Attack!");

        var targetEnemy = player.combatController.enemies[Random.Range(0, player.combatController.enemies.Length)];

        int snowballDamage = Random.Range(minSnowBallDamage, maxSnowBallDamage);
        targetEnemy.GetComponent<EnemyController>().TakeDamage(snowballDamage);
        Debug.Log("Enemy took " + snowballDamage + " snowball damage!");

        if (Random.Range(0, 100) < freezeChance)
        {
            targetEnemy.ApplyFreeze(freezeTurns);  
            Debug.Log("Enemy is frozen!");
        }
    }
}
