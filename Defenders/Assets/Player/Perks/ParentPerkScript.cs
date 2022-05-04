using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParentPerkScript : MonoBehaviour
{

    public int id;
    public bool unlocked;
    public string perkName;
    [TextArea]public string perkDescription;
    public int woodCost;
    public int ironCost;
    public int diamondCost;

    public abstract void UnlockUpgrade();


}
