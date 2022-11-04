using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSwitchScript : MonoBehaviour
{

    int currentIndex = 0;
    public GameObject[] menus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchMenu(int i)
    {
        if (i == currentIndex)
        {
            return;
        }
        foreach (var item in menus)
        {
            item.SetActive(false);
        }

        menus[i].SetActive(true);

    }

}
