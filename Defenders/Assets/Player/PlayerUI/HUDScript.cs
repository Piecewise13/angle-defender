using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{

    private PlayerScript player;

    [Header("Resource Variables")]
    public TMP_Text woodText;
    public TMP_Text ironText;
    public TMP_Text diamondText;
    [Space(10)]
    public float fadeInDur;
    public float fadeOutDur;

    [Space(10)]
    public RectTransform woodChangePos;
    public RectTransform ironChangePos;
    public RectTransform diamondChangePos;
    public GameObject changeValue;

    



    [Header("Player Variables")]
    public TMP_Text currentHealth;
    public TMP_Text maxHealth;
    public TMP_Text currentBullets;
    public TMP_Text maxBullets;

    public TMP_Text bulletCarried;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        UpdateResourceValues();
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBulletValues(int maxNum, int currentNum)
    {
        currentBullets.text = currentNum.ToString();
        maxBullets.text = "/" + maxNum.ToString();
        UpdateBulletValues();

    }
    public void UpdateBulletValues()
    {
        bulletCarried.text = BasicWeaponScript.NumBulletsCarried().ToString();
        

    }

    public void UpdateHealth()
    {
        maxHealth.text = "/" + player.maxHealth.ToString();
        currentHealth.text =  (player.health.ToString());
    }


    public void UpdateResourceValues()
    {

        woodText.text = player.getResourceAmount(ResourceType.Wood).ToString();
        ironText.text = player.getResourceAmount(ResourceType.Iron).ToString();
        diamondText.text = player.getResourceAmount(ResourceType.Diamond).ToString();
    }

    public void BulletsChangeFade(int amount)
    {
        Instantiate(changeValue, bulletCarried.rectTransform.position, Quaternion.Euler(Vector3.zero), gameObject.transform).GetComponent<ChangeValueScript>().SetValue(amount);
    }

    public void ResoucesChangeFade(ResourceType resource, int amount)
    {
        switch (resource)
        {
            case ResourceType.Wood:
                Instantiate(changeValue, woodChangePos.position, Quaternion.Euler(Vector3.zero), gameObject.transform).GetComponent<ChangeValueScript>().SetValue(amount);

                break;
            case ResourceType.Iron:
                Instantiate(changeValue, ironChangePos.position, Quaternion.Euler(Vector3.zero), gameObject.transform).GetComponent<ChangeValueScript>().SetValue(amount);
                break;
            case ResourceType.Diamond:
                Instantiate(changeValue, diamondChangePos.position, Quaternion.Euler(Vector3.zero), gameObject.transform).GetComponent<ChangeValueScript>().SetValue(amount);
                break;
        }
    }
}
