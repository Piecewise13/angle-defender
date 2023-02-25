using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{

    private Renderer[] meshes;
    // Start is called before the first frame update

    private void Awake()
    {
        meshes = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaterials(Material mat)
    {
        foreach (Renderer render in meshes)
        {
            render.material = mat;
        }
    }
}
