using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject VictoryMenu;
    public GameObject DefeatMenu;
    public Save save;
    public Text GameMessage;
    public Text PlayerMessage;
    private PetController petController;
    public GameObject PlayerMessageObject;


    [Header("Attribute")]
    bool isPlayerTurn = true;
    private int enemyIndex = -1;
    public string nextLevel = "Inventory";
    private PlayerController playerController;

   


    public int money;
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerMessageObject = GameObject.Find("Player Message");
        save = GameObject.Find("InventoryCanvas").GetComponent<Save>();
        combatState.text = "PLAYER TURN";
        EmptyPanel.SetActive(false);
        CanvasPlayer.GetComponent<GraphicRaycaster>().enabled = true;

        VictoryMenu.SetActive(false);
        DefeatMenu.SetActive(false);
        petController = playerController.petController;
    }

    private void Update()
    {
        petController = playerController.petController;
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
            StartCoroutine(ResolveTurnOrder());
        }
    }

    public IEnumerator ResolveTurnOrder()
    {
        List<object> turnOrder = new List<object>(enemies.Length + 1);
        turnOrder.AddRange(enemies);
        turnOrder.Add(playerController);

        turnOrder = turnOrder.OrderByDescending(e => e is EnemyController ? ((EnemyController)e).speed : playerController.speed).ToList();
        Debug.Log("Starting ResolveTurnOrder");

        bool enemyActionsCompleted = false;
        foreach (var entity in turnOrder)
        {
            if (entity is EnemyController enemy)
            {
                if (enemy.isFrozen)
                {
                    GameMessage.text = $"{enemy.EnemyName} is frozen and cannot act this turn.";
                    Debug.Log($"{enemy.EnemyName} is frozen and cannot act this turn.");
                    enemy.freezeTurns--;
                    if (enemy.freezeTurns <= 0)
                    {
                        enemy.isFrozen = false;
                        GameMessage.text = $"{enemy.EnemyName} is no longer frozen.";
                        Debug.Log($"{enemy.EnemyName} is no longer frozen.");
                    }
                }
                else if (enemy.gameObject.activeSelf && enemy.CanAct())
                {
                    Debug.Log(enemy.EnemyName + " attacks");
                    enemy.Attack();
                    enemy.EndTurnEffects();
                    yield return new WaitForSeconds(3f);
                    yield return null;
                }
                else if (!enemy.CanAct())
                {
                    GameMessage.text = $"{enemy.EnemyName} skips the turn due to paralysis.";

                    Debug.Log(enemy.EnemyName + " skips the turn due to paralysis.");
                }

            }
            else if (entity is PlayerController player)
            {
                Debug.Log("Waiting for enemies to finish their turns before player takes action.");
                yield return new WaitUntil(() => enemyActionsCompleted);

                Debug.Log("Player takes action");
                ActionPanel.SetActive(true);
                CanvasPlayer.GetComponent<GraphicRaycaster>().enabled = true;
                yield return new WaitUntil(() => !isPlayerTurn);
                yield return null;
                ActionPanel.SetActive(false);
                CanvasPlayer.GetComponent<GraphicRaycaster>().enabled = false;
            }
            enemyActionsCompleted = true;
        }
        playerController.ApplyEndTurnEffects();
        playerController.ApplyPetRegeneration();
        TogglePlayerTurn();
        Debug.Log("ResolveTurnOrder completed");
        PlayerMessage.text = "Player takes action";
        petController.EndTurn();
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

        yield return new WaitForSeconds(3f);
        combatState.text = "";
        ec.Attack();
        yield return new WaitForSeconds(3f);
        if (ec.health <= 0)
        {
            ec.gameObject.SetActive(false);
        }
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
        playerController.RemoveEffects();
        EmptyPanel.SetActive(true);
        CanvasPlayer.GetComponent<GraphicRaycaster>().enabled = false;
        CanvasPlayer.SetActive(false);

        if (victory)
        {
            Debug.Log("You won the battle!");

            VictoryMenu.SetActive(true);
            save.SaveInventory();
            SaveProgress();
        }
        else
        {
            save.SaveInventory();
            DefeatMenu.SetActive(true);
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

    private void SaveProgress()
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetInt(currentLevel, 1);
        PlayerPrefs.Save();

        Debug.Log($"Progress saved for {currentLevel}");
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void ButtonLevelMenu()
    {
        SceneManager.LoadScene("LvlMenuTower1");
    }

}
