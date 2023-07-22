using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeValueScript : MonoBehaviour
{

    public TMP_Text text;
    public float fadeInDur;
    public float fadeOutDur;
    public float stayTime;
    public float speed;
    private float startTime = 0f;

    public Color negativeColor;
    public Color positiveColor;

    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + stayTime < Time.time)
        {

            Destroy(gameObject);
        }
        else
        {
            rt.localPosition += Vector3.up * speed * Time.deltaTime;
            if (stayTime - fadeOutDur < startTime)
            {
                text.CrossFadeAlpha(0f, fadeOutDur, false);
            }
        }
    }

    public void SetValue(int value)
    {

        if (value > 0)
        {
            text.color = positiveColor;
            text.text = "+" + value.ToString();

        }
        else
        {
            text.color = negativeColor;
            text.text = value.ToString();
        }
        startTime = Time.time;
        text.CrossFadeAlpha(1, .1f, false);
    }
}
