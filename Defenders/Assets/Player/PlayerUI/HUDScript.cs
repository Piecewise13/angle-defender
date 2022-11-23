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

    [Space(20)]
    [Header("Weapon Vars")]
    public Image primaryImage;
    public LayoutElement primaryElement;
    public Image secondaryImage;
    public LayoutElement secondaryElement;
    public Image specialImage;
    public LayoutElement specialElement;
    [Space(10)]
    public float bigWidth;
    public float bigHeight;
    [Space(10)]
    public float smallWidth;
    public float smallHeight;



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

    public void UpdateWeaponIcon(Sprite weaponIcon, int tier)
    {
        print("updating icon: " + tier);
        if (tier == 1)
        {
            primaryImage.sprite = weaponIcon;
        } else if (tier == 2){
            print("updating image");
            secondaryImage.sprite = weaponIcon;
        } else if(tier == 3)
        {
            specialImage.sprite = weaponIcon;
        }
    }

    public void UpdateEquipedWeapon(int tier)
    {
        if (tier == 1)
        {
            primaryElement.preferredHeight = bigHeight;
            primaryElement.preferredWidth = bigWidth;

            secondaryElement.preferredHeight = smallHeight;
            secondaryElement.preferredWidth = smallWidth;
            specialElement.preferredHeight = smallHeight;
            specialElement.preferredWidth = smallWidth;
        } else if (tier == 2)
        {
            primaryElement.preferredHeight = smallHeight;
            primaryElement.preferredWidth = smallWidth;

            secondaryElement.preferredHeight = bigHeight;
            secondaryElement.preferredWidth = bigWidth;

            specialElement.preferredHeight = smallHeight;
            specialElement.preferredWidth = smallWidth;
        } else
        {
            primaryElement.preferredHeight = smallHeight;
            primaryElement.preferredWidth = smallWidth;
            secondaryElement.preferredHeight = smallHeight;
            secondaryElement.preferredWidth = smallWidth;

            specialElement.preferredHeight = bigHeight;
            specialElement.preferredWidth = bigWidth;
        }
    }


    public IEnumerator CantAffordResourcesFlash()
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
        yield return null;
    }

}
