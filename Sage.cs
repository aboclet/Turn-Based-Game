using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Sage : Characters
{

    private int damageStart;
    private int damageIncrementor;

    private bool isGuardUp = false;
    private bool b_isPetrified = false;

    private GameObject enemy;
    private Enemy enemyController;
    private Paladin paladinController;
    private Valkyrie valkyrieController;
    private TurnController turnController;
    private Animator animationController;
    private List<int> tileIds;
    private List<int> selectedOrder;
    private bool specialExecute;
    private int tileX, tileY, specialCount;
    private float timeLeft;
    private Image blueButtonImage;
    private Image greenButtonImage;
    private Image orangeButtonImage;
    private Image redButtonImage;
    private GameObject blueTileInstance;
    private GameObject greenTileInstance;
    private GameObject orangeTileInstance;
    private GameObject redTileInstance;
    private GameObject earthSpellInstance1;
    private GameObject earthSpellInstance2;
    private GameObject earthSpellInstance3;
    private GameObject earthSpellInstance4;
    private GameObject earthSpellInstance5;
    private GameObject forestPoisonInstance1;
    private GameObject forestPoisonInstance2;
    private GameObject forestPoisonInstance3;
    private GameObject forestPoisonInstance4;
    private GameObject forestPoisonInstance5;
    private GameObject forestConversionInstance1;
    private GameObject forestConversionInstance2;
    private GameObject forestConversionInstance3;
    private GameObject forestConversionInstance4;
    private GameObject forestConversionInstance5;
    private GameObject guardUpInstance;

    private int chance;

    public GameObject gameManager;
    public GameObject paladin;
    public GameObject valkyrie;
    [Header("Texts")]
    public Text healthText;
    public Text specialText;
    public Text increaseText;
    [Header("Special Buttons")]
    public Button blueButton;
    public Button greenButton;
    public Button orangeButton;
    public Button redButton;
    [Header("Special Tiles")]
    public GameObject blueTile;
    public GameObject greenTile;
    public GameObject orangeTile;
    public GameObject redTile;
    [Header("Earth Spell")]
    public GameObject earthSpell1;
    public GameObject earthSpell2;
    public GameObject earthSpell3;
    public GameObject earthSpell4;
    public GameObject earthSpell5;
    [Header("Forest Poison")]
    public GameObject forestPoison1;
    public GameObject forestPoison2;
    public GameObject forestPoison3;
    public GameObject forestPoison4;
    public GameObject forestPoison5;
    [Header("Forest Conversion")]
    public GameObject forestConversion1;
    public GameObject forestConversion2;
    public GameObject forestConversion3;
    public GameObject forestConversion4;
    public GameObject forestConversion5;
    [Header("Ailments")]
    public GameObject guardUp;


    // Use this for initialization
    void Start()
    {
        spriteImage = GetComponent<SpriteRenderer>();

        animationController = GetComponent<Animator>();

        TR_petrified = transform.Find("S_Petrified");
        petrifiedImage = TR_petrified.GetComponentInChildren<SpriteRenderer>();

        TR_special = transform.Find("S_Special");
        specialImage = TR_special.GetComponentInChildren<SpriteRenderer>();

        TR_magic = transform.Find("S_Magic");
        magicImage = TR_magic.GetComponentInChildren<SpriteRenderer>();

        GO_guardUp = GameObject.FindWithTag("S_GuardUp");
        guardUpImage = GO_guardUp.GetComponent<SpriteRenderer>();

        TR_win = transform.Find("S_Win");
        winImage = TR_win.GetComponentInChildren<SpriteRenderer>();

        TR_wounded = transform.Find("S_Wounded");
        woundedImage = TR_wounded.GetComponentInChildren<SpriteRenderer>();

        TR_dead = transform.Find("S_Dead");
        deadImage = TR_dead.GetComponentInChildren<SpriteRenderer>();

        enemy = GameObject.FindWithTag("Enemy");
        enemyController = enemy.GetComponent<Enemy>();
        turnController = gameManager.GetComponent<TurnController>();
        paladinController = paladin.GetComponent<Paladin>();
        valkyrieController = valkyrie.GetComponent<Valkyrie>();

        AudioSource[] audios = GetComponents<AudioSource>();
        takeDamageSound = audios[0];
        attackSound = audios[1];
        magicSound = audios[2];

        b_isDead = false;
        b_isWounded = false;

        blueButtonImage = blueButton.GetComponent<Image>();
        greenButtonImage = greenButton.GetComponent<Image>();
        orangeButtonImage = orangeButton.GetComponent<Image>();
        redButtonImage = redButton.GetComponent<Image>();

        //Initialise Stats



        //Derived Stats
        attackPower = STRENGTH * 10;    // 200 avg damage
        magicPower = SPIRIT * 10;       // 1000 avg earth spell damage
        dodge = AGILITY * 2;        // 16% Chance of dodging
        defencePower = DEFENCE * 2;     //6%
        special = 0;
        health = MAX_HEALTH;
        magic = MAX_MAGIC;



        tileIds = new List<int>();
        selectedOrder = new List<int>();
        tileIds.Add(1);
        tileIds.Add(2);
        tileIds.Add(3);
        tileIds.Add(4);

        UpdateStats();

    }

    // Update is called once per frame
    void Update()
    {
        if(specialExecute)
        {
            if(timeLeft < 0.0f || selectedOrder.Count == 4)
            {
                specialExecute = false;
                StartCoroutine(ExecuteSpecial());
            }

            timeLeft -= 1.0f * Time.deltaTime;
            specialText.text = "TIME LEFT = " + Mathf.RoundToInt(timeLeft);
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

        selectedOrder.Clear();

        if (b_isWounded)
        {
            spriteImage.material.color = clear;
            woundedImage.enabled = false;
        }

        specialImage.enabled = true;
        spriteImage.material.color = transparent;

        for (int i = 0; i < tileIds.Count; i++)
        {
            int temp = tileIds[i];
            int randomIndex = Random.Range(i, tileIds.Count);
            tileIds[i] = tileIds[randomIndex];
            tileIds[randomIndex] = temp;
        }

        

        // 1 == blue
        // 2 == green
        // 3 == orange
        // 4 == red

        tileX = 368;
        tileY = 442;


        for (int i = 0; i < tileIds.Count; i++)
        {
            if (tileIds[i] == 1)
                blueTileInstance = (GameObject)Instantiate(blueTile, new Vector2(tileX, tileY), Quaternion.identity);
            else if (tileIds[i] == 2)
                greenTileInstance = (GameObject)Instantiate(greenTile, new Vector2(tileX, tileY), Quaternion.identity);
            else if (tileIds[i] == 3)
                orangeTileInstance = (GameObject)Instantiate(orangeTile, new Vector2(tileX, tileY), Quaternion.identity);
            else if(tileIds[i] == 4)
                redTileInstance = (GameObject)Instantiate(redTile, new Vector2(tileX, tileY), Quaternion.identity);

            tileX += 96;
        }

        timeLeft = 5.0f;
        specialCount = 0;
        specialText.enabled = true;

        StartCoroutine(SpecialOrder());



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


    public IEnumerator OnMagic(int type)
    {

        if (b_isWounded)
        {
            spriteImage.material.color = clear;
            woundedImage.enabled = false;
        }

        spriteImage.material.color = transparent;
        magicImage.enabled = true;
        magicSound.Play();

        switch (type)
        {
            case 1:
            {
                earthSpellInstance1 = (GameObject)Instantiate(earthSpell1, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(earthSpellInstance1);

                earthSpellInstance2 = (GameObject)Instantiate(earthSpell2, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(earthSpellInstance2);

                earthSpellInstance3 = (GameObject)Instantiate(earthSpell3, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(earthSpellInstance3);

                earthSpellInstance4 = (GameObject)Instantiate(earthSpell4, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(earthSpellInstance4);

                earthSpellInstance5 = (GameObject)Instantiate(earthSpell5, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(earthSpellInstance5);

                yield return new WaitForSeconds(1);

                enemyController.StartCoroutine(enemyController.TakeDamage(Random.Range(magicPower - (magicPower / 2), magicPower + (magicPower / 2)), false));

                magic -= spell1;
            }
            break;

            case 2:
            {
                forestPoisonInstance1 = (GameObject)Instantiate(forestPoison1, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestPoisonInstance1);

                forestPoisonInstance2 = (GameObject)Instantiate(forestPoison2, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestPoisonInstance2);

                forestPoisonInstance3 = (GameObject)Instantiate(forestPoison3, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestPoisonInstance3);

                forestPoisonInstance4 = (GameObject)Instantiate(forestPoison4, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestPoisonInstance4);

                forestPoisonInstance5 = (GameObject)Instantiate(forestPoison5, new Vector2(370.0f, 327.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestPoisonInstance5);

                yield return new WaitForSeconds(1);

                enemyController.StartCoroutine(enemyController.Poisoned());

                magic -= spell2;

            }
            break;
            case 3:
            {
                forestConversionInstance1 = (GameObject)Instantiate(forestConversion1, new Vector2(662.0f, 330.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestConversionInstance1);

                forestConversionInstance2 = (GameObject)Instantiate(forestConversion2, new Vector2(662.0f, 330.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestConversionInstance2);

                forestConversionInstance3 = (GameObject)Instantiate(forestConversion3, new Vector2(662.0f, 330.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestConversionInstance3);

                forestConversionInstance4 = (GameObject)Instantiate(forestConversion4, new Vector2(662.0f, 330.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestConversionInstance4);

                forestConversionInstance5 = (GameObject)Instantiate(forestConversion5, new Vector2(662.0f, 330.0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Destroy(forestConversionInstance5);

                yield return new WaitForSeconds(1);

                turnController.SetForestConversion(true);

                magic -= spell3;

            }
            break;
        }
        
        yield return new WaitForSeconds(1);

        magicImage.enabled = false;
        spriteImage.material.color = clear;

        UpdateStats();

        if (b_isWounded)
        {
            spriteImage.material.color = transparent;
            woundedImage.enabled = true;
        }

        turnController.UpdateTurn();
    }

    private void UpdateStats()
    {
        healthText.text = "SAGE                  " + health + "/  " + MAX_HEALTH + "   " + magic + "/  " + MAX_MAGIC + "    " + special + "/  " + "5";
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

        increaseText.enabled = false;
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

    private IEnumerator SpecialOrder()
    {

        specialText.text = "REMEMBER THE ORDER";
        yield return new WaitForSeconds(3);

        Destroy(blueTileInstance);
        Destroy(orangeTileInstance);
        Destroy(greenTileInstance);
        Destroy(redTileInstance);

        blueButton.enabled = true;
        greenButton.enabled = true;
        orangeButton.enabled = true;
        redButton.enabled = true;
        blueButtonImage.enabled = true;
        greenButtonImage.enabled = true;
        orangeButtonImage.enabled = true;
        redButtonImage.enabled = true;

        specialExecute = true;
    }


    private IEnumerator ExecuteSpecial()
    {
        blueButton.enabled = false;
        greenButton.enabled = false;
        orangeButton.enabled = false;
        redButton.enabled = false;
        blueButtonImage.enabled = false;
        greenButtonImage.enabled = false;
        orangeButtonImage.enabled = false;
        redButtonImage.enabled = false;
        specialText.enabled = false;

        specialImage.enabled = false;
        spriteImage.material.color = clear;

        if (selectedOrder.Count == 4 && CheckMatch())
        {
            valkyrieController.StartCoroutine(valkyrieController.AddToHealth(6500));
            yield return new WaitForSeconds(1);
            valkyrieController.StartCoroutine(valkyrieController.AddToMagic(2000));
            yield return new WaitForSeconds(1);
            paladinController.StartCoroutine(paladinController.AddToHealth(8000));
            yield return new WaitForSeconds(1);
            paladinController.StartCoroutine(paladinController.AddToMagic(2000));
            yield return new WaitForSeconds(1);
            StartCoroutine(AddToHealth(5000));
            yield return new WaitForSeconds(1);
            StartCoroutine(AddToMagic(8000));
            yield return new WaitForSeconds(1);
        }
        else
        {
            valkyrieController.StartCoroutine(valkyrieController.AddToHealth(1625));
            yield return new WaitForSeconds(1);
            valkyrieController.StartCoroutine(valkyrieController.AddToMagic(500));
            yield return new WaitForSeconds(1);
            paladinController.StartCoroutine(paladinController.AddToHealth(2000));
            yield return new WaitForSeconds(1);
            paladinController.StartCoroutine(paladinController.AddToMagic(500));
            yield return new WaitForSeconds(1);
            StartCoroutine(AddToHealth(1250));
            yield return new WaitForSeconds(1);
            StartCoroutine(AddToMagic(2000));
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        if (b_isWounded)
        {
            spriteImage.material.color = transparent;
            woundedImage.enabled = true;
        }

        turnController.UpdateTurn();
    }

    public int GetHealth()
    {
        return health;
    }

    public IEnumerator TakeDamage(int damage)
    {

        //Minus Defence

        damage -= damage * (defencePower / 100);

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

    public IEnumerator OnDead()
    {
        increaseText.enabled = true;
        increaseText.color = Color.red;

        increaseText.text = "SAGE HAS DIED";
        yield return new WaitForSeconds(1);
        increaseText.enabled = false;

        turnController.UpdateTurn();
    }

    public bool isDead()
    {
        return b_isDead;
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

        b_isPetrified = true;
        petrifiedImage.enabled = true;
        spriteImage.material.color = transparent;

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


    public void BlueClicked()
    {
        selectedOrder.Add(1);
        blueButton.interactable = false;
    }

    public void GreenClicked()
    {
        selectedOrder.Add(2);
        greenButton.interactable = false;
    }

    public void OrangeClicked()
    {
        selectedOrder.Add(3);
        orangeButton.interactable = false;
    }
    public void RedClicked()
    {
        selectedOrder.Add(4);
        redButton.interactable = false;
    }

    private bool CheckMatch()
    {
        for (int i = 0; i < tileIds.Count; i++)
        {
            if (tileIds[i] != selectedOrder[i])
                return false;
        }

        return true;
    }


}
