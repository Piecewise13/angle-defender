using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDeathScreen : MonoBehaviour
{
    public Image panel;

    public TMP_Text countdown;

    private PlayerScript player;

    float countdownTimer;
    float countdownStart;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerScript>();   
    }

    // Update is called once per frame
    void Update()
    {
        countdown.text = Mathf.CeilToInt(player.GetRespawnTime() - (Time.time - countdownStart)).ToString();
    }

    private void OnEnable()
    {
        countdownStart = Time.time;
    }
}
