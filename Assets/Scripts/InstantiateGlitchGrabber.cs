using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGlitchGrabber : MonoBehaviour
{
    private Material glitchMaterial;
    // Start is called before the first frame update
    void Start()
    {
        glitchMaterial = GetComponent<MeshRenderer>().material;
        glitchMaterial.SetFloat("seed", Random.value * 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
