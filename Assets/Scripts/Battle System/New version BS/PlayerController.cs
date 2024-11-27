using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Attribute")]
    public string playername;
    public int maxhealth = 100;
    public Image healthBar;
    public int currentHP = 100;
    public int numberofHPrecovery = 20;
    public int minBaseDamage = 8;
    public int maxBaseDamage = 15;
    public int critChance = 2;
    private int actionselected = 0;
    private float originalHealthBarWidth;

    private void Start()
    {
        originalHealthBarWidth = healthBar.GetComponent<RectTransform>().rect.width;
    }
    public void selectAction(int action)
    {
        actionselected = action;
    }

    public void Attack(GameObject enemy)
    {
        int crit = Random.Range(0, 100);
        if (actionselected == 0)
        {
            if (crit <= critChance)
            {
                float min = minBaseDamage * 1.5f;
                float max = minBaseDamage * 1.5f;
                minBaseDamage = Mathf.RoundToInt(min);
                maxBaseDamage = Mathf.RoundToInt(max);
            }
            int dmg = Random.Range(minBaseDamage, maxBaseDamage);
            Debug.Log("Player damage: " + dmg);

            enemy.GetComponent<EnemyController>().TakeDamage(dmg);
        }
        else if (actionselected == 1)
        {
            Heal();
        }
        else
        {
            Debug.Log("You have not selected any action!");
        }
    }



    public void TakeDamage(int dmgTaken)
    {
        currentHP -= dmgTaken;
        if (currentHP < 0) currentHP = 0;
        float healthPercentage = (float)currentHP / maxhealth;
        float maxBarWidth = healthBar.GetComponent<RectTransform>().rect.width;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(
            maxBarWidth * healthPercentage,
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
}
