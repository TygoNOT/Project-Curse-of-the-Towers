using UnityEngine;

public class LightningPet : PetController
{
    [Header("Pet Attributes")]
    public int minLightningDamage;
    public int maxLightningDamage;

    public override void UseAbility(PlayerController player)
    {
        if (IsReadyToUse())
        {
            LightningStrike(player);
            currentCooldownTurns = cooldownTurns;
        }

        else
        {
            Debug.Log($"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).");
        }
    }

    public void LightningStrike(PlayerController player)
    {
        Debug.Log("Pet uses Lightning Strike!");

        var targetEnemy = player.combatController.enemies[Random.Range(0, player.combatController.enemies.Length)];

        int lightningDamage = Random.Range(minLightningDamage, maxLightningDamage);
        targetEnemy.GetComponent<EnemyController>().TakeDamage(lightningDamage);
        Debug.Log($"Lightning strike dealt {lightningDamage} damage to {targetEnemy.EnemyName}");

       
    }
}
