using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tutorial
{
    public class ShootingRangeTextScript : MonoBehaviour
    {

        private int textCount;

        [TextArea]
        public string[] textToDisplay;


        public TMP_Text tutorialText;
        public Transform canvasTransform;
        [Space(20)]
        [Header("Tutorial Text")]
        public float forwardDistance;
        public float rightDistance;
        public float upDistance;
        public float rotationSnappiness;

        private bool startTutorial;

        public GameObject miniGameObject;

        Tutorial_PlayerScript player;

        [Space(10)]
        public GameObject door;
        public float doorUpDistance;
        public float speed;
        bool textOverDoor = false;

        // Start is called before the first frame update
        void Start()
        {
            tutorialText.text = textToDisplay[0];
            miniGameObject.SetActive(false);
        }

        private void Update()
        {
            if (!startTutorial)
            {
                return;
            }

            if (textOverDoor)
            {
                canvasTransform.transform.position = Vector3.Lerp(canvasTransform.transform.position, door.transform.position + Vector3.up * doorUpDistance, speed * Time.deltaTime);
                canvasTransform.transform.LookAt(player.playerCamera.transform.position);
                return;
            }

            canvasTransform.position = player.transform.position + (player.playerCamera.transform.forward * forwardDistance
    + player.playerCamera.transform.right * rightDistance
    + player.playerCamera.transform.up * upDistance);

            canvasTransform.rotation = Quaternion.Lerp(canvasTransform.rotation, Quaternion.LookRotation(canvasTransform.position - player.playerCamera.transform.position, player.playerCamera.transform.up), rotationSnappiness * Time.deltaTime);
        }


        public void DisplayNextText(TUTORIAL_STEPS step)
        {
            if (!startTutorial)
            {
                return;
            }



            if ((int)player.currentStep + 1 != (int)step)
            {
                return;
            }
            player.currentStep = step;
            tutorialText.text = textToDisplay[(int)step];

            if (step == TUTORIAL_STEPS.MOVEMENT)
            {
                miniGameObject.SetActive(true);
                textOverDoor = true;
                tutorialText.transform.Rotate(Vector3.up * 180);
                return;
            }

            StartCoroutine(TextFlash());





        }

        IEnumerator TextFlash ()
        {
            tutorialText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
            tutorialText.gameObject.SetActive(true);
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag == "Player")
            {
                startTutorial = true;
                player = other.GetComponentInParent<Tutorial_PlayerScript>();
            }
        }

        public void HideText()
        {
            tutorialText.gameObject.SetActive(false);
        }
    }
}