using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettings : MonoBehaviour
{
    private PlayerScript player;

    [Header("Sensitivity")]
    public Slider sensSlider;
    public TMP_Text sensText;
    public float sensMax;
    private float sensValue;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SensitivitySlider()
    {
        sensValue = Mathf.Lerp(0.10f, sensMax, sensSlider.value);
        sensText.text = string.Format("{0:0.00}", sensValue);
        player.lookScript.SetSensitivity(sensValue);
    }

}
