using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BattleStatus { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPref;
    public GameObject playerPref;
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Text dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("Attribute")]
    public BattleStatus status;
    Unit playerUnit;
    Unit enemyUnit;

    private void Start()
    {
        status = BattleStatus.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGo = Instantiate(playerPref, playerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();

        GameObject enemyGo = Instantiate(enemyPref);
        enemyUnit = enemyGo.GetComponent<Unit>();

        dialogueText.text = "O this is a creppy " + enemyUnit.unitName;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(3);
        status = BattleStatus.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " use attack!";

        yield return new WaitForSeconds(2);

        if (isDead)
        {
            status = BattleStatus.WON;
            EndBattle();
        }
        else
        {
            status = BattleStatus.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    void EndBattle()
    {
        if (status == BattleStatus.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (status == BattleStatus.LOST)
        {
            dialogueText.text = "You Dead!";
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " use attack!";
        yield return new WaitForSeconds(1);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1);

        if (isDead)
        {
            status = BattleStatus.LOST;
            EndBattle();
        }
        else
        {
            status = BattleStatus.PLAYERTURN;
            PlayerTurn();
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose the action:";

    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = playerUnit.unitName + " use heal!";
        yield return new WaitForSeconds(2);

        status = BattleStatus.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (status != BattleStatus.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (status != BattleStatus.PLAYERTURN)
            return;
        StartCoroutine(PlayerHeal());
    }
}