using UnityEngine;
using System.Collections;

public class Characters : MonoBehaviour {


    [Header("CONSTANT STATS")]
    public int MAX_HEALTH;
    public int MAX_MAGIC;
    public int MAX_SPECIAL = 5;
    public int STRENGTH;
    public int AGILITY;
    public int SPIRIT;
    public int DEFENCE;
    [Header("Spell Costs")]
    public int spell1;
    public int spell2;
    public int spell3;

    [HideInInspector]
    public int attackPower;
    [HideInInspector]
    public int magicPower;
    [HideInInspector]
    public int defencePower;
    [HideInInspector]
    public int dodge;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public int magic;
    [HideInInspector]
    public int special = 5;

    [HideInInspector]
    public bool b_isDead;
    [HideInInspector]
    public bool b_isWounded;


//----------Colours-----------//

    [HideInInspector]
    protected Color healthGreen = new Color(0.0f / 255.0f, 255.0f / 255.0f, 128.0f / 255.0f);
    [HideInInspector]
    protected Color manaBlue = new Color(0.0f / 255.0f, 223.0f / 255.0f, 255.0f / 255.0f);
    [HideInInspector]
    protected Color transparent = new Vector4(1f, 1f, 1f, 0f);
    [HideInInspector]
    protected Color clear = new Vector4(1f, 1f, 1f, 1f);

//----------Images-----------//
    protected SpriteRenderer spriteImage;
    [HideInInspector]
    protected Transform TR_magic;
    [HideInInspector]
    protected SpriteRenderer magicImage;

    [HideInInspector]
    protected Transform TR_special;
    [HideInInspector]
    protected SpriteRenderer specialImage;

    [HideInInspector]
    protected Transform TR_petrified;
    [HideInInspector]
    protected SpriteRenderer petrifiedImage;

    [HideInInspector]
    protected GameObject GO_guardUp;
    [HideInInspector]
    protected SpriteRenderer guardUpImage;

    [HideInInspector]
    protected Transform TR_win;
    [HideInInspector]
    protected SpriteRenderer winImage;

    [HideInInspector]
    protected Transform TR_wounded;
    [HideInInspector]
    protected SpriteRenderer woundedImage;

    [HideInInspector]
    protected Transform TR_dead;
    [HideInInspector]
    protected SpriteRenderer deadImage;

//----------Sounds-----------//
    [HideInInspector]
    protected AudioSource takeDamageSound;
    [HideInInspector]
    protected AudioSource attackSound;
    [HideInInspector]
    protected AudioSource magicSound;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
