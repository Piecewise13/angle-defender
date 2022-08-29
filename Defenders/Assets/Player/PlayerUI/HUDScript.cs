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
    public Slider healthSlider;
    public Slider soulFireSlider;
    private float previousSoulFire;


    [Header("Egg Vars")]
    public  Slider eggHealthSlider;

    public static EggScript egg;


    // Start is called before the first frame update
    void Start()
    {

        player = GetComponentInParent<PlayerScript>();
        egg = FindObjectOfType<EggScript>();
        UpdateResourceValues();
        UpdateSoulFireValues();
        UpdateHealth();
        UpdateEggValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSoulFireValues()
    {

        soulFireSlider.value = (float)player.GetSoulFire() / (float)player.GetSoulFireMax();

    }

    public void UpdateHealth()
    {
        healthSlider.value = player.health / player.maxHealth;
    }


    public void UpdateResourceValues()
    {

        woodText.text = player.GetResourceAmount(ResourceType.Wood).ToString();
        ironText.text = player.GetResourceAmount(ResourceType.Iron).ToString();
        diamondText.text = player.GetResourceAmount(ResourceType.Diamond).ToString();
    }


    public void UpdateEggValues()
    {
        print("updating");
        print(egg.maxHealth);
        eggHealthSlider.value = egg.health / egg.maxHealth;
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
