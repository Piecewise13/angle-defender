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
    public TMP_Text healthText;
    //public Slider soulFireSlider;
    public TMP_Text soulFireText;
    private float previousSoulFire;


    [Header("Egg Vars")]
    public  Slider eggHealthSlider;

    public static EggScript egg;

    [Space(20)]
    [Header("Mode Vars")]
    public Transform modeTransform;
    public GameObject weaponsIcon;
    public GameObject buildingIcon;
    public GameObject towerIcon;
    private RectTransform currentRect;
    Vector3 currentInitPos;
    [SerializeField] private int modeSpace;
    [SerializeField] private float modeAnimationTime;
    [SerializeField] private float modeAnimationSpeed;


    // Start is called before the first frame update
    void Start()
    {

        player = GetComponentInParent<PlayerScript>();
        egg = FindObjectOfType<EggScript>();
        UpdateResourceValues();
        UpdateSoulFireValues();
        UpdateHealth();
        currentRect = weaponsIcon.GetComponent<RectTransform>();
        currentInitPos = currentRect.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSoulFireValues()
    {

        //soulFireSlider.value = (float)player.GetSoulFire() / (float)player.GetSoulFireMax();
        soulFireText.text = player.GetSoulFire() + "";

    }

    public void UpdateHealth()
    {
        healthSlider.value = player.health / player.maxHealth;
        healthText.text = player.health + " / " + player.maxHealth;
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

    public void UpdatePlayerMode(PlayerMode newMode)
    {
        StartCoroutine(PlayerModeSwitchAnimation(newMode));
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

    private IEnumerator PlayerModeSwitchAnimation(PlayerMode newMode)
    {
        bool hasSwitched = false;

        //RectTransform top = weaponsIcon.GetComponent<RectTransform>();
        //RectTransform middle = towerIcon.GetComponent<RectTransform>();
        //RectTransform bottom = buildingIcon.GetComponent<RectTransform>();

        //RectTransform weapons = weaponsIcon.GetComponent<RectTransform>();
        //RectTransform tower = towerIcon.GetComponent<RectTransform>();
        //RectTransform building = buildingIcon.GetComponent<RectTransform>();

        RectTransform moveRect = null;


        switch (newMode)
        {
            case PlayerMode.Weapons:
                //middle = weaponsIcon.GetComponent<RectTransform>();
                //bottom = towerIcon.GetComponent<RectTransform>();
                //top = buildingIcon.GetComponent<RectTransform>();
                moveRect = weaponsIcon.GetComponent<RectTransform>();
                break;
            case PlayerMode.Building:
                //middle = buildingIcon.GetComponent<RectTransform>();
                //bottom = weaponsIcon.GetComponent<RectTransform>();
                //top = towerIcon.GetComponent<RectTransform>();
                moveRect = buildingIcon.GetComponent<RectTransform>();
                break;
            case PlayerMode.Tower:
                //middle = towerIcon.GetComponent<RectTransform>();
                //bottom = buildingIcon.GetComponent<RectTransform>();
                //top = weaponsIcon.GetComponent<RectTransform>();
                moveRect = towerIcon.GetComponent<RectTransform>();
                break;
        }

        //Vector3 topPosition = top.localPosition;
        //Vector3 middlePosition = middle.localPosition;
        //Vector3 bottomPosition = bottom.localPosition;
        
        moveRect.localPosition = currentInitPos + Vector3.up * modeSpace;


        float timer = 0;
        while (timer < modeAnimationTime)
        {

            currentRect.localPosition = Vector3.Lerp(currentInitPos, currentInitPos + Vector3.down * modeSpace, timer / modeAnimationTime);
            moveRect.localPosition = Vector3.Lerp(currentInitPos + Vector3.up * modeSpace, currentInitPos, timer / modeAnimationTime);

            //top.localPosition = Vector3.Lerp(topPosition, middlePosition, timer / modeAnimationTime);
            //middle.localPosition = Vector3.Lerp(middlePosition, bottomPosition, timer / modeAnimationTime);
            //bottom.localPosition = Vector3.Lerp(bottomPosition, bottomPosition + Vector3.down * modeSpace, timer / modeAnimationTime);

            timer += Time.deltaTime;
            yield return null;
        }
        currentRect = moveRect;
        //bottom.localPosition = topPosition;

        yield return null;

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
