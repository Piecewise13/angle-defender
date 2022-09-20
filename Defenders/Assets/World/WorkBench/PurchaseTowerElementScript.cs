using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseTowerElementScript : MonoBehaviour
{

    public GameObject towerObject;
    public LongClickButton_Slider longClickButton;

    public Image panel;


    public Image imageIcon;
    public Sprite iconSprite;

    [Header("Cost Text")]
    public TMP_Text woodText;
    public TMP_Text ironText;
    public TMP_Text diamondText;


    [Header("Description Stuff")]
    public string towerName;
    [TextArea]
    public string towerDescription;


    [Header("Cost Values")]
    public int woodCost;
    public int ironCost;
    public int diamondCost;



    private int numMade;
    [SerializeField] private int tier;


    // Start is called before the first frame update
    void Start()
    {
        woodText.text = woodCost + "";
        ironText.text = ironCost + "";
        diamondText.text = diamondCost + "";
        longClickButton = GetComponent<LongClickButton_Slider>();


    }

    // Update is called once per frame
    void Update()
    {
        


    }


    public void LongClickDisabled()
    {
        StartCoroutine(CostTextFlash());
    }

    public bool CanAfford(PlayerScript player)
    {
        if (player.GetResourceAmount(ResourceType.Wood) >= woodCost &&
            player.GetResourceAmount(ResourceType.Iron) >= ironCost &&
            player.GetResourceAmount(ResourceType.Diamond) >= diamondCost)
        {
            return true;
        }
        return false;
    }

    public int GetNumMade()
    {
        return numMade;
    }

    public void AddNumMade()
    {
        numMade++;
    }

    public int GetTier()
    {
        return tier;
    }

    IEnumerator CostTextFlash()
    {
        woodText.color = Color.red;
        ironText.color = Color.red;
        diamondText.color = Color.red;
        yield return new WaitForSeconds(.5f);
        woodText.color = Color.white;
        ironText.color = Color.white;
        diamondText.color = Color.white;
        yield return new WaitForSeconds(.5f);
        woodText.color = Color.red;
        ironText.color = Color.red;
        diamondText.color = Color.red;
        yield return new WaitForSeconds(.5f);
        woodText.color = Color.white;
        ironText.color = Color.white;
        diamondText.color = Color.white;
        yield return new WaitForSeconds(.5f);
    }

}
