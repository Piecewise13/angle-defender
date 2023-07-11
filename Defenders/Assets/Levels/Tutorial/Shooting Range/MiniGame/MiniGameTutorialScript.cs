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

        [Header("Objects")]
        public List<GameObject> resources;
        public GameObject spawner;


        // Start is called before the first frame update
        void Start()
        {
            foreach (var item in resources)
            {
                item.SetActive(false);
            }
            spawner.SetActive(false);
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

            if (timedSteps.Contains(step))
            {
                isTimedStep = true;
                timedSteps.Remove(step);
                startStepTime = Time.time;
            }


            player.currentStep = step;
            tutorialText.text = textToDisplay[(int)step - (int)TUTORIAL_STEPS.EGG];

            if (step == TUTORIAL_STEPS.COLLECT_RESOURCES)
            {
                foreach (var item in resources)
                {
                    item.SetActive(true);
                }
            }

            if (step == TUTORIAL_STEPS.START_GAME)
            {
                spawner.SetActive(true);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag != "Player")
            {
                return;
            }

            player = other.GetComponentInParent<Tutorial_PlayerScript>();
            DisplayText(TUTORIAL_STEPS.EGG);
        }
    }
}