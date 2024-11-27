using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    [Header("References")]
    public GameObject CanvasPlayer;
    public GameObject EmptyPanel;
    public GameObject EnemyPanel;
    public GameObject ActionPanel;
    public EnemyController[] enemies;
    public Text combatState;
    public Button[] enemyAttackButtons;

    [Header("Attribute")]
    bool isPlayerTurn = true;
    private int enemyIndex = -1;

    void Start()
    {
        combatState.text = "PLAYER TURN";
        EmptyPanel.SetActive(false);
        CanvasPlayer.GetComponent<GraphicRaycaster>().enabled = true;

    }

    void Update()
    {
    }

    public void TogglePlayerTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
        {
            ActionPanel.SetActive(true);
            EnemyPanel.SetActive(false);
            combatState.text = "PLAYER TURN";
            EmptyPanel.SetActive(false);
            CanvasPlayer.GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            combatState.text = "ENEMY TURN";
            EmptyPanel.SetActive(true);
            CanvasPlayer.GetComponent<GraphicRaycaster>().enabled = false;
            EnemyTurn();
        }
    }

    public void EnemyTurn()
    {
        enemyIndex++;

        while (enemyIndex < enemies.Length && !enemies[enemyIndex].gameObject.activeSelf)
        {
            enemyIndex++;
        }

        if (enemyIndex >= enemies.Length)
        {
            enemyIndex = -1;
            TogglePlayerTurn();
            return;
        }
        combatState.text = enemies[enemyIndex].EnemyName + " is preparing for attack";
        StartCoroutine(WaitForAttack(enemies[enemyIndex]));
    }

    public void NextEnemy()
    {
        EnemyTurn();
    }



    IEnumerator WaitForAttack(EnemyController ec)
    {
        if (!ec.gameObject.activeSelf)
        {
            NextEnemy();
            yield break;
        }

        combatState.text = enemies[enemyIndex].EnemyName + " is preparing to attack";
        yield return new WaitForSeconds(3f);
        combatState.text = "";
        ec.Attack();

        yield return new WaitForSeconds(1f);
        NextEnemy();
    }

    public void ApplyDamage()
    {
        enemies[enemyIndex].ApplyDamage();
    }

    public void EnemyDefeated()
    {
        bool allDefeated = true;

        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                allDefeated = false;
                break;
            }
        }

        if (allDefeated)
        {
            GameOver(true);
        }
    }

    public void GameOver(bool victory)
    {
        EmptyPanel.SetActive(true);
        CanvasPlayer.GetComponent<GraphicRaycaster>().enabled = false;

        if (victory)
        {
            combatState.text = "Victory! All enemies defeated!";
            Debug.Log("You won the battle!");
        }
        else
        {
            combatState.text = "Defeat! Game Over!";
            Debug.Log("You lost the battle!");
        }


        ActionPanel.SetActive(false);
        EnemyPanel.SetActive(false);


        StopAllCoroutines();
    }

    public void OnEnemyDefeated(EnemyController defeatedEnemy)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == defeatedEnemy)
            {
                if (enemyAttackButtons[i] != null)
                {
                    enemyAttackButtons[i].gameObject.SetActive(false);
                }
                break;
            }
        }

        EnemyDefeated();
    }

}
