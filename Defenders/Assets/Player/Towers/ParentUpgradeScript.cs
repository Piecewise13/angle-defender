using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public abstract class ParentUpgradeScript : MonoBehaviour
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
    public LongClickButton_Image longClick;
    public Image panelImage;

    public abstract void UnlockUpgrade(PlayerScript player);

    public void Start()
    {
        costText = GetComponentInChildren<TMP_Text>();
        costText.text = soulFireCost + "";
        //panelImage = GetComponentInChildren<Image>();
        longClick = GetComponent<LongClickButton_Image>();
        SetUnavalible();
    }

    public void SetAvalible()
    {
        isAvailale = true;
        panelImage.color = new Color(.2f,.2f,.2f,.4f);
        panelImage.fillAmount = 0f;

    }

    public void SetUnavalible()
    {
        isAvailale = false;
        panelImage.color = new Color(.6f, .6f, .6f, .3f);
        panelImage.fillAmount = 1f;
        longClick.SetCanLongClick(false);

    }

    public void Unlocked()
    {
        isUnlocked = true;
        isAvailale = false;
        panelImage.fillAmount = 1f;
        panelImage.color = new Color(.1f, .1f, .1f, .4f);
        longClick.SetCanLongClick(true);

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
