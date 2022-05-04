using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScript : MonoBehaviour
{

    public Material validPlacement;
    public Material invalidPlacement;

    MeshRenderer meshMat;

    // Start is called before the first frame update
    void Awake()
    {
        meshMat = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void validMaterial()
    {
        meshMat.material = validPlacement;

    }

    public void invalidMaterial()
    {
        meshMat.material = invalidPlacement;

    }
}
