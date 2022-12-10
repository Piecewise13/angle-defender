using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseHUD : MonoBehaviour
{

    public Image fillImage;
    public TMP_Text costText;
    private PurchaseableParent purchaseScript;
    private Transform playerTransform;
    //private Canvas canvas;


    // Start is called before the first frame update
    void Start()
    {
        purchaseScript = GetComponentInParent<PurchaseableParent>();
        costText.text = purchaseScript.GetCost() + "";
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - playerTransform.position);
    }

    public void Setup(PlayerScript player)
    {
        playerTransform = player.transform;
        fillImage.fillAmount = 0f;
        //canvas.worldCamera = player.playerCamera;
    }

    public void UpdateFillAmount(float amount)
    {
        fillImage.fillAmount = amount;
    }
    
}
