using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tutorial
{
    public class ResetAreaScript : MonoBehaviour
    {
        [SerializeField] private Transform resetPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag == "Player")
            {
                other.transform.root.position = resetPoint.position;
            }
        }
    }
}
