using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton_Slider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool pointerDown;
    private float pointerDownTimer;
    public float pointerDownRequiredTime;

    public Slider holdSlider;

    public UnityEvent onLongClick;
    public UnityEvent onShortClick;

    [HideInInspector]public bool canLongClick = true;
    private bool messageSent;

    // Start is called before the first frame update
    void Start()
    {
        holdSlider = GetComponentInChildren<Slider>();
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
                    holdSlider.value = Mathf.Clamp(pointerDownTimer / pointerDownRequiredTime, 0, 1f);
                    pointerDownTimer += Time.deltaTime;
                }
            } else
            {
                if (!messageSent)
                {
                    gameObject.SendMessage("LongClickDisabled");
                    messageSent = true;
                }

            }

        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        pointerDownTimer = 0f;
        pointerDown = true;
        onShortClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
        pointerDown = false;
    }

    private void Reset()
    {

        messageSent = false;
        holdSlider.value = 0f;
        pointerDownTimer = 0f;
    }

}
