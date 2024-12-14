using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Attribute")]
    public string EnemyName;
    public Image healthBar;
    private PlayerController playerController;

    [Header("Stats")]
    public int maxhealth;
    public int health;
    public int minBaseDamage = 8;
    public int maxBaseDamage = 15;
    public int critChance = 2;
    public float critDamage = 1.5f;
    private int curentDamage;
    public int speed = 8;
    public Attribute enemyAttribute;


    [Header("Effect")]
    public bool canApplyParalysis = false;
    public int paralysisChance = 30;

    public bool canApplyBurn = false;
    public int burnChance = 20;

    public bool canApplyPoison = false;
    public int poisonChance = 20;

    public bool canApplySilence = false;
    public int silenceChance = 15;

    public bool canApplyFreeze = false;
    public int freezeChance = 25;

    [Header("Debuff")]
    public bool isBurned = false;
    public int burnTurns = 0;

    public bool isFrozen = false;
    public int freezeTurns = 0;

    public bool isParalyzed = false;
    public int paralysisTurns = 0;

    private void Start()
    {
        curentDamage = 0;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Attack()
    {
        float damageMultiplier = playerController.CalculateAttributeDamageMultiplier(enemyAttribute, playerController.weaponAttribute);


        int crit = Random.Range(0, 100);
        if (crit <= critChance)
        {
            minBaseDamage = Mathf.RoundToInt(minBaseDamage * critDamage);
            maxBaseDamage = Mathf.RoundToInt(maxBaseDamage * critDamage);
        }
        int dmg = Mathf.RoundToInt(Random.Range(minBaseDamage, maxBaseDamage) * damageMultiplier); 
        Debug.Log(EnemyName + " " + dmg);

        TryApplyEffects();
        curentDamage = 0;
        playerController.TakeDamage(dmg);
    }

    public void TryApplyEffects()
    {
        if (canApplyParalysis && Random.Range(0, 100) < paralysisChance)
        {
            playerController.ApplyParalysis();
        }

        if (canApplyBurn && Random.Range(0, 100) < burnChance)
        {
            playerController.ApplyBurn();
        }

        if (canApplyPoison && Random.Range(0, 100) < poisonChance)
        {
            playerController.ApplyPoison();
        }

        if (canApplySilence && Random.Range(0, 100) < silenceChance)
        {
            playerController.ApplySilence();
        }

        if (canApplyFreeze && Random.Range(0, 100) < freezeChance)
        {
            playerController.ApplyFreeze();
        }
    }

    public void ApplyDamage()
    {
        playerController.TakeDamage(curentDamage);
    }

    public void TakeDamage(int dmgTaken)
    {
        health -= dmgTaken;
        if (health < 0) health = 0;
        StartCoroutine(UpdateHealthBarDelayed());
        CheckDeath();
    }
    public IEnumerator UpdateHealthBarDelayed()
    {
        yield return new WaitForSeconds(0.5f);

        float healthPercentage = (float)health / maxhealth;
        float maxBarWidth = healthBar.GetComponent<RectTransform>().rect.width;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(
            maxBarWidth * healthPercentage,
            healthBar.GetComponent<RectTransform>().sizeDelta.y
        );
    }

    public void UpdateHealthBar()
    {
        float healthPercentage = (float)health / maxhealth;
        float maxBarWidth = healthBar.GetComponent<RectTransform>().rect.width;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(
            maxBarWidth * healthPercentage,
            healthBar.GetComponent<RectTransform>().sizeDelta.y
        );
    }

    public void CheckDeath()
    {
        if (health <= 0)
        {
            health = 0;
            Debug.Log(EnemyName + " has been defeated!");

            StartCoroutine(DeathDelay());

        }
    }

    public IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);

        FindObjectOfType<CombatController>().EnemyDefeated();
        FindObjectOfType<CombatController>().OnEnemyDefeated(this);
    }

    public void ApplyBurn(int duration)
    {
        if (!isBurned)
        {
            isBurned = true;
            burnTurns = duration; 
            Debug.Log($"{EnemyName} is burned for 3 turns!");
        }
    }

    public void ApplyFreeze(int duration)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            freezeTurns = duration;
            Debug.Log(EnemyName + " is now frozen for " + freezeTurns + " turns.");
        }
    }
    public void ApplyParalysis(int duration)
    {
        if (!isParalyzed)
        {
            isParalyzed = true;
            paralysisTurns = duration;
            speed = Mathf.RoundToInt(speed * 0.5f);
            Debug.Log(EnemyName + " is now paralyzed for " + paralysisTurns + " turns.");
        }
    }

    public void EndTurnEffects()
    {
        if (isBurned && burnTurns > 0)
        {
            int burnDamage = 2;
            TakeDamage(burnDamage);
            burnTurns--;
            Debug.Log($"{EnemyName} takes {burnDamage} burn damage!");

            if (burnTurns <= 0)
            {
                isBurned = false;
                Debug.Log($"{EnemyName} is no longer burned!");
            }
        }
        else if(freezeTurns > 0)
        {
            freezeTurns--;
            Debug.Log(EnemyName + " has " + freezeTurns + " turns left of freeze.");
            if (freezeTurns == 0)
            {
                Debug.Log(EnemyName + " is no longer frozen!");
            }
            return;
        }
        else if(paralysisTurns > 0)
        {
            paralysisTurns--;
            if (paralysisTurns == 0)
            {
                isParalyzed = false;
                speed *= 2;
                Debug.Log($"{EnemyName} is no longer paralyzed!");
            }
        }
    }

    public bool CanAct()
    {
        if (isParalyzed && Random.Range(0, 100) < 20)
        {
            Debug.Log($"{EnemyName} is paralyzed and cannot act this turn!");
            return false;
        }
        return true;
    }
}