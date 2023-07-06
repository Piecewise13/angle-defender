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

        public TUTORIAL_STEPS currentStep;




        // Start is called before the first frame update
        void Start()
        {
            tutorialText.text = textToDisplay[0];
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