using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {

    private Animator animationController;
    private Paladin paladinController;
    private Valkyrie valkyrieController;
    private Sage sageController;
    private TurnController turnController;

    private Transform attackDownTransform;
    private SpriteRenderer attackDownImage;
    protected Transform TR_enraged;
    protected SpriteRenderer enragedImage;

    private GameObject boulderInstance;

    private int quarterHealth;
    private int specialPower;

    private int damageStart = 0;
    private int damageIncrementor;
    private bool attackDown = false;
    private bool poisoned = false;
    private int chance;

    private bool b_specialExecute = false;
    private bool b_specialUsed = false;


    private int ailmentCount = 0;
    private int petrifyCount = 0;

    private Color poisonPurple;

    [Header("Stats")]
    public int healthPoints;
    public int attackPower;
    [Header("Texts")]
    public Text damageText;
    public Text healthText;
    public GameObject gameManager;
    public GameObject paladin;
    public GameObject valkyrie;
    public GameObject sage;
    public GameObject boulder;

    

    // Use this for initialization
    void Start () {


        animationController = GetComponent<Animator>();
        paladinController = paladin.GetComponent<Paladin>();
        valkyrieController = valkyrie.GetComponent<Valkyrie>();
        sageController = sage.GetComponent<Sage>();
        turnController = gameManager.GetComponent<TurnController>();


        attackDownTransform = transform.Find("Attack Down");
        attackDownImage = attackDownTransform.GetComponentInChildren<SpriteRenderer>();

        TR_enraged = transform.Find("Enraged");
        enragedImage = TR_enraged.GetComponentInChildren<SpriteRenderer>();

        quarterHealth = healthPoints / 4;
        specialPower = attackPower * 6;

        poisonPurple = new Color(139, 0, 139);

        ailmentCount = 0;

        poisoned = false;

        damageText.enabled = false;



	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public IEnumerator TakeDamage(int damage, bool b_poisonDamage)
    {
        healthPoints -= damage;
        healthText.text = "" + (healthPoints - damage);

        damageStart = damage - 50;    

        animationController.Play("Damaged Animation");
        GetComponent<AudioSource>().Play();

        if (b_poisonDamage)
            damageText.color = poisonPurple;
        else
            damageText.color = Color.white;

        damageText.enabled = true;

        while (damageStart != damage + 1)
        {
            damageText.text = " " + damageStart;
            damageStart++;
            yield return new WaitForSeconds(0.00000001f);
        }

        yield return new WaitForSeconds(1);

        damageText.enabled = false;

    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public IEnumerator DecideAction()
    {
        

        chance = Random.Range(1, 100);

        yield return new WaitForSeconds(1);

        if (b_specialExecute && !b_specialUsed)
            StartCoroutine(SpecialAttack());
        else if (healthPoints < quarterHealth && !b_specialExecute)
            StartCoroutine(OnEnraged());
        else if (petrifyCount >= 3)
            StartCoroutine(Petrify());
        else if (chance <= 25)
            StartCoroutine(AllCharacterAttack());
        else if (ailmentCount >= 4 && attackDown || poisoned)
            StartCoroutine(RemoveAilments());
        else
            StartCoroutine(StandardAttack());
        
        yield return new WaitForSeconds(3);

        if (attackDown)
            ailmentCount++;

        if (poisoned)
        {
            StartCoroutine(TakeDamage(Random.Range(500, 750), true));
            yield return new WaitForSeconds(3);
            ailmentCount++;
        }
            

        petrifyCount++;
        
    }

    public void CallDamageAnimation()
    {
        animationController.Play("Damaged Animation");
    }

    public IEnumerator AttackDown()
    {
        attackDown = true;

        attackPower = attackPower / 2;

        damageText.text = "ATTACK DOWN";

        damageText.enabled = true;

        animationController.Play("Damaged Animation");

        yield return new WaitForSeconds(1);

        attackDownImage.enabled = true;

        damageText.enabled = false;

        damageText.text = "";
  
    }

    public bool IsAttackDown()
    {
        return attackDown;
    }

    public IEnumerator RemoveAilments()
    {
        attackDown = false;
        poisoned = false;

        attackPower = attackPower * 2;

        if (paladinController.IsGuardUp())
            paladinController.RemoveGuardUp();

        if (sageController.IsGuardUp())
            valkyrieController.RemoveGuardUp();

        if (sageController.IsGuardUp())
            sageController.RemoveGuardUp();

        damageText.text = "AILMENTS REMOVED";

        damageText.enabled = true;

        yield return new WaitForSeconds(1);

        attackDownImage.enabled = false;

        damageText.enabled = false;

        damageText.text = "";

        ailmentCount = 0;

    }

    public bool IsPoisoned()
    {
        return poisoned;
    }

    public IEnumerator Poisoned()
    {
        poisoned = true;

        damageText.text = "POISONED";

        animationController.Play("Damaged Animation");

        damageText.color = poisonPurple;

        damageText.enabled = true;

        yield return new WaitForSeconds(1);

        damageText.enabled = false;

        damageText.text = "";

    }

    public IEnumerator OnPoisoned()
    {

        damageText.text = "ALREADY POISONED";

        damageText.color = Color.white;

        damageText.enabled = true;

        yield return new WaitForSeconds(1);

        damageText.enabled = false;

        damageText.text = "";

    }

    private IEnumerator StandardAttack()
    {

        animationController.Play("Attack Animation");

        yield return new WaitForSeconds(1);

        if (turnController.GetForestConversion())
        {
            damageText.text = "FOREST CONVERSION";

            damageText.enabled = true;

            yield return new WaitForSeconds(1);

            StartCoroutine(TakeDamage(Random.Range(attackPower - (attackPower / 2), attackPower + (attackPower / 2)), false));

            yield return new WaitForSeconds(1);

            damageText.enabled = false;

            turnController.SetForestConversion(true);
        }
        else
        {
            chance = Random.Range(1, 3);

            switch (chance)
            {

                case 1:
                    paladinController.StartCoroutine(paladinController.TakeDamage(Random.Range(attackPower - (attackPower / 2), attackPower + (attackPower / 2))));
                    break;

                case 2:
                    valkyrieController.StartCoroutine(valkyrieController.TakeDamage(Random.Range(attackPower - (attackPower / 2), attackPower + (attackPower / 2))));
                    break;

                case 3:
                    sageController.StartCoroutine(sageController.TakeDamage(Random.Range(attackPower - (attackPower / 2), attackPower + (attackPower / 2))));
                    break;

            }
        }
    }

    private IEnumerator AllCharacterAttack()
    {
        animationController.Play("Attack Animation");

        yield return new WaitForSeconds(1);

        if (turnController.GetForestConversion())
        {
            damageText.color = Color.green;
            damageText.text = "FOREST CONVERSION";

            damageText.enabled = true;
            yield return new WaitForSeconds(1);

            StartCoroutine(TakeDamage(Random.Range(attackPower - (attackPower / 2), attackPower + (attackPower / 2) * 3), false));
            yield return new WaitForSeconds(1);

            damageText.enabled = false;

            turnController.SetForestConversion(true);
        }
        else
        {
            paladinController.StartCoroutine(paladinController.TakeDamage(Random.Range(attackPower - (attackPower / 2), attackPower + (attackPower / 2))));
            valkyrieController.StartCoroutine(valkyrieController.TakeDamage(Random.Range(attackPower - (attackPower / 2), attackPower + (attackPower / 2))));
            sageController.StartCoroutine(sageController.TakeDamage(Random.Range(attackPower - (attackPower / 2), attackPower + (attackPower / 2))));
        }


    }

    private IEnumerator SpecialAttack()
    {
        animationController.Play("Special Animation");

        boulderInstance = Instantiate(boulder);

        yield return new WaitForSeconds(1);

        if(!paladinController.isDead())
            paladinController.StartCoroutine(paladinController.TakeDamage(Random.Range(specialPower - (specialPower / 2), specialPower + (specialPower / 2))));

        valkyrieController.StartCoroutine(valkyrieController.TakeDamage(Random.Range(specialPower - (specialPower / 2), specialPower + (specialPower / 2))));
        sageController.StartCoroutine(sageController.TakeDamage(Random.Range(specialPower - (specialPower / 2), specialPower + (specialPower / 2))));

        b_specialUsed = true;

        attackPower *= 2;


    }

    private IEnumerator Petrify()
    {
        animationController.Play("Petrify Animation");

        yield return new WaitForSeconds(1);

        chance = Random.Range(1, 3);

        switch (chance)
        {

            case 1:
                paladinController.SetIsPetrified(true);
                paladinController.StartCoroutine(paladinController.Petrified());               
                break;

            case 2:
                valkyrieController.SetIsPetrified(true);
                valkyrieController.StartCoroutine(valkyrieController.Petrified());
                break;

            case 3:
                sageController.SetIsPetrified(true);
                sageController.StartCoroutine(sageController.Petrified());
                break;

        }

        petrifyCount = 0;

    }

    private IEnumerator OnEnraged()
    {
        enragedImage.enabled = true;

        damageText.color = Color.white;
        damageText.text = "CYCLOPS IS ENRAGED";
        damageText.enabled = false;

        b_specialExecute = true;

        yield return new WaitForSeconds(1);

        damageText.enabled = false;


    }


}
