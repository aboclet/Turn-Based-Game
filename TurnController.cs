using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnController : MonoBehaviour {

    private enum Turn
    {
        PALADIN,
        VALKYRIE,
        SAGE,
        ENEMY,
        TOTAL_TURNS,
    };

    [Header("Portraits")]
    public GameObject paladinPortrait;
    public GameObject valkyriePortrait;
    public GameObject sagePortrait;
    [Header("Buttons")]
    public Button attackButton;
    public Button magicButton;
    public Button specialButton;
    public Button itemButton;
    public Button potionButton;
    public Button antidoteButton;
    public Button manaButton;
    public Button magic1Button;
    public Button magic2Button;
    public Button magic3Button;
    public Button special1Button;
    public Button paladinSelectButton;
    public Button valkyrieSelectButton;
    public Button sageSelectButton;
    [Header("Texts")]
    public Text stateText;
    public Text selectText;

    private Turn currentTurn;
    private GameObject paladin;
    private GameObject valkyrie;
    private GameObject sage;
    private GameObject enemy;
    private Paladin paladinController;
    private Valkyrie valkyrieController;
    private Sage sageController;
    private Enemy enemyController;

    private SpriteRenderer paladinPortraitRenderer;
    private SpriteRenderer valkyriePortraitRenderer;
    private SpriteRenderer sagePortraitRenderer;

    private Image potionButtonImage;
    private Image manaButtonImage;
    private Image magic1ButtonImage;
    private Image magic2ButtonImage;
    private Image magic3ButtonImage;
    private Image special1ButtonImage;
    private Image paladinSelectImage;
    private Image valkyrieSelectImage;
    private Image sageSelectImage;
    private Text potionButtonText;
    private Text manaButtonText;
    private Text magic1ButtonText;
    private Text magic2ButtonText;
    private Text magic3ButtonText;
    private Text special1ButtonText;
    private Text paladinSelectText;
    private Text valkyrieSelectText;
    private Text sageSelectText;

    private bool itemButtonSelected;
    private bool magicButtonSelected;
    private bool specialButtonSelected;
    private bool potionButtonSelected;
    private bool manaButtonSelected;
    private bool forestConversionOn;

    private bool FAST_PLAY = true;
    private int rounds = 0;

    // Use this for initialization
    void Start () {

        currentTurn = Turn.PALADIN;

        stateText.text = "PALADIN'S TURN";

        InitialiseVariables();

        forestConversionOn = false;

        paladinPortraitRenderer.enabled = true;
        //valkyriePortraitRenderer.enabled = true;
        //sagePortraitRenderer.enabled = true;


    }

    void Update()
    {
        
    }

    private void InitialiseVariables()
    {
        paladin = GameObject.FindWithTag("Paladin");
        valkyrie = GameObject.FindWithTag("Valkyrie");
        sage = GameObject.FindWithTag("Sage");
        enemy = GameObject.FindWithTag("Enemy");
        paladinController = paladin.GetComponent<Paladin>();
        sageController = sage.GetComponent<Sage>();
        valkyrieController = valkyrie.GetComponent<Valkyrie>();
        enemyController = enemy.GetComponent<Enemy>();

        paladinPortraitRenderer = paladinPortrait.GetComponent<SpriteRenderer>();
        valkyriePortraitRenderer = valkyriePortrait.GetComponent<SpriteRenderer>();
        sagePortraitRenderer = sagePortrait.GetComponent<SpriteRenderer>();

        potionButtonImage = potionButton.GetComponent<Image>();
        manaButtonImage = manaButton.GetComponent<Image>();
        magic1ButtonImage = magic1Button.GetComponent<Image>();
        magic2ButtonImage = magic2Button.GetComponent<Image>();
        magic3ButtonImage = magic3Button.GetComponent<Image>();
        special1ButtonImage = special1Button.GetComponent<Image>();
        paladinSelectImage = paladinSelectButton.GetComponent<Image>();
        valkyrieSelectImage = valkyrieSelectButton.GetComponent<Image>();
        sageSelectImage = sageSelectButton.GetComponent<Image>();

        potionButtonText = potionButton.GetComponentInChildren<Text>();
        manaButtonText = manaButton.GetComponentInChildren<Text>();
        magic1ButtonText = magic1Button.GetComponentInChildren<Text>();
        magic2ButtonText = magic2Button.GetComponentInChildren<Text>();
        magic3ButtonText = magic3Button.GetComponentInChildren<Text>();
        special1ButtonText = special1Button.GetComponentInChildren<Text>();
        paladinSelectText = paladinSelectButton.GetComponentInChildren<Text>();
        valkyrieSelectText = valkyrieSelectButton.GetComponentInChildren<Text>();
        sageSelectText = sageSelectButton.GetComponentInChildren<Text>();

        
    }

    public void UpdateTurn()
    {

        

        currentTurn++;
        rounds++;

        if(currentTurn == Turn.TOTAL_TURNS)
        {
            currentTurn = Turn.PALADIN;
        }
   
        magicButtonSelected = false;
        potionButtonSelected = false;
        manaButtonSelected = false;

        if (paladinController.IsGuardUp())
            paladinController.setGuardUpImage(false);

        if (valkyrieController.IsGuardUp())
            valkyrieController.setGuardUpImage(false);

        if (sageController.IsGuardUp())
            sageController.setGuardUpImage(false);

        if (currentTurn == Turn.PALADIN)
        {
            if(paladinController.isDead())
            {
                paladinController.StartCoroutine(paladinController.OnDead());
            }

            if (paladinController.GetIsPetrified())
            {
                EnableAllButtons(false);
                StartCoroutine(ShowPetrificationStatus(1));
            }

            if (paladinController.IsGuardUp())
                paladinController.setGuardUpImage(true);
        }

        if (currentTurn == Turn.VALKYRIE)
        {
            if (valkyrieController.isDead())
            {
                valkyrieController.StartCoroutine(valkyrieController.OnDead());
            }

            if (valkyrieController.GetIsPetrified())
            {
                EnableAllButtons(false);
                StartCoroutine(ShowPetrificationStatus(2));
            }

            if (valkyrieController.IsGuardUp())
                valkyrieController.setGuardUpImage(true);
        }

        if (currentTurn == Turn.SAGE)
        {

            if (sageController.isDead())
            {
                sageController.StartCoroutine(sageController.OnDead());
            }

            if (sageController.GetIsPetrified())
            {
                EnableAllButtons(false);
                StartCoroutine(ShowPetrificationStatus(3));
            }

            if (sageController.IsGuardUp())
                sageController.setGuardUpImage(true);
        }

        EnableAllButtons(true);

        

        switch (currentTurn)
        {
            case Turn.PALADIN:
            {
                stateText.text = "PALADIN'S TURN";
                paladinPortraitRenderer.enabled = true;
                valkyriePortraitRenderer.enabled = false;
                sagePortraitRenderer.enabled = false;
            }
            break;
            case Turn.VALKYRIE:
            {
                stateText.text = "VALKYRIE'S TURN";
                paladinPortraitRenderer.enabled = false;
                valkyriePortraitRenderer.enabled = true;
                sagePortraitRenderer.enabled = false;
            }
            break;
            case Turn.SAGE:
            {
                stateText.text = "SAGE'S TURN";
                paladinPortraitRenderer.enabled = false;
                valkyriePortraitRenderer.enabled = false;
                sagePortraitRenderer.enabled = true;
            }
            break;

            case Turn.ENEMY:
            {
                stateText.text = "CYCLOP'S TURN";
                paladinPortraitRenderer.enabled = false;
                valkyriePortraitRenderer.enabled = false;
                sagePortraitRenderer.enabled = false;
                EnableAllButtons(false);

                enemyController.StartCoroutine(enemyController.DecideAction());

            }
            break;
        }

        if (FAST_PLAY)
            FastPlay();

        if (enemyController.GetHealth() <= 0)
            EndGame();

    }
	

    //--------------------------------------BUTTON PRESSES----------------------------------------//

    public void OnAttack()
    {
        EnableAllButtons(false);
        EnableItemButtons(false);
        EnableMagicButtons(false);
        EnableSpecialButton(false);

        switch (currentTurn)
        {
            case Turn.PALADIN:
                {
                   paladinController.StartCoroutine(paladinController.OnAttack());
                }
                break;
            case Turn.VALKYRIE:
                {
                    valkyrieController.StartCoroutine(valkyrieController.OnAttack());
                }
                break;
            case Turn.SAGE:
                {
                    sageController.StartCoroutine(sageController.OnAttack());              
                }
                break;
        }

        

    }

    public void OnSpecial()
    {
        EnableItemButtons(false);
        EnableMagicButtons(false);

        EnableSpecialButton(true);
    }

    public void OnSpecialSelected()
    {

        EnableAllButtons(false);

        switch (currentTurn)
        {
            case Turn.PALADIN:
                {
                    if(paladinController.GetSpecial() == 5)
                    {
                        special1ButtonImage.enabled = false;
                        special1ButtonText.enabled = false;
                        special1Button.interactable = false;
                        paladinController.OnSpecial();
                        stateText.text = "JUDGEMENT";
                    }
                    else
                    {
                        paladinController.StartCoroutine(paladinController.OnNotEnoughSpecial());
                        EnableAllButtons(true);
                    }
                
                }
                break;
            case Turn.VALKYRIE:
                {
                    if (valkyrieController.GetSpecial() == 5)
                    {
                        special1ButtonImage.enabled = false;
                        special1ButtonText.enabled = false;
                        special1Button.interactable = false;
                        valkyrieController.OnSpecial();
                        stateText.text = "FALLING STARS";
                    }
                    else
                    {
                        valkyrieController.StartCoroutine(valkyrieController.OnNotEnoughSpecial());
                        EnableAllButtons(true);
                    }
                }
                break;
            case Turn.SAGE:
                {
                    if(sageController.GetSpecial() == 5)
                    {
                        special1ButtonImage.enabled = false;
                        special1ButtonText.enabled = false;
                        special1Button.interactable = false;
                        sageController.OnSpecial();
                        stateText.text = "FOREST CALLING";
                    }
                    else
                    {
                        valkyrieController.StartCoroutine(valkyrieController.OnNotEnoughSpecial());
                        EnableAllButtons(true);
                    }
                    
                }
                break;
        }

    }

    public void OnItem()
    {
        EnableMagicButtons(false);
        EnableSpecialButton(false);

        EnableItemButtons(true);
       
    }

    public void OnMagic()
    {
        EnableItemButtons(false);
        EnableSpecialButton(false);

        EnableMagicButtons(true);

    }

    public void OnMagicButton1()
    {
        magicButtonSelected = true;
        EnableMagicButtons(false);
        EnableAllButtons(false);

        switch (currentTurn)
        {
            case Turn.PALADIN:
                {
                    if(!enemyController.IsAttackDown())
                    {
                        paladinController.StartCoroutine(paladinController.OnMagic());
                        stateText.text = "ATTACK DOWN";
                    }                      
                    else
                    {
                        paladinController.StartCoroutine(paladinController.OnMagic());
                        EnableAllButtons(true);
                    }
                        

                }
                break;
            case Turn.VALKYRIE:
                {
                    EnableSelectionButtons(true);
                }
                break;
            case Turn.SAGE:
                {
                    sageController.StartCoroutine(sageController.OnMagic(1));
                    stateText.text = "EARTH SPELL";
                }
                break;
        }
    }

    public void OnMagicButton2()
    {
        EnableMagicButtons(false);
        EnableAllButtons(false);

        if (currentTurn == Turn.SAGE)
        {
            if(enemyController.IsPoisoned())
            {
                enemyController.StartCoroutine(enemyController.OnPoisoned());
                EnableAllButtons(true);
            }
            else
            {
                sageController.StartCoroutine(sageController.OnMagic(2));
                stateText.text = "FOREST POISON";
            }
            
        }
    }

    public void OnMagicButton3()
    {
        EnableMagicButtons(false);
        EnableAllButtons(false);

        if (currentTurn == Turn.SAGE)
        {
            sageController.StartCoroutine(sageController.OnMagic(3));
            stateText.text = "FOREST CONVERSION";
        }
    }

    public void OnPotion()
    {

        EnableAllButtons(false);
        EnableItemButtons(false);

        potionButtonSelected = true;

        EnableSelectionButtons(true);

    }



    public void OnMana()
    {
        EnableAllButtons(false);
        EnableItemButtons(false);

        manaButtonSelected = true;

        EnableSelectionButtons(true);
    }

    public void PaladinSelected()
    {

        EnableSelectionButtons(false);

        if (magicButtonSelected)
        {
            if(paladinController.IsGuardUp())
            {
                paladinController.StartCoroutine(paladinController.OnGuardUp());
                magicButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                valkyrieController.StartCoroutine(valkyrieController.OnMagic(1));
                stateText.text = "GUARD UP";
            }           
        }
        else if(potionButtonSelected)
        {
            if (paladinController.isHealthFull())
            {
                paladinController.StartCoroutine(paladinController.OnHealthFull());
                potionButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                paladinController.StartCoroutine(paladinController.OnPotion(4000));
            }
            
        }
        else if(manaButtonSelected)
        {
            if (paladinController.isMagicFull())
            {
                paladinController.StartCoroutine(paladinController.OnMagicFull());
                manaButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                paladinController.StartCoroutine(paladinController.OnMana(1000));
            }
        }    

    }

    public void ValkyrieSelected()
    {
        EnableSelectionButtons(false);

        if (magicButtonSelected)
        {
            if (valkyrieController.IsGuardUp())
            {
                valkyrieController.StartCoroutine(valkyrieController.OnGuardUp());
                magicButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                valkyrieController.StartCoroutine(valkyrieController.OnMagic(2));
                stateText.text = "GUARD UP";
            }
        }           
        else if(potionButtonSelected)
        {
            if(valkyrieController.isHealthFull())
            {
                valkyrieController.StartCoroutine(valkyrieController.OnHealthFull());
                potionButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                valkyrieController.StartCoroutine(valkyrieController.OnPotion(3250));
            }
            
        }
        else if(magicButtonSelected)
        {
            if (valkyrieController.isMagicFull())
            {
                valkyrieController.StartCoroutine(valkyrieController.OnMagicFull());
                manaButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                valkyrieController.StartCoroutine(valkyrieController.OnMana(1000));
            }
        }             
    }

    public void SageSelected()
    {
        EnableSelectionButtons(false);

        if (magicButtonSelected)
        {
            if (sageController.IsGuardUp())
            {
                sageController.StartCoroutine(sageController.OnGuardUp());
                magicButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                valkyrieController.StartCoroutine(valkyrieController.OnMagic(3));
                stateText.text = "ATTACK DOWN";
            }
        }
        else if (potionButtonSelected)
        {
            if (sageController.isHealthFull())
            {
                sageController.StartCoroutine(sageController.OnHealthFull());
                potionButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                sageController.StartCoroutine(sageController.OnPotion(2500));
            }

        }
        else if(manaButtonSelected)
        {
            if (sageController.isMagicFull())
            {
                sageController.StartCoroutine(sageController.OnMagicFull());
                manaButtonSelected = false;
                EnableAllButtons(true);
            }
            else
            {
                sageController.StartCoroutine(sageController.OnMana(4000));
            }
        }
    }

    public void SetForestConversion(bool check)
    {
        forestConversionOn = check;
    }

    public bool GetForestConversion()
    {
        return forestConversionOn;
    }

//--------------------------------------BUTTON ENABLING-------------------------------------------//

    private void EnableItemButtons(bool check)
    {
        potionButtonImage.enabled = check;
        manaButtonImage.enabled = check;
        potionButtonText.enabled = check;
        manaButtonText.enabled = check;
    }

    private void EnableMagicButtons(bool check)
    {
        switch (currentTurn)
        {
            case Turn.PALADIN:
                {
                    magic1ButtonText.text = "ATTACK DOWN";
                    magic1ButtonImage.enabled = check;
                    magic1ButtonText.enabled = check;
                }
                break;
            case Turn.VALKYRIE:
                {
                    magic1ButtonText.text = "GUARD UP";
                    magic1ButtonImage.enabled = check;
                    magic1ButtonText.enabled = check;
                }
                break;
            case Turn.SAGE:
                {
                    magic1ButtonText.text = "EARTH SPELL";
                    magic2ButtonText.text = "FOREST POISON";
                    magic3ButtonText.text = "FOREST CONVERSION";

                    magic1ButtonImage.enabled = check;
                    magic2ButtonImage.enabled = check;
                    magic3ButtonImage.enabled = check;
                    magic1ButtonText.enabled = check;
                    magic2ButtonText.enabled = check;
                    magic3ButtonText.enabled = check;
                }
                break;
        }
    }

    private void EnableSpecialButton(bool check)
    {
        special1Button.interactable = check;

        switch (currentTurn)
        {
            case Turn.PALADIN:
                {
                    special1ButtonText.text = "JUDGEMENT";
                }
                break;
            case Turn.VALKYRIE:
                {
                    special1ButtonText.text = "FALLING      STARS";
                }
                break;
            case Turn.SAGE:
                {
                    special1ButtonText.text = "FOREST CALLING";
                }
                break;
        }

        special1ButtonImage.enabled = check;
        special1ButtonText.enabled = check;
    }

    private void EnableAllButtons(bool check)
    {
        attackButton.interactable = check;
        magicButton.interactable = check;
        specialButton.interactable = check;
        itemButton.interactable = check;
    }

    private void EnableSelectionButtons(bool check)
    {
        selectText.text = "SELECT CHARACTER";


        selectText.enabled = check;

        paladinSelectImage.enabled = check;
        paladinSelectText.enabled = check;
        valkyrieSelectImage.enabled = check;
        valkyrieSelectText.enabled = check;
        sageSelectImage.enabled = check;
        sageSelectText.enabled = check;

    }

    private IEnumerator ShowPetrificationStatus(int type)
    {
        switch(type)
        {
            case 1:
            {
                selectText.text = "PALADIN IS PETRIFIED";
                selectText.enabled = true;
                yield return new WaitForSeconds(2);
                selectText.enabled = false;
                UpdateTurn();
                paladinController.SetIsPetrified(false);
            }
            break;

            case 2:
            {
                selectText.text = "VALKYRIE IS PETRIFIED";
                selectText.enabled = true;
                yield return new WaitForSeconds(2);
                selectText.enabled = false;
                UpdateTurn();
                valkyrieController.SetIsPetrified(false);
                
            }
            break;

            case 3:
            {
                selectText.text = "SAGE IS PETRIFIED";
                selectText.enabled = true;
                yield return new WaitForSeconds(2);
                selectText.enabled = false;
                UpdateTurn();
                sageController.SetIsPetrified(false);
            }
            break;
        }

        
    }

    public void EndGame()
    {
        EnableAllButtons(false);
        stateText.text = "GAME OVER \n" + "Rounds = "+ rounds + "\n";

    }

    private void FastPlay()
    {
        int chance;

        switch (currentTurn)
        {
            case Turn.PALADIN:
                {
                    if (paladinController.GetSpecial() == 5)
                        OnSpecial();
                    else
                    {
                        if(paladinController.GetHealth() < 1500)
                        {
                            paladinController.StartCoroutine(paladinController.OnPotion(3750));
                        }
                        chance = Random.Range(1, 3);
                        if (chance == 1)
                            paladinController.StartCoroutine(paladinController.OnMagic());
                        else
                            paladinController.StartCoroutine(paladinController.OnAttack());
                    }
                }
                break;
            case Turn.VALKYRIE:
                {
                    if (valkyrieController.GetSpecial() == 5)
                        OnSpecial();
                    else
                    {
                        if (valkyrieController.GetHealth() < 1500)
                        {
                            valkyrieController.StartCoroutine(valkyrieController.OnPotion(3750));
                        }
                        chance = Random.Range(1, 3);
                        if (chance == 1)
                            valkyrieController.StartCoroutine(valkyrieController.OnMagic(Random.Range(1,3)+1));
                        else
                            valkyrieController.StartCoroutine(valkyrieController.OnAttack());
                    }
                }
                break;
            case Turn.SAGE:
                {
                    if (sageController.GetSpecial() == 5)
                        OnSpecial();
                    else
                    {
                        if (sageController.GetHealth() < 1500)
                        {
                            sageController.StartCoroutine(sageController.OnPotion(3750));
                        }
                        chance = Random.Range(1, 3)+1;
                        if (chance == 1 || chance == 2)
                            sageController.StartCoroutine(sageController.OnMagic(Random.Range(1, 3) + 1));
                        else
                            sageController.StartCoroutine(sageController.OnAttack());
                    }
                }
                break;
        }
    }

}
