using UnityEngine;

public class PetFireUser : PetController
{
    [Header("Pet Attributes")]
    public int minFireBallDamage;
    public int maxFireBallDamage;
    public int BurnChance;
    public int BurnTurns = 3;

    public override void UseAbility(PlayerController player)
    {
        if (IsReadyToUse())
        {
            FireballAbility(player);
            currentCooldownTurns = cooldownTurns;
        }

        else
        {
            //combatController.PlayerMessage.text = $"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).";
            Debug.Log($"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).");
        }
    }
    private void FireballAbility(PlayerController player)
    {
        foreach (EnemyController enemy in player.combatController.enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                int fireballDamage = Random.Range(minFireBallDamage, maxFireBallDamage);
                //combatController.PlayerMessage.text = $"{petName} uses Fireball!";
                Debug.Log($"{petName} uses Fireball on {enemy.EnemyName}, dealing {fireballDamage} damage!");

                enemy.TakeDamage(fireballDamage);

                if (Random.Range(0, 100) < BurnChance)
                {
                    enemy.ApplyBurn(BurnTurns);
                    //combatController.PlayerMessage.text = $"{enemy.EnemyName} is burned for 3 turns!";
                    Debug.Log($"{enemy.EnemyName} is burned for 3 turns!");
                }
            }
        }
    }
}
