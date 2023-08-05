using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicatorScript : MonoBehaviour
{
    public TMP_Text text;
    public float fadeInDur;
    public float fadeOutDur;
    public float stayTime;
    public float speed;
    private float startTime;

    private RectTransform rt;

    private Vector3 moveDir;
    public static float normalDist = 30;

    private Color critColor = new Color(0.9405066f, 0.03146139f, 0.9528302f);

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        moveDir = Vector3.up + transform.right * Random.Range(-1.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (stayTime + startTime < Time.time)
        {
            Destroy(gameObject);
        }
        else
        {
            rt.localPosition += moveDir * speed * Time.deltaTime;
            if (Time.time >  (startTime + stayTime) - fadeOutDur)
            {
                text.CrossFadeAlpha(0f, fadeOutDur, false);
            }

        }
    }

    public void SetDamage(float damage, float distance)
    {

        startTime = Time.time;
        text.text = damage + "";
        transform.localScale = Vector3.one * (distance / normalDist);
        speed *= (distance / normalDist);
        //text.CrossFadeAlpha(1, .1f, false);
    }

    public void IsCrit()
    {
        text.color = critColor;
    }
}
