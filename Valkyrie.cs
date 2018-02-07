using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Valkyrie : Characters
{
    [Header("GameObjects")]
    public GameObject paladin;
    public GameObject sage;
    public GameObject gameManager;
    [Header("Texts")]
    public Text healthText;
    public Text specialHitText;
    public Text increaseText;

    [Header("Special")]
    public GameObject timeBar;
    public GameObject spear;

    [Header("Magic")]
    public GameObject magic1;
    public GameObject magic2;
    public GameObject magic3;
    public GameObject magic4;

    [Header("Ailments")]
    public GameObject guardUp;

    private GameObject guardUpInstance;
    private GameObject magicInstance1;
    private GameObject magicInstance2;
    private GameObject magicInstance3;
    private GameObject magicInstance4;


    private TurnController turnController;
    private Animator animationController;

    private bool isGuardUp = false;
    private bool b_isPetrified = false;

    private int damageStart;
    private int damageIncrementor;
    private int chance;

    private GameObject enemy;
    private GameObject timeBarInstance;
    private Paladin paladinController;
    private Sage sageController;
    private Enemy enemyController;

    private bool specialExecute;
    private int specialHits;
    private float timeLeft;
    private Vector2 spearPosition;
    private Vector2 magicPosition;

    // Use this for initialization
    void Start()
    {

        animationController = GetComponent<Animator>();
        spriteImage = GetComponent<SpriteRenderer>();
        turnController = gameManager.GetComponent<TurnController>();
        paladinController = paladin.GetComponent<Paladin>();
        sageController = sage.GetComponent<Sage>();

        TR_petrified = transform.Find("V_Petrified");
        petrifiedImage = TR_petrified.GetComponentInChildren<SpriteRenderer>();

        TR_special = transform.Find("V_Special");
        specialImage = TR_special.GetComponentInChildren<SpriteRenderer>();

        TR_magic = transform.Find("V_Magic");
        magicImage = TR_magic.GetComponentInChildren<SpriteRenderer>();

        GO_guardUp = GameObject.FindWithTag("V_GuardUp");
        guardUpImage = GO_guardUp.GetComponent<SpriteRenderer>();

        TR_win = transform.Find("V_Win");
        winImage = TR_win.GetComponentInChildren<SpriteRenderer>();

        TR_wounded = transform.Find("V_Wounded");
        woundedImage = TR_wounded.GetComponentInChildren<SpriteRenderer>();

        TR_dead = transform.Find("V_Dead");
        deadImage = TR_dead.GetComponentInChildren<SpriteRenderer>();

        b_isDead = false;
        b_isWounded = false;

        //Derived Stats
        attackPower = STRENGTH * 10;    // 1000 avg damage
        magicPower = 0;             // No magic offense
        dodge = AGILITY * 2;        // 10% Chance of dodging
        defencePower = DEFENCE * 2;     //10% Defence
        special = 0;
        health = MAX_HEALTH;
        magic = MAX_MAGIC;

        AudioSource[] audios = GetComponents<AudioSource>();
        takeDamageSound = audios[0];
        attackSound = audios[1];
        magicSound = audios[2];

        specialExecute = false;

        UpdateStats();

        enemy = GameObject.FindWithTag("Enemy");
        enemyController = enemy.GetComponent<Enemy>();

    }

    // Update is called once per frame
    void Update()
    {
        if (specialExecute)
        {
            if(timeLeft < 0.0f)
            {
                specialExecute = false;
                Destroy(timeBarInstance);
                specialHitText.enabled = false;
                StartCoroutine(ShootSpears());

            }

            if (Input.GetKeyDown("space"))
            {
                specialHits++;
                UpdateHitText();
            }

            timeLeft -= 1.0f * Time.deltaTime;
        }
    }

    public IEnumerator OnAttack()
    {
        if (b_isWounded)
        {
            spriteImage.material.color = clear;
            woundedImage.enabled = false;
        }

        animationController.Play("Attack Animation");

        yield return new WaitForSeconds(0.75f);

        attackSound.Play();

        yield return new WaitForSeconds(0.75f);

        enemyController.StartCoroutine(enemyController.TakeDamage(Random.Range(attackPower - 500, attackPower + 500), false));

        if (b_isWounded)
        {
            spriteImage.material.color = transparent;
            woundedImage.enabled = true;
        }


        yield return new WaitForSeconds(2);

        turnController.UpdateTurn();
    }

    public void OnSpecial()
    {
        if (b_isWounded)
        {
            spriteImage.material.color = clear;
            woundedImage.enabled = false;
        }

        specialImage.enabled = true;
        spriteImage.material.color = transparent;
        specialHits = 0;
        UpdateHitText();
        timeLeft = 6.5f;
        timeBarInstance = Instantiate(timeBar);
        specialHitText.enabled = true;
        
        specialExecute = true;
    }

    public int GetSpecial()
    {
        return special;
    }

    public IEnumerator OnNotEnoughSpecial()
    {
        increaseText.enabled = true;
        increaseText.color = Color.white;

        increaseText.text = "NOT ENOUGH SP";
        yield return new WaitForSeconds(1);
        increaseText.enabled = false;
    }


    public IEnumerator OnMagic(int selection)
    {

        if (b_isWounded)
        {
            spriteImage.material.color = clear;
            woundedImage.enabled = false;
        }

        magicImage.enabled = true;
        spriteImage.material.color = transparent;
        magicSound.Play();

        switch (selection)
        {
            case 1: //PALADIN
                {
                    magicPosition = new Vector2(715, 310);
                }
                break;
            case 2: //VALKYRIE
                {
                    magicPosition = new Vector2(815, 418);
                }
                break;
            case 3: //SAGE
                {
                    magicPosition = new Vector2(815, 210);
                }
                break;
        }

        magicInstance1 = (GameObject)Instantiate(magic1, magicPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(magicInstance1);

        magicInstance2 = (GameObject)Instantiate(magic2, magicPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(magicInstance2);

        magicInstance3 = (GameObject)Instantiate(magic3, magicPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(magicInstance3);

        magicInstance4 = (GameObject)Instantiate(magic4, magicPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(magicInstance4);

        yield return new WaitForSeconds(1);

        switch (selection)
        {
            case 1: //PALADIN
                {
                    paladinController.StartCoroutine(paladinController.GuardUp());
                }
                break;
            case 2: //VALKYRIE
                {
                    StartCoroutine(GuardUp());
                }
                break;
            case 3: //SAGE
                {
                    sageController.StartCoroutine(sageController.GuardUp());
                }
                break;
        }

        magic -= spell1;
        UpdateStats();

        yield return new WaitForSeconds(1);

        magicImage.enabled = false;
        spriteImage.material.color = clear;

        if (b_isWounded)
        {
            spriteImage.material.color = transparent;
            woundedImage.enabled = true;
        }

        turnController.UpdateTurn();

    }

    private void UpdateStats()
    {
        healthText.text = "VALKYRIE     " + health + "/  " + MAX_HEALTH + "   " + magic + "/  " + MAX_MAGIC + "   " + special + "/  " + "5";
    }


    public IEnumerator AddToHealth(int healthIncrease)
    {
        increaseText.enabled = true;
        increaseText.color = healthGreen;

        health += healthIncrease;
        if (health > MAX_HEALTH)
            health = MAX_HEALTH;


        increaseText.text = " " + healthIncrease;

        UpdateStats();

        yield return new WaitForSeconds(1);

        if (health <= 0)
        {
            b_isDead = true;
            if (b_isWounded)
            {
                woundedImage.enabled = false;
                b_isWounded = false;
            }
            else
            {
                spriteImage.material.color = transparent;
            }

            deadImage.enabled = true;
        }
        else if (health < (MAX_HEALTH * 20) / 100)
        {
            b_isWounded = true;
            spriteImage.material.color = transparent;
            woundedImage.enabled = true;
        }

        increaseText.enabled = false;
    }

    public IEnumerator OnDead()
    {
        increaseText.enabled = true;
        increaseText.color = Color.red;

        increaseText.text = "VALKYRIE HAS DIED";
        yield return new WaitForSeconds(1);
        increaseText.enabled = false;

        turnController.UpdateTurn();
    }

    public bool isDead()
    {
        return b_isDead;
    }

    public IEnumerator AddToMagic(int magicIncrease)
    {
        increaseText.enabled = true;
        increaseText.color = manaBlue;

        magic += magicIncrease;
        if (magic > MAX_MAGIC)
            magic = MAX_MAGIC;

        increaseText.text = " " + magicIncrease;

        UpdateStats();

        yield return new WaitForSeconds(1);

        increaseText.enabled = false;
    }

    public IEnumerator OnPotion(int amount)
    {
        increaseText.enabled = true;
        increaseText.color = healthGreen;

        if (health + amount > MAX_HEALTH)
        {
            amount = MAX_HEALTH - health;
        }

        StartCoroutine(AddToHealth(amount));

        yield return new WaitForSeconds(1);

        turnController.UpdateTurn();
    }

    public bool isHealthFull()
    {
        if (health == MAX_HEALTH)
            return true;
        else
            return false;
    }

    public IEnumerator OnHealthFull()
    {
        increaseText.enabled = true;
        increaseText.color = healthGreen;

        increaseText.text = "HEALTH FULL";
        yield return new WaitForSeconds(1);
        increaseText.enabled = false;
    }

    public IEnumerator OnMana(int amount)
    {
        increaseText.enabled = true;
        increaseText.color = manaBlue;

        if (magic == MAX_MAGIC)
        {
            increaseText.text = "MAGIC FULL";
            yield return new WaitForSeconds(1);
            increaseText.enabled = false;
        }
        else
        {
            if (magic + amount > MAX_MAGIC)
            {
                amount = MAX_MAGIC - magic;
            }

            AddToMagic(amount);

            turnController.UpdateTurn();

        }
    }

    public bool isMagicFull()
    {
        if (magic == MAX_MAGIC)
            return true;
        else
            return false;
    }

    public IEnumerator OnMagicFull()
    {
        increaseText.enabled = true;
        increaseText.color = manaBlue;

        increaseText.text = "MAGIC FULL";
        yield return new WaitForSeconds(1);
        increaseText.enabled = false;
    }

    private IEnumerator ShootSpears()
    {
        for(int i = 0; i < specialHits; i++)
        {
            spearPosition = new Vector2(Random.Range(480.0f, 640.0f), 738.0f);
            Instantiate(spear, spearPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }

        specialImage.enabled = false;
        spriteImage.material.color = clear;

        enemyController.StartCoroutine(enemyController.TakeDamage(specialHits * 500, false));

        if (b_isWounded)
        {
            spriteImage.material.color = transparent;
            woundedImage.enabled = true;
        }

        turnController.UpdateTurn();
    }

    void UpdateHitText()
    {
        specialHitText.text = " " + specialHits + " -    PRESS SPACEBAR";
    }

    public IEnumerator TakeDamage(int damage)
    {

        //Minus Defence

        damage -= (damage * defencePower) / 100;


        increaseText.color = Color.white;

        health -= damage;

        damageStart = damage - 50;

        animationController.Play("Damaged Animation");
        takeDamageSound.Play();

        increaseText.enabled = true;

        while (damageStart != damage + 1)
        {
            increaseText.text = " " + damageStart;
            damageStart++;
            yield return new WaitForSeconds(0.00000001f);
        }

        chance = Random.Range(0, 99);

        if (chance > 80)
            special += 2;
        else if (chance > 50)
            special++;

        if (special > MAX_SPECIAL)
            special = MAX_SPECIAL;

        UpdateStats();

        if (health <= 0)
        {
            b_isDead = true;
            health = 0;
            if (b_isWounded)
            {
                woundedImage.enabled = false;
                b_isWounded = false;
            }
            else
            {
                spriteImage.material.color = transparent;
            }

            deadImage.enabled = true;
        }
        else if (health < (MAX_HEALTH * 20) / 100)
        {
            b_isWounded = true;
            spriteImage.material.color = transparent;
            woundedImage.enabled = true;
        }

        yield return new WaitForSeconds(1);

        increaseText.enabled = false;
    }

    public int GetHealth()
    {
        return health;
    }

    public IEnumerator OnGuardUp()
    {
        increaseText.enabled = true;
        increaseText.color = Color.white;

        increaseText.text = "FAILED";
        yield return new WaitForSeconds(1);
        increaseText.enabled = false;
    }

    public IEnumerator GuardUp()
    {
        isGuardUp = true;

        defencePower = defencePower * 2;

        increaseText.text = "GUARD UP";

        increaseText.enabled = true;

        yield return new WaitForSeconds(1);

        increaseText.enabled = false;

        increaseText.text = "";

        guardUpImage.enabled = true;
    }

    public void RemoveGuardUp()
    {
        isGuardUp = false;
        defencePower /= 2;
        Destroy(guardUpInstance);
    }

    public bool IsGuardUp()
    {
        return isGuardUp;
    }

    public void setGuardUpImage(bool check)
    {
        guardUpImage.enabled = check;
    }

    public IEnumerator Petrified()
    {

        increaseText.color = Color.white;

        increaseText.text = "PETRIFIED";

        animationController.Play("Damaged Animation");

        increaseText.enabled = true;

        spriteImage.material.color = transparent;

        petrifiedImage.enabled = true;
        
        yield return new WaitForSeconds(2);

        increaseText.enabled = false;
    }

    public bool GetIsPetrified()
    {
        return b_isPetrified;
    }

    public void SetIsPetrified(bool check)
    {
        b_isPetrified = check;
        petrifiedImage.enabled = check;
        spriteImage.material.color = clear;
    }

}
