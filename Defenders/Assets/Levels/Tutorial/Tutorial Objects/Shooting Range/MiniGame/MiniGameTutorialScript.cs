using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Tutorial
{
    public class MiniGameTutorialScript : MonoBehaviour
    {

        private Tutorial_PlayerScript player;


        private int textCount;

        [TextArea]
        public string[] textToDisplay;

        public TMP_Text tutorialText;
        public Transform canvasTransform;


        public TUTORIAL_STEPS currentStep;

        [Space(20)]
        [Header("Tutorial Text")]
        public float forwardDistance;
        public float rightDistance;
        public float upDistance;
        [Space(5)]
        public float rotationSnappiness;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (player)
            {
                canvasTransform.position = player.transform.position + (player.transform.forward * forwardDistance
                    + player.transform.right * rightDistance
                    + Vector3.up * upDistance);

                canvasTransform.rotation = Quaternion.Lerp(canvasTransform.rotation, Quaternion.LookRotation(canvasTransform.position - player.transform.position), 10f * Time.deltaTime);
                //canvasTransform.LookAt(player.transform.position);
            }


        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag != "Player")
            {
                return;
            }

            player = other.GetComponentInParent<Tutorial_PlayerScript>();
        }

        public void DisplayNextText(TUTORIAL_STEPS step)
        {
            if ((int)currentStep < (int)step)
            {
                currentStep = step;
                tutorialText.text = textToDisplay[(int)step];
            }
        }
    }
}