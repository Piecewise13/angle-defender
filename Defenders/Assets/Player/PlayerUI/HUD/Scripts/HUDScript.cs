using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{

    public PlayerScript player;

    [Header("Resource Variables")]
    public TMP_Text diamondText;
    public TMP_Text soulFireText;

    [Space(10)]
    public RectTransform diamondChangePos;
    public RectTransform soulFireChangePos;
    public GameObject changeValue;

    [Space(10)]
    public float fadeInDur;
    public float fadeOutDur;



    [Header("Player Variables")]
    public Slider healthSlider;
    public TMP_Text healthText;
    //public Slider soulFireSlider;

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
    [Space(15)]
    public GameObject entityCostObject;
    public TMP_Text costValue;
    public Image costIcon;
    public Sprite soulFireIcon;
    public Sprite resourceIcon;

    [Space(20)]
    [Header("Weapon Vars")]
    public Image primaryImage;
    public Image secondaryImage;
    public Image specialImage;
    [Space(5)]
    public Transform selectedWeaponImage;
    [Space(10)]
    public TMP_Text bulletsLeft;
    public TMP_Text clipSizeText;

    public TMP_Text damageMultiplier;

    [Space(10)]
    [Header("DefenseVars")]
    public Image wallImage;
    public Image towerImage;
    public Image ladderImage;
    [Space(5)]
    public GameObject selectedDefenseImage;

    [Space(5)]
    [Header("Misc Weapon Vars")]
    public GameObject damageIndicator;
    public Transform damageIndicatorSpawn;

    [Space(10)]
    [Header("Mode Info Panels")]
    public GameObject weaponsModeInformation;
    public GameObject defenseModeInformation;
    public Animator informationPanelAnim;
    public float switchTime;
    private float switchTimer;
    private bool shouldSwitch;
    private Transform informationPanel;

    [Space(20)]
    [Header("Game Data")]
    public TMP_Text roundsCounter;
    public TMP_Text enemiesCounter;

    [Space(20)]
    [Header("Hint Panel")]
    public GameObject hintPanel;
    public GameObject costHint;
    public GameObject useHint;
    public GameObject rotateHint;

    [Space(20)]
    [Header("Abilities")]
    public Image normalAbilityIcon;
    public Image specialAbilityIcon;
    public Image ultimateAbilityIcon;

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
        UpdateEquipedWeapon(1);
    }

    public void Update()
    {
        //doesn't work
        if (!shouldSwitch)
        {
            return;
        }

        if (switchTimer <= switchTime)
        {
            print(informationPanel);
            informationPanel.localScale.Set(Mathf.Lerp(0f, 1f, switchTimer / switchTime), 1f, 1f);
            switchTimer += Time.deltaTime;
        } else
        {
            
            shouldSwitch = false;
            print("End Switch");
        } 
        
    }

    public void ChangeNormalAbilityImage(Sprite icon)
    {
        normalAbilityIcon.sprite = icon;
    }




    public void ChangeSpecialAbilityImage(Sprite icon)
    {
        specialAbilityIcon.sprite = icon;
    }

    public void UpdateSpecialAbilityCooldown(float percent)
    {
        specialAbilityIcon.fillAmount = percent;
    }

    public void ChangeUltimateAbilityImage(Sprite icon)
    {
        ultimateAbilityIcon.sprite = icon;

    }

    public void UpdateUltimateAbilityCharge(float percent)
    {
        ultimateAbilityIcon.fillAmount = percent;
    }

    #region PLAYER VALUES
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
        diamondText.text = player.GetDiamondAmount() + "";
    }

    #endregion

    #region INDICATOR FUNCTIONS

    public void DisplayHint(PLAYER_HINT hint)
    {
        
        switch (hint)
        {
            case PLAYER_HINT.USE:
                useHint.SetActive(true);
                break;
            case PLAYER_HINT.COST:
                costHint.SetActive(true);
                break;
            case PLAYER_HINT.ROTATE:
                rotateHint.SetActive(true);
                break;
            default:
                break;
        }
        hintPanel.SetActive(true);
    }

    public void StopDisplayingHint()
    {
        useHint.SetActive(false);
        costHint.SetActive(false);
        //rotateHint.SetActive(false);
        hintPanel.SetActive(false);
    }

    //Used for when player picks up new resources or gets more soul fire
    public void SpawnResoucesChangeIndicator(bool isDiamond, int amount)
    {
        if (amount == 0 )
        {
            return;
        }

        if (isDiamond)
        {
            Instantiate(changeValue, diamondChangePos.position, Quaternion.Euler(Vector3.zero), gameObject.transform).GetComponent<ChangeValueScript>().SetValue(amount);
            return;
        }

        Instantiate(changeValue, soulFireChangePos.position, Quaternion.Euler(Vector3.zero), gameObject.transform).GetComponent<ChangeValueScript>().SetValue(amount);

    }


    //TODO MAKE A COOL ANIMATION OR SOMETHING HERE
    public void UpdatePlayerMode(PlayerMode newMode)
    {
        informationPanelAnim.SetTrigger("ModeSwitch");
        switch (newMode)
        {
            case PlayerMode.Weapons:
                weaponsIcon.SetActive(true);
                towerIcon.SetActive(false);
                weaponsModeInformation.SetActive(true);
                informationPanel = weaponsModeInformation.transform;
                break;
            case PlayerMode.Defense:
                weaponsIcon.SetActive(false);
                buildingIcon.SetActive(true);
                weaponsModeInformation.SetActive(false);
                defenseModeInformation.SetActive(true);
                informationPanel = defenseModeInformation.transform;

                break;
            case PlayerMode.Tower:
                towerIcon.SetActive(true);
                buildingIcon.SetActive(false);
                defenseModeInformation.SetActive(false);
                break;
            default:
                break;
        }


    }

    public void PlacingEntity(bool isSoulFire, int cost)
    {
        if (isSoulFire)
        {
            costIcon.sprite = soulFireIcon;
        }
        else
        {
            costIcon.sprite = resourceIcon;
        }

        costValue.text = "" + cost;

    }

    public void StopPlacingEntity()
    {
        entityCostObject.SetActive(false);
    }

    public IEnumerator CantAffordResourcesFlash()
    {
        diamondText.color = Color.red;
        yield return new WaitForSeconds(.5f);
        diamondText.color = Color.white;
        yield return new WaitForSeconds(.5f);
        diamondText.color = Color.red;
        yield return new WaitForSeconds(.5f);
        diamondText.color = Color.white;
        yield return null;
    }

    #endregion

    #region WEAPONS INFORMATION
    //Updates the weapon for a specified tier
    public void UpdateWeaponIcon(Sprite weaponIcon, int tier)
    {
        if (tier == 1)
        {
            primaryImage.sprite = weaponIcon;
        }
        else if (tier == 2)
        {
            secondaryImage.sprite = weaponIcon;
        }
        else if (tier == 3)
        {
            specialImage.sprite = weaponIcon;
        }
    }

    //Changes which weapon is equiped and show be shown as such in the HUD
    public void UpdateEquipedWeapon(int tier)
    {
        
        switch (tier)
        {
            case 1:
                selectedWeaponImage.transform.position = primaryImage.transform.position;
                break;
            case 2:
                selectedWeaponImage.transform.position = secondaryImage.transform.position;
                break;
            case 3:
                selectedWeaponImage.transform.position = specialImage.transform.position;
                break;
            default:
                break;
        }
        
    }

    //Changes the clip size for when a new weapon is equiped
    public void UpdateClipSize(int number)
    {
        clipSizeText.text = "" + number;
    }

    //Updates the bullet counter
    public void UpdateBulletCount(int count)
    {
        bulletsLeft.text = "" + count;
    }
    #endregion

    public void ChangeEquipedDefense(int defense)
    {
        switch (defense)
        {
            case 0:
                selectedDefenseImage.transform.position = wallImage.transform.position;
                break;
            case 1:
                selectedDefenseImage.transform.position = towerImage.transform.position;
                break;
            case 2:
                selectedDefenseImage.transform.position = ladderImage.transform.position;
                break;
            default:
                break;
        }

    }

    public void UpdateDamageMultiplier(float multiplier)
    {
        damageMultiplier.text = "x" + multiplier;
    }

    #region GAME VALUES
    public void UpdateEggValues()
    {

        eggHealthSlider.value = egg.health / egg.maxHealth;
    }


    public void UpdateRoundsCounter(int number)
    {
        roundsCounter.text = "" + number;
    }


    public void UpdateEnemiesCounter(int number)
    {
        enemiesCounter.text = "" + number;
    }
    #endregion

    //I LIKE THE WAY IT SWITCHES BUT MAKE IT SWITCH SMOOTHER
    /*
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
                weaponsModeInformation.SetActive(true);
                defenseModeInformation.SetActive(false);
                break;
            case PlayerMode.Building:
                //middle = buildingIcon.GetComponent<RectTransform>();
                //bottom = weaponsIcon.GetComponent<RectTransform>();
                //top = towerIcon.GetComponent<RectTransform>();
                moveRect = buildingIcon.GetComponent<RectTransform>();
                weaponsModeInformation.SetActive(false);
                defenseModeInformation.SetActive(true);
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
    */





}
