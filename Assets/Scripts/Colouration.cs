using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colouration : MonoBehaviour
{
    public Material[] myMaterials;
    public MeshRenderer[] meshRenderers;
    void Start()
    {
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = myMaterials[Random.Range(0, myMaterials.Length)];
        }
    }
}
