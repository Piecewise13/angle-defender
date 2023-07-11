using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class Tutorial_IngotScript : MonoBehaviour
    {
        public ResourceType type;
        public int amount;
        private static bool upgraded = false;

        SphereCollider sphereCollider;



        // Start is called before the first frame update
        void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
            if (upgraded)
            {
                sphereCollider.radius = 5f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //print(other.gameObject.transform.root.gameObject.tag);
            if (!other.transform.root.tag.Equals("Player"))
            {
                return;
            }

            Tutorial_PlayerScript player = other.gameObject.GetComponentInParent<Tutorial_PlayerScript>();
            player.ChangeDiamondAmount(amount);
            player.miniGameScript.DisplayText(TUTORIAL_STEPS.COLLECT_RESOURCES + 1);
            Destroy(gameObject);
        }
    }
}