using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Paladin : Characters
{

//================PUBLIC==========================

    public GameObject gameManager;
    public Text healthText;
    public Text increaseText;

    [Header("Special")]
    public GameObject ball;
    public GameObject bar1;
    public GameObject bar2;
    public GameObject bar3;

    [Header("Magic")]
    public GameObject magic1;
    public GameObject magic2;
    public GameObject magic3;
    public GameObject magic4;

    [Header("Ailments")]
    public GameObject guardUp;

//================PRIVATE=========================

    private GameObject magicInstance1;
    private GameObject magicInstance2;
    private GameObject magicInstance3;
    private GameObject magicInstance4;
    private GameObject guardUpInstance;

    private GameObject enemy;
    private Enemy enemyController;
    private Animator animationController;
    private TurnController turnController;
    //Stats//

    private int damageStart;
    private int damageIncrementor;

    private bool isGuardUp = false;
    private bool b_isPetrified = false;

    private bool specialActive;
    private int specialStage;
    private int leftBoundaryX;
    private int rightBoundaryX;

    private int chance;


    // Use this for initialization
    void Start () {

        spriteImage = GetComponent<SpriteRenderer>();

        animationController = GetComponent<Animator>();
        turnController = gameManager.GetComponent<TurnController>();

        b_isDead = false;
        b_isWounded = false;


    //Initialise Images

        TR_petrified = transform.Find("P_Petrified");
        petrifiedImage = TR_petrified.GetComponentInChildren<SpriteRenderer>();

        TR_magic = transform.Find("P_Magic");
        magicImage = TR_magic.GetComponentInChildren<SpriteRenderer>();

        GO_guardUp = GameObject.FindWithTag("P_GuardUp");
        guardUpImage = GO_guardUp.GetComponent<SpriteRenderer>();

        TR_win = transform.Find("P_Win");
        winImage = TR_win.GetComponentInChildren<SpriteRenderer>();

        TR_wounded = transform.Find("P_Wounded");
        woundedImage = TR_wounded.GetComponentInChildren<SpriteRenderer>();

        TR_dead = transform.Find("P_Dead");
        deadImage = TR_dead.GetComponentInChildren<SpriteRenderer>();

        AudioSource[] audios = GetComponents<AudioSource>();
        takeDamageSound = audios[0];
        attackSound = audios[1];
        magicSound = audios[2];

    //Initialise Stats
        attackPower = STRENGTH * 10;    // 1000 avg damage
        magicPower = 0;             // No magic offense
        dodge = AGILITY * 2;        // 6% Chance of dodging
        defencePower = DEFENCE * 2;     //18%
        special = 0;
        health = MAX_HEALTH;
        magic = MAX_MAGIC;
        
        specialActive = false;

        UpdateStats();

        enemy = GameObject.FindWithTag("Enemy");
        enemyController = enemy.GetComponent<Enemy>();

	}
	
	// Update is called once per frame
	void Update () {

        if(specialActive)
        {
            if(Input.GetKeyDown("space"))
            {
                if(ball.transform.position.x > leftBoundaryX && ball.transform.position.x <= rightBoundaryX)
                {
                    if (specialStage == 1)
                    {
                        Destroy(bar1);
                        bar2 = Instantiate(bar2);
                        leftBoundaryX = 451;
                        rightBoundaryX = 567;
                        specialStage++;
                    }
                    else if (specialStage == 2)
                    {
                        Destroy(bar2);
                        bar3 = Instantiate(bar3);
                        leftBoundaryX = 487;
                        rightBoundaryX = 531;
                        specialStage++;
                    }
                    else if (specialStage == 3)
                    {
                        Destroy(bar3);
                        Destroy(ball);
                        ExecuteSpecial();
                        specialActive = false;
                    }
                }
                else
                {
                    if (specialStage == 1)
                        Destroy(bar1);
                    else if (specialStage == 2)
                        Destroy(bar2);
                    else if (specialStage == 3)
                        Destroy(bar3);
                    


                    Destroy(ball);
                    StartCoroutine(ExecuteSpecial());
                    specialActive = false;
                }
         
            }
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

        ball = Instantiate(ball);
        bar1 = Instantiate(bar1);

        animationController.Play("Special Charging");

        specialActive = true;
        specialStage = 1;
        leftBoundaryX = 414;
        rightBoundaryX = 603;    
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

    private IEnumerator ExecuteSpecial()
    {
        animationController.Play("Special Attack");

        yield return new WaitForSeconds(1);

        switch(specialStage)
        {
            case 1:
                enemyController.StartCoroutine(enemyController.TakeDamage(2500, false));
                break;
            case 2:
                enemyController.StartCoroutine(enemyController.TakeDamage(5000, false));
                break;
            case 3:
                enemyController.StartCoroutine(enemyController.TakeDamage(10000, false));
                break;
        }

        special = 0;

        yield return new WaitForSeconds(1);

        if (b_isWounded)
        {
            spriteImage.material.color = transparent;
            woundedImage.enabled = true;
        }

        turnController.UpdateTurn();

        
    }

    public IEnumerator OnMagic()
    {

        if(!enemyController.IsAttackDown())
        {
            if (b_isWounded)
            {
                spriteImage.material.color = clear;
                woundedImage.enabled = false;
            }

            magicImage.enabled = true;

            spriteImage.material.color = transparent;

            magicSound.Play();

            magicInstance1 = (GameObject)Instantiate(magic1, new Vector2(370.0f, 327.0f), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Destroy(magicInstance1);

            magicInstance2 = (GameObject)Instantiate(magic2, new Vector2(370.0f, 327.0f), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Destroy(magicInstance2);

            magicInstance3 = (GameObject)Instantiate(magic3, new Vector2(370.0f, 327.0f), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Destroy(magicInstance3);

            magicInstance4 = (GameObject)Instantiate(magic4, new Vector2(370.0f, 327.0f), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Destroy(magicInstance4);


            yield return new WaitForSeconds(1);

            enemyController.StartCoroutine(enemyController.AttackDown());

            magic -= spell1;
            UpdateStats();

            magicImage.enabled = false;

            spriteImage.material.color = clear;

            if (b_isWounded)
            {
                spriteImage.material.color = transparent;
                woundedImage.enabled = true;
            }

            turnController.UpdateTurn();
        }
        else
        {
            increaseText.enabled = true;
            increaseText.color = Color.white;

            increaseText.text = "FAILED";
            yield return new WaitForSeconds(1);
            increaseText.enabled = false;
        }


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

        if (health + amount > MAX_MAGIC)
        {
            amount = MAX_MAGIC - magic;
        }

        StartCoroutine(AddToMagic(amount));

        yield return new WaitForSeconds(1);

        turnController.UpdateTurn();
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

    public IEnumerator OnDead()
    {
        increaseText.enabled = true;
        increaseText.color = Color.red;

        increaseText.text = "PALADIN HAS DIED";
        yield return new WaitForSeconds(1);
        increaseText.enabled = false;

        turnController.UpdateTurn();
    }

    public bool isDead()
    {
        return b_isDead;
    }


    private void UpdateStats()
    {
        healthText.text = "PALADIN         " + health + "/  " +MAX_HEALTH + "   " + magic + "/  " + MAX_MAGIC + "   " + special + "/  " + "5";
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

        if(b_isWounded)
        {
            if(health > (MAX_HEALTH * 20) / 100)
            {
                b_isWounded = false;
                woundedImage.enabled = false;
                spriteImage.material.color = clear;
            }
        }

        increaseText.enabled = false;
    }

    public int GetHealth()
    {
        return health;
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

    public IEnumerator TakeDamage(int damage)
    {

        if(turnController.GetForestConversion())
        {
            increaseText.text = "forest conversion";
        }
        else
        {

            damage -= (damage * (defencePower)) / 100;

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
                special ++;

            if (special > MAX_SPECIAL)
                special = MAX_SPECIAL;

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

            UpdateStats();
        }



        yield return new WaitForSeconds(1);

        increaseText.enabled = false;

    }

    public IEnumerator OnGuardUp()
    {
        increaseText.enabled = true;
        increaseText.color = healthGreen;

        increaseText.text = "GUARD ALREADY UP";
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
