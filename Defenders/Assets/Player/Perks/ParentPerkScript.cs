using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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

    public Button perkButton;

    public abstract void UnlockUpgrade(PlayerScript player);

    public void Start()
    {
        perkButton = GetComponent<Button>();
        SetUnavalible();
    }

    public void SetAvalible()
    {
        isAvailale = false;
        perkButton.interactable = true; 
    }

    public void SetUnavalible()
    {
        isAvailale = false;
        perkButton.interactable = false;


    }


}
