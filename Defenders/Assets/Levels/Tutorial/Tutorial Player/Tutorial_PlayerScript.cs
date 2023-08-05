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
        START_GAME,
        COLLECT_RESOURCES,
        CYCLE_MODE,
        BUILD_DEFENSES,
        ROTATE_DEFENSES1,
        PLACE_DEFENSE,
        ROTATE_DEFENSES2,
        START_GAME2,
        ENTER_TURRET,
        PLACE_TURRET,
        TOWER_MODE,
        PLACE_SENTRY,
        START_GAME3,
        TOWER_MODE2,
        WRENCH,
        THROW_WRENCH,
        FINAL_START

    }

}

