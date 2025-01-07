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
            //combatController.PlayerMessage.text = $"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).";
            Debug.Log($"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).");
        }
    }

    public void SnowballAbility(PlayerController player)
    {
        //combatController.PlayerMessage.text = "Pet is using Snowball Attack!";
        Debug.Log("Pet is using Snowball Attack!");

        var targetEnemy = player.combatController.enemies[Random.Range(0, player.combatController.enemies.Length)];

        int snowballDamage = Random.Range(minSnowBallDamage, maxSnowBallDamage);
        targetEnemy.GetComponent<EnemyController>().TakeDamage(snowballDamage);
        //combatController.PlayerMessage.text = "Enemy took " + snowballDamage + " snowball damage!";
        Debug.Log("Enemy took " + snowballDamage + " snowball damage!");

        if (Random.Range(0, 100) < freezeChance)
        {
            targetEnemy.ApplyFreeze(freezeTurns);
            //combatController.PlayerMessage.text = "Enemy is frozen!";
            Debug.Log("Enemy is frozen!");
        }
    }
}
