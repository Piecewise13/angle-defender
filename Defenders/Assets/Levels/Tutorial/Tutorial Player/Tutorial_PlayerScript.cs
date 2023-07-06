using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace Tutorial
{
    public class Tutorial_PlayerScript : PlayerScript, Damageable
    {

        [Space(20)]
        [Header("Tutorial Vars")]
        //[TextArea]
        public TUTORIAL_STEPS currentStep;
        public ShootingRangeTextScript shootingRangeScript;
        [TextArea]
        public string[] tutorialPrompts;


        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();

            shootingRangeScript = FindObjectOfType<ShootingRangeTextScript>();

        }
    }

    [System.Serializable]
    public enum TUTORIAL_STEPS
    {
        NONE,
        SHOOT,
        AIM,
        RELOAD,
        MOVEMENT,

    }

}

