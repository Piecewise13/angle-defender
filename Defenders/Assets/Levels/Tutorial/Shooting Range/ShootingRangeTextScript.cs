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

        private bool startTutorial;

        public GameObject miniGameObject;

        Tutorial_PlayerScript player;

        // Start is called before the first frame update
        void Start()
        {
            tutorialText.text = textToDisplay[0];
            miniGameObject.SetActive(false);
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
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag == "Player")
            {
                startTutorial = true;
                player = other.GetComponentInParent<Tutorial_PlayerScript>();
            }
        }
    }
}