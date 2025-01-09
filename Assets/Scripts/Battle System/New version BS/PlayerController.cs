using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Reference to PlayerStats")]
    public PlayerStats playerStats;
    public bool UpdateStats = true;
    private InventoryManager inventoryManager;

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
    public Attribute weaponAttribute;

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

    public GameObject Burn;
    public GameObject Paralyze;
    public GameObject Poisen;
    public GameObject Frozen;
    public GameObject Silence;

    [Header("Pet")]
    public PetController petController;

    public GameObject targetEnemy;
    private Animator anim;

    [Header("Pets anim")]
    public GameObject PetHealer;
    private Animator PethealerAnim;

    public GameObject FreezePet;
    private Animator FreezePetAnim;

    public GameObject PetFire;
    private Animator PetFireAnim;

    public GameObject LightningPet;
    private Animator LightningPetAnim;

    public GameObject ElectricWavePet;
    private Animator ElectricWavePetAnim;

    public GameObject WindBuffPet;
    private Animator WindBuffPetAnim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        originalHealthBarWidth = healthBar.GetComponent<RectTransform>().rect.width;
        Debug.Log("Original health bar width: " + originalHealthBarWidth);
        if (UpdateStats  == true)
        {
            InitializeStats();
        }
        combatController = FindObjectOfType<CombatController>();
        Debug.Log("After update health bar width: " + originalHealthBarWidth);
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();

        PethealerAnim = PetHealer.GetComponent<Animator>();
        FreezePetAnim = FreezePet.GetComponent<Animator>();
        PetFireAnim = PetFire.GetComponent<Animator>();
        LightningPetAnim = LightningPet.GetComponent<Animator>();
        ElectricWavePetAnim = ElectricWavePet.GetComponent<Animator>();
        WindBuffPetAnim = WindBuffPet.GetComponent<Animator>();

        if (petController != null)
        {
            if (petController is PetHealer)
            {
                PetHealer.SetActive(true);
                FreezePet.SetActive(false);
                PetFire.SetActive(false);
                LightningPet.SetActive(false);
                ElectricWavePet.SetActive(false);
                WindBuffPet.SetActive(false);
            }

            else if (petController is FreezePet)
            {
                FreezePet.SetActive(true);
                PetHealer.SetActive(false);
                PetFire.SetActive(false);
                LightningPet.SetActive(false);
                ElectricWavePet.SetActive(false);
                WindBuffPet.SetActive(false);
            }

            else if (petController is PetFireUser)
            {
                PetFire.SetActive(true);
                FreezePet.SetActive(false);
                PetHealer.SetActive(false);
                LightningPet.SetActive(false);
                ElectricWavePet.SetActive(false);
                WindBuffPet.SetActive(false);
            }
            else if (petController is LightningPet)
            {
                LightningPet.SetActive(true);
                FreezePet.SetActive(false);
                PetHealer.SetActive(false);
                PetFire.SetActive(false);
                ElectricWavePet.SetActive(false);
                WindBuffPet.SetActive(false);
            }
            else if (petController is ElectricWavePet)
            {
                ElectricWavePet.SetActive(true);
                FreezePet.SetActive(false);
                PetHealer.SetActive(false);
                PetFire.SetActive(false);
                LightningPet.SetActive(false);
                WindBuffPet.SetActive(false);
            }
            else if (petController is PetWind)
            {
                WindBuffPet.SetActive(true);
                FreezePet.SetActive(false);
                PetHealer.SetActive(false);
                PetFire.SetActive(false);
                LightningPet.SetActive(false);
                ElectricWavePet.SetActive(false);
            }
        }
        else{
            WindBuffPet.SetActive(false);
            FreezePet.SetActive(false);
            PetHealer.SetActive(false);
            PetFire.SetActive(false);
            LightningPet.SetActive(false);
            ElectricWavePet.SetActive(false);
        }
    }

    private void Update()
    {
    }
    public void selectAction(int action)
    {
        actionselected = action;
        ExecuteAction();
    }

    public void Attack(GameObject enemy)
    {
        anim.SetTrigger("Attack");
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

        float damageMultiplier = CalculateAttributeDamageMultiplier(weaponAttribute, enemy.GetComponent<EnemyController>().enemyAttribute);

        int crit = Random.Range(0, 100);
        if (crit <= critChance)
        {
            minBaseDamage = Mathf.RoundToInt(minBaseDamage * critDamage);
            maxBaseDamage = Mathf.RoundToInt(maxBaseDamage * critDamage);
        }
        int dmg = Mathf.RoundToInt(Random.Range(minBaseDamage, maxBaseDamage) * damageMultiplier);
        combatController.PlayerMessage.text = $"{playername} attacks the {enemy.name} and deals {dmg} damage";
        Debug.Log("Player damage: " + dmg);
        enemy.GetComponent<EnemyController>().TakeDamage(dmg);
        StartCoroutine(UpdateHealthBarDelayed(enemy));

    }

    private IEnumerator UpdateHealthBarDelayed(GameObject enemy)
    {
        yield return new WaitForSeconds(0.5f);

        enemy.GetComponent<EnemyController>().UpdateHealthBar();
    }

    public void SelectTarget(GameObject enemy)
    {
        targetEnemy = enemy;

        Debug.Log("Target selected: " + enemy.name);
    }

    public void ExecuteAction()
    {
        Debug.Log("ExecuteAction called.");
        if (IsActionBlocked() && isParalyzed == true)
        {
            Debug.Log("Player action not performed due to paralysis!");
            combatController.TogglePlayerTurn();
            return;
        }

        else if (IsSilenced() && (actionselected == 1 || actionselected == 2))
        {
            Debug.Log("Player cannot perform this action due to Silence!");
            combatController.TogglePlayerTurn();
            return;
        }

        else if (IsActionBlocked() && isFrozen == true)
        {
            Debug.Log("Player action not performed due to freeze!");
            combatController.TogglePlayerTurn();
            return;
        }

        else
        {
            if (actionselected == 0)
            {
                if (targetEnemy != null)
                {
                    Debug.Log("Player attacks target: " + targetEnemy.name);
                    Attack(targetEnemy);
                }
            }
            else if (actionselected == 1)
            {
                Heal(numberofHPrecovery);
            }
            else if (actionselected == 2)
            {
                AttemptEscape();
            }
            else if (actionselected == 3)
            {
                if (petController == null) combatController.PlayerMessage.text = $"{playername} don't have a pet";
                if (petController.IsReadyToUse())
                {
                    if (petController is PetHealer)
                    {
                        PethealerAnim.SetTrigger("Use ability");
                    }
                    else if (petController is FreezePet)
                    {
                        FreezePetAnim.SetTrigger("Use ability");
                    }
                    else if (petController is PetFireUser)
                    {
                        PetFireAnim.SetTrigger("Use ability");
                    }
                    else if (petController is LightningPet)
                    {
                        LightningPetAnim.SetTrigger("Use ability");
                    }
                    else if (petController is ElectricWavePet)
                    {
                        ElectricWavePetAnim.SetTrigger("Use ability");
                    }
                    else if (petController is PetWind)
                    {
                        WindBuffPetAnim.SetTrigger("Use ability");
                    }

                }
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

    public void Heal(int HPrecovery)
    {
        Debug.Log("Heal method called");
        currentHP += HPrecovery;
        if (currentHP > maxhealth)
            currentHP = maxhealth;

        float healthPercentage = (float)currentHP / maxhealth;
        float newWidth = healthPercentage * originalHealthBarWidth;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, healthBar.GetComponent<RectTransform>().sizeDelta.y);
        combatController.PlayerMessageObject.SetActive(true);
        CloseInventoryInBattle();
        FindObjectOfType<CombatController>().TogglePlayerTurn();
    }

    public void CheckDeath()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
            combatController.PlayerMessage.text = $"{playername} has been defeated! Game Over!";
            Debug.Log(playername + " has been defeated! Game Over!");
            ResetHealthBar();
            FindObjectOfType<CombatController>().GameOver(false);
        }
    }

    public void CloseInventoryInBattle()
    {
        inventoryManager.InventoryMenu.SetActive(false);
        inventoryManager.EquipmentMenu.SetActive(false);
        inventoryManager.PetMenu.SetActive(false);
        inventoryManager.InventoryDescription.SetActive(false);
        inventoryManager.InventoryNavigation.SetActive(false);
        inventoryManager.StatPanel.SetActive(false);
        inventoryManager.PlayerEquipmentPanel.SetActive(false);

        GameObject.Find("CloseInv").SetActive(false);
    }

    public void AttemptEscape()
    {
        int escapeChance = Random.Range(0, 100);
        int requiredChance = 50;

        CombatController combatController = FindObjectOfType<CombatController>();

        if (escapeChance < requiredChance)
        {
            combatController.PlayerMessage.text = "Escape failed! Enemy turn begins.";
            Debug.Log("Escape failed! Enemy turn begins.");
        }
        else
        {
            Debug.Log("Escape successful! Loading new scene...");
            combatController.PlayerMessage.text = "Escape successful!";
            Invoke("LoadEscapeScene", 1f);
        }
    }
    public void TeleportationScroll()
    {
        combatController.PlayerMessageObject.SetActive(true);
        CloseInventoryInBattle();
        Debug.Log("Escape successful! Loading new scene...");
        combatController.PlayerMessage.text = "Escape successful!";
        Invoke("LoadEscapeScene", 1f);
    }

    private void LoadEscapeScene()
    {
        ResetHealthBar();
        SceneManager.LoadScene("LvlMenuTower1");
    }

    public void ApplyParalysis()
    {
        if (isPoisoned || isParalyzed || isBurned || isSilenced) return;

        combatController.PlayerMessage.text = $"{playername} subject to paralysis!";
        Debug.Log($"{playername} subject to paralysis!");
        isParalyzed = true;
        Paralyze.SetActive(true);
        originalSpeed = speed;
        speed = Mathf.RoundToInt(speed * 0.5f);
    }

    public bool IsActionBlocked()
    {
        if (isFrozen)
        {
            combatController.PlayerMessage.text = $"{playername} is frozen and cannot act!";
            Debug.Log($"{playername} is frozen and cannot act!");
            return true;
        }

        if (isParalyzed && Random.Range(0, 100) < 20)
        {
            combatController.PlayerMessage.text = $"{playername}'s Your action is blocked!";
            Debug.Log($"{playername}'s Your action is blocked!");
            return true;
        }
        return false;
    }

    public void ApplyBurn()
    {
        if (isPoisoned || isParalyzed || isBurned || isSilenced) return;

        combatController.PlayerMessage.text = $"{playername} is burned!";
        Debug.Log($"{playername} is burned!");
        isBurned = true;
        Burn.SetActive(true);
        originalDamage = (minBaseDamage + maxBaseDamage) / 2;
        minBaseDamage = Mathf.RoundToInt(minBaseDamage * 0.5f);
        maxBaseDamage = Mathf.RoundToInt(maxBaseDamage * 0.5f);

    }

    public void InflictBurnDamage()
    {
        if (isBurned)
        {
            currentHP -= 1;
            combatController.PlayerMessage.text = $"{playername} takes 1 burn damage due to Burn effect!";
            Debug.Log($"{playername} takes 1 burn damage due to Burn effect!");
            CheckDeath();
        }
    }

    public void ApplyPoison()
    {
        if (isPoisoned || isParalyzed || isBurned || isSilenced) return;

        combatController.PlayerMessage.text = $"{playername} is poisoned!";
        Debug.Log($"{playername} is poisoned!");
        isPoisoned = true;
        Poisen.SetActive(true);
    }

    public void InflictPoisonDamage()
    {
        if (isPoisoned)
        {
            currentHP -= 3;
            combatController.PlayerMessage.text = $"{playername} takes 3 poison damage due to Poison effect!";
            Debug.Log($"{playername} takes 3poison damage due to Poison effect!");
            CheckDeath();
        }
    }

    public void ApplySilence()
    {
        if (isSilenced || isParalyzed || isBurned || isPoisoned) return;

        combatController.PlayerMessage.text = $"{playername} is silenced!";
        Debug.Log($"{playername} is silenced!");
        isSilenced = true;
        Silence.SetActive(true);
    }

    public bool IsSilenced()
    {
        return isSilenced;
    }

    public void ApplyFreeze()
    {
        if (isFrozen) return;

        combatController.PlayerMessage.text = $"{playername} is frozen!";
        Debug.Log($"{playername} is frozen!");
        isFrozen = true;
        Frozen.SetActive(true);
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
                Frozen.SetActive(false);
                combatController.PlayerMessage.text = $"{playername} is no longer frozen!";
                Debug.Log($"{playername} is no longer frozen!");
            }
        }
    }

    public void RemoveEffects()
    {
        if (isPoisoned)
        {
            combatController.PlayerMessage.text = $"{playername} is no longer poisoned!";
            Debug.Log($"{playername} is no longer poisoned!");
        }
        if (isSilenced)
        {
            combatController.PlayerMessage.text = $"{playername} is no longer silenced!";
            Debug.Log($"{playername} is no longer silenced!");
        }
        if (isBurned) 
        {
            combatController.PlayerMessage.text = $"{playername} is no longer burned!";
            Debug.Log($"{playername} is no longer burned!");
        }
        if (isParalyzed) 
        {
            combatController.PlayerMessage.text = $"{playername} is no longer paralyzed!";
            Debug.Log($"{playername} is no longer paralyzed!");
        }

        isPoisoned = false;
        isSilenced = false;
        isBurned = false;
        isParalyzed = false;

        Poisen.SetActive(false);
        Silence.SetActive(false);
        Burn.SetActive(false);
        Paralyze.SetActive(false);

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
            combatController.PlayerMessage.text = $"Wind buff applied: +{speedMultiplier}x speed, +{critChanceIncrease}% crit chance, +{critDamageMultiplier}x crit damage for {duration} turns.";
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
                combatController.PlayerMessage.text = $"{playername} is no longer have Wind Buff!";
                Debug.Log($"{playername} is no longer have Wind Buff!");
                
            }
        }
    }

    public void InitializeStats()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }

        if (playerStats != null)
        {
            float maincurrentHP = currentHP;
            int takedamage = maxhealth - currentHP;
            maxhealth = playerStats.hp;
            currentHP = maxhealth - takedamage;
            if (currentHP > maincurrentHP && currentHP < maincurrentHP)
            {
                UpdateHealthBar();

            }
            speed = playerStats.speed;
            minBaseDamage = playerStats.attack - Mathf.CeilToInt(playerStats.attack * 0.1f);
            maxBaseDamage = playerStats.attack;
            critChance = playerStats.critChance;
            critDamage = playerStats.critDmg;
            weaponAttribute = playerStats.attribute;
            petController = playerStats.petprefab;
            if (petController != null)
            {
                if(petController is PetHealer)
                {
                    PetHealer.SetActive(true);
                    FreezePet.SetActive(false);
                    PetFire.SetActive(false);
                    LightningPet.SetActive(false);
                    ElectricWavePet.SetActive(false);
                    WindBuffPet.SetActive(false);
                }

                else if (petController is FreezePet)
                {
                    FreezePet.SetActive(true);
                    PetHealer.SetActive(false);
                    PetFire.SetActive(false);
                    LightningPet.SetActive(false);
                    ElectricWavePet.SetActive(false);
                    WindBuffPet.SetActive(false);
                }

                else if (petController is PetFireUser)
                {
                    PetFire.SetActive(true);
                    FreezePet.SetActive(false);
                    PetHealer.SetActive(false);
                    LightningPet.SetActive(false);
                    ElectricWavePet.SetActive(false);
                    WindBuffPet.SetActive(false);
                }
                else if (petController is LightningPet)
                {
                    LightningPet.SetActive(true);
                    FreezePet.SetActive(false);
                    PetHealer.SetActive(false);
                    PetFire.SetActive(false);
                    ElectricWavePet.SetActive(false);
                    WindBuffPet.SetActive(false);
                }
                else if (petController is ElectricWavePet)
                {
                    ElectricWavePet.SetActive(true);
                    FreezePet.SetActive(false);
                    PetHealer.SetActive(false);
                    PetFire.SetActive(false);
                    LightningPet.SetActive(false);
                    WindBuffPet.SetActive(false);
                }
                else if (petController is PetWind)
                {
                    WindBuffPet.SetActive(true);
                    FreezePet.SetActive(false);
                    PetHealer.SetActive(false);
                    PetFire.SetActive(false);
                    LightningPet.SetActive(false);
                    ElectricWavePet.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerStats не найден, используйте значения по умолчанию.");
        }
        UpdateStats = false;
    }
       private void UpdateHealthBar()
    {
        float healthPercentage = (float)currentHP / maxhealth;
        float newWidth = healthPercentage * originalHealthBarWidth;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(
            newWidth,
            healthBar.GetComponent<RectTransform>().sizeDelta.y
        );
    }

    private static readonly Dictionary<Attribute, Attribute> NeutralAttributes = new Dictionary<Attribute, Attribute>
{
    { Attribute.Chaos, Attribute.Darkness },
    { Attribute.Darkness, Attribute.Chaos },
    { Attribute.Spirit, Attribute.Order },
    { Attribute.Order, Attribute.Spirit }
};

    public float CalculateAttributeDamageMultiplier(Attribute attacker, Attribute defender)
    {
        if (attacker == defender)
        {
            return 1f;
        }

        if ((attacker == Attribute.Spirit && defender == Attribute.Darkness) ||
            (attacker == Attribute.Darkness && defender == Attribute.Order) ||
            (attacker == Attribute.Order && defender == Attribute.Chaos) ||
            (attacker == Attribute.Chaos && defender == Attribute.Spirit))
        {
            return 2f;
        }
        else if ((defender == Attribute.Spirit && attacker == Attribute.Darkness) ||
                 (defender == Attribute.Darkness && attacker == Attribute.Order) ||
                 (defender == Attribute.Order && attacker == Attribute.Chaos) ||
                 (defender == Attribute.Chaos && attacker == Attribute.Spirit))
        {
            return 0.5f;
        }
        else if (NeutralAttributes.TryGetValue(attacker, out Attribute neutral) && neutral == defender)
        {
            return 1f; 
        }
        else
        {
            return 1f;
        }

    }
    public void RemoveEffect(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.Paralysis:
                if (isParalyzed)
                {
                    isParalyzed = false;
                    Paralyze.SetActive(false);
                    speed = Mathf.RoundToInt(originalSpeed);
                    combatController.PlayerMessage.text = $"{playername} is no longer paralyzed!";
                    Debug.Log($"{playername} больше не парализован!");
                }
                break;

            case EffectType.Burn:
                if (isBurned)
                {
                    isBurned = false;
                    Burn.SetActive(false);
                    minBaseDamage = Mathf.RoundToInt(originalDamage * 2);
                    maxBaseDamage = Mathf.RoundToInt(originalDamage * 2);
                    combatController.PlayerMessage.text = $"{playername} is no longer burned!";
                    Debug.Log($"{playername} is no longer burned!");
                }
                break;

            case EffectType.Freeze:
                if (isFrozen)
                {
                    isFrozen = false;
                    Frozen.SetActive(false);
                    frozenTurns = 0;
                    combatController.PlayerMessage.text = $"{playername} is no longer frozen!";
                    Debug.Log($"{playername} is no longer frozen!");
                }
                break;

            case EffectType.Poison:
                if (isPoisoned)
                {
                    isPoisoned = false;
                    Poisen.SetActive(false);
                    combatController.PlayerMessage.text = $"{playername} is no longer poisoned!";
                    Debug.Log($"{playername} is no longer poisoned!");
                }
                break;

            case EffectType.Silence:
                if (isSilenced)
                {
                    isSilenced = false;
                    Silence.SetActive(false);
                    combatController.PlayerMessage.text = $"{playername} is no longer silenced!";
                    Debug.Log($"{playername} is no longer silenced!");
                }
                break;

            default:
                combatController.PlayerMessage.text = "BEBE effect!!";

                Debug.LogWarning("BEBE effect!!");
                break;
        }
        combatController.PlayerMessageObject.SetActive(true);
        CloseInventoryInBattle();
        FindObjectOfType<CombatController>().TogglePlayerTurn();
    }

    public void ResetHealthBar()
    {
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(
            originalHealthBarWidth,
            healthBar.GetComponent<RectTransform>().sizeDelta.y
        );
        currentHP = maxhealth; 
    }

}
