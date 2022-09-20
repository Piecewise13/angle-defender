using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public abstract class ParentPerkScript : MonoBehaviour
{

    public int id;
    public bool isUnlocked = false;
    public bool isAvailale = false;
    public string perkName;
    [TextArea]public string perkDescription;
    //public int woodCost;
    //public int ironCost;
    //public int diamondCost;
    public int soulFireCost;

    public TMP_Text costText;
    private Image panelImage;

    public abstract void UnlockUpgrade(PlayerScript player);

    public void Start()
    {
        panelImage = GetComponent<Image>();
        SetUnavalible();
    }

    public void SetAvalible()
    {
        isAvailale = false;
        costText.text = soulFireCost + "";

    }

    public void SetUnavalible()
    {
        isAvailale = false;


    }

    public void Unlocked()
    {
        isUnlocked = true;
        panelImage.color = new Color(100,100,100, 255);

    }

    public IEnumerator CostTextFlash()
    {
        costText.color = Color.red;
        yield return new WaitForSeconds(.5f);
        costText.color = Color.white;
        yield return new WaitForSeconds(.5f);
        costText.color = Color.red;
        yield return new WaitForSeconds(.5f);
        costText.color = Color.white;
    }

}
