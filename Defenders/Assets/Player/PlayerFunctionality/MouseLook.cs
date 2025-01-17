using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public const float SENS_DEFAULT = 350f;
    public Transform playerBody;
    public Transform cameraParent;
    private float xRotation;
    public float mouseSensitivity;
    public float aimSensitivty;

    public bool bUseAimSens;

    public float shakeDuration;


    [SerializeField]private bool canLook = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = SENS_DEFAULT;
        aimSensitivty = SENS_DEFAULT;
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

    public void CameraShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1f) * magnitude;
            float y = Random.Range(-1, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

}
