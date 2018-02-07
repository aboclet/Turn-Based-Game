using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHover : MonoBehaviour {


    public Text infoText;

	public void OnAttackHover()
    {
        infoText.text = "PRESS TO ATTACK";
        infoText.enabled = true;     
    }

    public void ExitAttackHover()
    {
        infoText.enabled = false;
    }

    public void OnSpecialHover()
    {
        infoText.text = "SPECIAL ABILITY";
        infoText.enabled = true;
    }

    public void ExitSpecialHover()
    {
        infoText.enabled = false;
    }

    public void OnItemHover()
    {
        infoText.text = "VIEW ITEMS";
        infoText.enabled = true;
    }

    public void ExitItemHover()
    {
        infoText.enabled = false;
    }

    public void OnMagicHover()
    {
        infoText.text = "VIEW MAGIC SPELLS";
        infoText.enabled = true;
    }

    public void ExitMagicHover()
    {
        infoText.enabled = false;
    }
}
