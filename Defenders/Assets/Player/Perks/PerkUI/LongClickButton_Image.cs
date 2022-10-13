using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton_Image : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool pointerDown;
    private float pointerDownTimer;
    public float pointerDownRequiredTime;

    public Image holdImage;

    public UnityEvent onLongClick;
    public UnityEvent onShortClick;

    public bool canLongClick = false;
    private bool shouldReset = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pointerDown)
        {
            if (canLongClick)
            {
                if (pointerDownTimer >= pointerDownRequiredTime)
                {

                    onLongClick.Invoke();

                    Reset();
                }
                else
                {
                    holdImage.fillAmount = Mathf.Clamp(pointerDownTimer / pointerDownRequiredTime, 0, 1f);
                    //holdSlider.value = Mathf.Clamp(pointerDownTimer / pointerDownRequiredTime, 0, 1f);
                    pointerDownTimer += Time.deltaTime;
                }
            }

        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        print("pointer down");

        pointerDownTimer = 0f;
        pointerDown = true;
        onShortClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (canLongClick)
        {
            Reset();
        }

        pointerDown = false;
        print("pointer up");
    }

    private void Reset()
    {
        if (shouldReset)
        {


            holdImage.fillAmount = 0f;
            //holdSlider.value = 0f;
            pointerDownTimer = 0f;
        }
        
    }

    public void DisableLongClick()
    {
        canLongClick = false;
        holdImage.fillAmount = 1f;
        shouldReset = false;

    }
}
