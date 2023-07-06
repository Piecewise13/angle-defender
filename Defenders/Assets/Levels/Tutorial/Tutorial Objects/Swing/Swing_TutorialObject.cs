using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Tutorial
{
    public class Swing_TutorialObject : MonoBehaviour
    {

        public Transform textTransform;

        Tutorial_PlayerScript player;

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Tutorial_PlayerScript>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 towardPlayerDir = (player.transform.position - transform.position).normalized.xz3();
            Vector3 location = transform.position + Vector3.Cross(towardPlayerDir, Vector3.up) * 2.5f;
            textTransform.position = location;
            textTransform.LookAt(player.transform.position);
            textTransform.Rotate(0f, 180f, 0f);
        }
    }
}