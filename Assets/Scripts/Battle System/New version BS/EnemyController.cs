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

    private void Start()
    {
        curentDamage = 0;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Attack()
    {
        int crit = Random.Range(0, 100);
        if (crit <= critChance)
        {
            minBaseDamage = Mathf.RoundToInt(minBaseDamage * critDamage);
            maxBaseDamage = Mathf.RoundToInt(maxBaseDamage * critDamage);
        }
        int dmg = Random.Range(minBaseDamage, maxBaseDamage);
        Debug.Log(EnemyName + " "+ dmg);

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
}
