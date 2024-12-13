using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Attribute")]
    public string playername;
    public Image healthBar;
    private int actionselected = 0;
    public float originalHealthBarWidth;

    [Header("Stats")]
    public int maxhealth = 100;
    public int currentHP = 100;
    public int numberofHPrecovery = 20;
    public int minBaseDamage = 8;
    public int maxBaseDamage = 15;
    public int critChance = 2;
    public float critDamage = 1.5f;
    private float originalCritDamage;
    private int originalCritChance;
    private int originalspeed;
    public int speed = 10;
    public CombatController combatController;

    [Header("Effect")]
    public bool Windbuff = false;
    private bool isParalyzed = false;
    private bool isBurned = false;
    private bool isPoisoned = false;
    private bool isSilenced = false;
    private bool isFrozen = false;
    private float originalSpeed;
    private float originalDamage;
    private int frozenTurns = 0;
    private int maxFrozenTurns = 4;
    private int windBuffTurns;

    [Header("Pet")]
    public PetController petController;

    private void Start()
    {
        originalHealthBarWidth = healthBar.GetComponent<RectTransform>().rect.width;
        combatController = FindObjectOfType<CombatController>();
    }

    public void selectAction(int action)
    {
        actionselected = action;
        ExecuteAction();
    }

    public void Attack(GameObject enemy)
    {
        if (IsActionBlocked())
        {
            Debug.Log("Player action not performed due to paralysis!");
            combatController.TogglePlayerTurn();
            return;
        }

        if (IsActionBlocked())
        {
            Debug.Log("Player action not performed due to freeze!");
            combatController.TogglePlayerTurn();
            return;
        }

        int crit = Random.Range(0, 100);
        if (crit <= critChance)
        {
            minBaseDamage = Mathf.RoundToInt(minBaseDamage * critDamage);
            maxBaseDamage = Mathf.RoundToInt(maxBaseDamage * critDamage);
        }
        int dmg = Random.Range(minBaseDamage, maxBaseDamage);
        Debug.Log("Player damage: " + dmg);
        enemy.GetComponent<EnemyController>().TakeDamage(dmg);
        StartCoroutine(UpdateHealthBarDelayed(enemy));
    }

    private IEnumerator UpdateHealthBarDelayed(GameObject enemy)
    {
        yield return new WaitForSeconds(0.5f);

        enemy.GetComponent<EnemyController>().UpdateHealthBar();
    }

    public void ExecuteAction()
    {
        Debug.Log("ExecuteAction called.");
        if (IsActionBlocked())
        {
            Debug.Log("Player action not performed due to paralysis!");
            combatController.TogglePlayerTurn();
            return;
        }

        if (IsSilenced() && (actionselected == 1 || actionselected == 2))
        {
            Debug.Log("Player cannot perform this action due to Silence!");
            combatController.TogglePlayerTurn();
            return;
        }

        if (IsActionBlocked())
        {
            Debug.Log("Player action not performed due to freeze!");
            combatController.TogglePlayerTurn();
            return;
        }

        else
        {
            if (actionselected == 0)
            {
                Debug.Log("Player attacks");
                GameObject targetEnemy = combatController.enemies.First(e => e.gameObject.activeSelf).gameObject;
                Attack(targetEnemy);
            }
            else if (actionselected == 1)
            {
                Heal();
            }
            else if (actionselected == 2)
            {
                AttemptEscape();
            }
            else if (actionselected == 3) 
            {
                petController.UseAbility(this);
                combatController.ActionPanel.SetActive(false);
            }
            else
            {
                Debug.Log("You have not selected any action!");
            }
            combatController.TogglePlayerTurn();
        }
    }

    public void TakeDamage(int dmgTaken)
    {
        currentHP -= dmgTaken;
        if (currentHP < 0) currentHP = 0;
        float healthPercentage = (float)currentHP / maxhealth;
        float newWidth = healthPercentage * originalHealthBarWidth;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(
            newWidth,
            healthBar.GetComponent<RectTransform>().sizeDelta.y
        );

        CheckDeath();
    }

    public void Heal()
    {
        Debug.Log("Heal method called");
        currentHP += numberofHPrecovery;
        if (currentHP > maxhealth)
            currentHP = maxhealth;

        float healthPercentage = (float)currentHP / maxhealth;
        float newWidth = healthPercentage * originalHealthBarWidth;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, healthBar.GetComponent<RectTransform>().sizeDelta.y);

        FindObjectOfType<CombatController>().TogglePlayerTurn();
    }

    public void CheckDeath()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
            Debug.Log(playername + " has been defeated! Game Over!");
            FindObjectOfType<CombatController>().GameOver(false);
        }
    }

    public void AttemptEscape()
    {
        int escapeChance = Random.Range(0, 100);
        int requiredChance = 50;

        CombatController combatController = FindObjectOfType<CombatController>();

        if (escapeChance < requiredChance)
        {
            Debug.Log("Escape failed! Enemy turn begins.");
            combatController.combatState.text = "Escape failed! Enemy turn begins.";
            combatController.TogglePlayerTurn();
        }
        else
        {
            Debug.Log("Escape successful! Loading new scene...");
            combatController.combatState.text = "Escape successful!";
            Invoke("LoadEscapeScene", 1f);
        }
    }

    private void LoadEscapeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ApplyParalysis()
    {
        if (isPoisoned || isParalyzed || isBurned || isSilenced) return;

        Debug.Log($"{playername} subject to paralysis!");
        isParalyzed = true;
        originalSpeed = speed;
        speed = Mathf.RoundToInt(speed * 0.5f);
    }

    public bool IsActionBlocked()
    {
        if (isFrozen)
        {
            Debug.Log($"{playername} is frozen and cannot act!");
            return true;
        }

        if (isParalyzed && Random.Range(0, 100) < 20)
        {
            Debug.Log($"{playername}'s Your action is blocked!");
            return true;
        }
        return false;
    }

    public void ApplyBurn()
    {
        if (isPoisoned || isParalyzed || isBurned || isSilenced) return;

        Debug.Log($"{playername} is burned!");
        isBurned = true;

        originalDamage = (minBaseDamage + maxBaseDamage) / 2;
        minBaseDamage = Mathf.RoundToInt(minBaseDamage * 0.5f);
        maxBaseDamage = Mathf.RoundToInt(maxBaseDamage * 0.5f);

    }

    public void InflictBurnDamage()
    {
        if (isBurned)
        {
            currentHP -= 2;
            Debug.Log($"{playername} takes 2 burn damage due to Burn effect!");
            CheckDeath();
        }
    }

    public void ApplyPoison()
    {
        if (isPoisoned || isParalyzed || isBurned || isSilenced) return;

        Debug.Log($"{playername} is poisoned!");
        isPoisoned = true;
    }

    public void InflictPoisonDamage()
    {
        if (isPoisoned)
        {
            currentHP -= 10;
            Debug.Log($"{playername} takes 10 poison damage due to Poison effect!");
            CheckDeath();
        }
    }

    public void ApplySilence()
    {
        if (isSilenced || isParalyzed || isBurned || isPoisoned) return;

        Debug.Log($"{playername} is silenced!");
        isSilenced = true;
    }

    public bool IsSilenced()
    {
        return isSilenced;
    }

    public void ApplyFreeze()
    {
        if (isFrozen) return;

        Debug.Log($"{playername} is frozen!");
        isFrozen = true;
        frozenTurns = Random.Range(1, maxFrozenTurns + 1);
    }

    public void EndFreezeTurn()
    {
        if (isFrozen)
        {
            frozenTurns--;

            if (frozenTurns <= 0)
            {
                isFrozen = false;
                Debug.Log($"{playername} is no longer frozen!");
            }
        }
    }

    public void RemoveEffects()
    {
        if (isPoisoned) Debug.Log($"{playername} is no longer poisoned!");
        if (isSilenced) Debug.Log($"{playername} is no longer silenced!");
        if (isBurned) Debug.Log($"{playername} is no longer burned!");
        if (isParalyzed) Debug.Log($"{playername} is no longer paralyzed!");

        isPoisoned = false;
        isSilenced = false;
        isBurned = false;
        isParalyzed = false;

        speed = Mathf.RoundToInt(originalSpeed);
        minBaseDamage = Mathf.RoundToInt(originalDamage * 0.5f);
        maxBaseDamage = Mathf.RoundToInt(originalDamage * 0.5f);
    }

    public void ApplyEndTurnEffects()
    {
        if (isFrozen)
        {
            EndFreezeTurn();
        }

        if (isBurned) InflictBurnDamage();
        if (isPoisoned) InflictPoisonDamage();
        UpdateWindBuff();
    }

    public void ApplyPetRegeneration()
    {
        if (petController != null && petController is PetHealer healerPet)
        {
            healerPet.ApplyRegeneration(this); 
        }
    }

    public void EndTurnEffects()
    {
        if (petController != null)
        {
            petController.EndTurn();
        }
    }

    public void ApplyWindBuff(int duration, float speedMultiplier, int critChanceIncrease, float critDamageMultiplier)
    {

        if (!Windbuff)
        {
            Windbuff = true;
            windBuffTurns = duration;

            originalSpeed = speed;
            originalCritChance = critChance;
            originalCritDamage = critDamage;

            speed = Mathf.RoundToInt(speed * speedMultiplier);
            critChance += critChanceIncrease;
            critDamage *= critDamageMultiplier;
            Debug.Log($"Wind buff applied: +{speedMultiplier}x speed, +{critChanceIncrease}% crit chance, +{critDamageMultiplier}x crit damage for {duration} turns.");

        }
    }


    public void UpdateWindBuff()
    {
        if (windBuffTurns > 0)
        {
            windBuffTurns--;

            if (windBuffTurns <= 0)
            {
                Windbuff = false;

                speed = Mathf.RoundToInt(originalSpeed);
                critChance = originalCritChance;
                critDamage = originalCritDamage;
                Debug.Log($"{playername} is no longer have Wind Buff!");
            }
        }
    }
}
