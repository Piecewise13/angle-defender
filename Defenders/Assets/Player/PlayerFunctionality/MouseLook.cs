using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class MouseLook : MonoBehaviour
{

    public Transform playerBody;
    public Transform cameraParent;
    private float xRotation;
    public float mouseSensitivity;

    public float shakeDuration;

    public CameraShaker shaker;


    private bool canLook = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if (canLook) {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);



        }
    }

    public void setCanLook (bool value)
    {
        canLook = value;

    }

    public void ShakeCamera(float magnitude)
    {
        shaker.ShakeOnce(magnitude / 5, 4f, .1f, 1);
    }

    public IEnumerator CameraShake(float magnitude)
    {
        Vector3 originalPos = cameraParent.localPosition;
        magnitude = Mathf.Lerp(.1f, 1f, magnitude / 30f);
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraParent.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraParent.localPosition = originalPos;
    }
}
