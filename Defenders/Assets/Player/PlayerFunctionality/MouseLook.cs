using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class MouseLook : MonoBehaviour
{
    public const float SENS_DEFAULT = 200;
    public Transform playerBody;
    public Transform cameraParent;
    private float xRotation;
    public float mouseSensitivity;
    public float aimSensitivty;

    public bool bUseAimSens;

    public float shakeDuration;

    public CameraShaker shaker;


    [SerializeField]private bool canLook = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if (canLook) {
            if (bUseAimSens)
            {
                float mouseX = Input.GetAxis("Mouse X") * aimSensitivty * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * aimSensitivty * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);

            } else {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);
            }




        }
    }

    public void setCanLook (bool value)
    {
        canLook = value;

    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = SENS_DEFAULT * value;
    }

    public void ShakeCamera(float magnitude)
    {
        shaker.ShakeOnce(magnitude / 5, 4f, .1f, 1);
    }
}
