using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Attribute")]
    public string EnemyName;
    public int maxhealth;
    public int health;
    public Image healthBar;
    public int minBaseDamage = 8;
    public int maxBaseDamage = 15;
    public int critChance = 2;
    private int curentDamage;
    private PlayerController playerController;

    private void Start()
    {
        curentDamage = 0;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {

    }
    public void Attack()
    {
        int crit = Random.Range(0, 100);
        if (crit <= critChance)
        {
            float min = minBaseDamage * 1.5f;
            float max = minBaseDamage * 1.5f;
            minBaseDamage = Mathf.RoundToInt(min);
            maxBaseDamage = Mathf.RoundToInt(max);
        }
        int dmg = Random.Range(minBaseDamage, maxBaseDamage);
        Debug.Log(dmg);

        curentDamage = 0;
        playerController.TakeDamage(dmg);
    }

    public void ApplyDamage()
    {
        playerController.TakeDamage(curentDamage);
    }

    public void TakeDamage(int dmgTaken)
    {
        health -= dmgTaken;
        if (health < 0) health = 0;

        float healthPercentage = (float)health / maxhealth;
        float maxBarWidth = healthBar.GetComponent<RectTransform>().rect.width;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(
            maxBarWidth * healthPercentage,
            healthBar.GetComponent<RectTransform>().sizeDelta.y
        );

        CheckDeath();
    }

    public void CheckDeath()
    {
        if (health <= 0)
        {
            health = 0;
            Debug.Log(EnemyName + " has been defeated!");
            gameObject.SetActive(false);
            FindObjectOfType<CombatController>().EnemyDefeated();
            FindObjectOfType<CombatController>().OnEnemyDefeated(this);
        }
    }
}
