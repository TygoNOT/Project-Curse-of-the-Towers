using UnityEngine;
using UnityEngine.UI;

public class PetHealer : PetController
{
    [Header("Pet Attributes")]
    public int healingAmount = 20;
    public int regenAmount = 5;
    public int regenTurns = 3;
    private int currentRegenTurns = 0;


    public override void UseAbility(PlayerController player)
    {
        if (IsReadyToUse())
        {
            player.currentHP += healingAmount;
            if (player.currentHP > player.maxhealth)
                player.currentHP = player.maxhealth;

            float healthPercentage = (float)player.currentHP / player.maxhealth;
            float newWidth = healthPercentage * player.originalHealthBarWidth;
            player.healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, player.healthBar.GetComponent<RectTransform>().sizeDelta.y);
            currentRegenTurns = regenTurns;
            currentCooldownTurns = cooldownTurns;
            
            //combatController.PlayerMessage.text = $"{petName} heals the {player.playername} on {healingAmount}";
            Debug.Log($"{petName} heals the {player.playername} on {healingAmount}");
        }
        else
        {
            //combatController.PlayerMessage.text = $"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).";
            Debug.Log($"{petName} is not ready for use. Waiting for {currentCooldownTurns} turn(s).");
        }
    }

    public void ApplyRegeneration(PlayerController player)
    {
        if (currentRegenTurns > 0)
        {
            player.TakeDamage(-regenAmount);
            currentRegenTurns--;
            //combatController.PlayerMessage.text = $"{petName} regenerates the {player.playername} with {regenAmount} hp";
            Debug.Log($"{petName} regenerates the {player.playername} with {regenAmount} hp");
        }
    }
}
