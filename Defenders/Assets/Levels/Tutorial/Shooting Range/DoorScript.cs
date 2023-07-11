using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial {
    public class DoorScript : MonoBehaviour
    {

        private bool hasPlayer;
        public GameObject door;

        Tutorial_PlayerScript player;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!hasPlayer)
            {
                return;
            }

            if (Input.GetButtonDown("Use"))
            {
                player.hudScript.StopDisplayingHint();
                player.currentStep = TUTORIAL_STEPS.USE;
                door.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag == "Player")
            {
                player = other.GetComponentInParent<Tutorial_PlayerScript>();

                player.hudScript.DisplayHint(PLAYER_HINT.USE);
                if (player.currentStep != TUTORIAL_STEPS.MOVEMENT)
                {
                    return;
                }
                hasPlayer = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.tag == "Player")
            {
                player = other.GetComponentInParent<Tutorial_PlayerScript>();

                player.hudScript.StopDisplayingHint();
                hasPlayer = false;
            }
        }
    }
}