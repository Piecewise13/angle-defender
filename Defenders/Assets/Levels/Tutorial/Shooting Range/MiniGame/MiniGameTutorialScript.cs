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

        [Space(20)]
        [Header("Tutorial Text")]
        public float forwardDistance;
        public float rightDistance;
        public float upDistance;

        [Space(5)]
        public float rotationSnappiness;

        [Space(5)]
        [SerializeField] private float stepTime;
        private float startStepTime;
        bool isTimedStep;
        public List<TUTORIAL_STEPS> timedSteps;

        private bool hasPlayer = false;

        [Header("Objects")]
        public List<GameObject> resources;
        public Tutorial_MasterAI spawner;
        private ShootingRangeTextScript shootRange;

        bool textFlash = true;


        // Start is called before the first frame update
        void Start()
        {
            foreach (var item in resources)
            {
                item.SetActive(false);
            }
            spawner.gameObject.SetActive(false);
            player = FindObjectOfType<Tutorial_PlayerScript>();
            tutorialText.gameObject.SetActive(false);
            shootRange = FindObjectOfType<ShootingRangeTextScript>();
        }

        // Update is called once per frame
        void Update()
        {

            if (hasPlayer)
            {
                canvasTransform.position = player.transform.position + (player.transform.forward * forwardDistance
                    + player.transform.right * rightDistance
                    + Vector3.up * upDistance);

                canvasTransform.rotation = Quaternion.Lerp(canvasTransform.rotation, Quaternion.LookRotation(canvasTransform.position - player.transform.position), rotationSnappiness * Time.deltaTime);
                //canvasTransform.LookAt(player.transform.position);
            }

            if (!isTimedStep)
            {
                return;
            }

            if (startStepTime + stepTime < Time.time)
            {
                isTimedStep = false;
                DisplayText(player.currentStep + 1);

            }
        }

        public void DisplayText(TUTORIAL_STEPS step)
        {
            if ((int)player.currentStep + 1 != (int)step)
            {
                return;
            }
            if (textFlash)
            {
                StartCoroutine(TextFlash());
            }


            if (timedSteps.Contains(step))
            {
                isTimedStep = true;
                timedSteps.Remove(step);
                startStepTime = Time.time;
            }




            if (step == TUTORIAL_STEPS.COLLECT_RESOURCES)
            {
                foreach (var item in resources)
                {
                    item.SetActive(true);
                }
            }
            if (step == TUTORIAL_STEPS.START_GAME || step == TUTORIAL_STEPS.START_GAME2 || step == TUTORIAL_STEPS.START_GAME3 || step == TUTORIAL_STEPS.FINAL_START)
            {
                spawner.gameObject.SetActive(true);
                spawner.ShowStartSign();
                
            }

            player.currentStep = step;
            tutorialText.text = textToDisplay[(int)step - (int)TUTORIAL_STEPS.EGG];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag != "Player")
            {
                return;
            }
            shootRange.HideText();
            //tutorialText.gameObject.SetActive(true);
            hasPlayer = true;
            DisplayText(TUTORIAL_STEPS.EGG);
            tutorialText.gameObject.SetActive(true);
        }

        IEnumerator TextFlash()
        {
            tutorialText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
            tutorialText.gameObject.SetActive(true);
            print("Show text");

        }

        public void HideText()
        {
            tutorialText.gameObject.SetActive(false);
            textFlash = false;
        }

        public void ShowText()
        {
            tutorialText.gameObject.SetActive(true);
            textFlash = true;
        }
    }
}