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

        [Header("Tutorial Sections")]
        public ShootingRangeTextScript shootingRangeScript;
        public MiniGameTutorialScript miniGameScript;

        [TextArea]
        public string[] tutorialPrompts;


        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();

            shootingRangeScript = FindObjectOfType<ShootingRangeTextScript>();
            miniGameScript = FindObjectOfType<MiniGameTutorialScript>();       
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
        USE,
        EGG,
        EGG_POWERS,
        COLLECT_RESOURCES,
        BUILD_DEFENSES,
        ROTATE_DEFENSES1,
        PLACE_DEFENSE,
        ROTATE_DEFENSES2,
        START_GAME

    }

}

