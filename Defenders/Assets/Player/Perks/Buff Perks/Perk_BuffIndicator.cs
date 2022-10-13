using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk_BuffIndicator : MonoBehaviour
{

    public Image[] indicatorImages;
    public Color nullColor;
    public Color checkColor;

    private int numChecked;

    // Start is called before the first frame update
    void Start()
    {
        indicatorImages = GetComponentsInChildren<Image>();

        for (int i = 0; i < indicatorImages.Length; i++)
        {
            indicatorImages[i].color = nullColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseIndicator()
    {
        indicatorImages[numChecked].color = checkColor;
        numChecked++;

    }



}
