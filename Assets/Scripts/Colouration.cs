using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colouration : MonoBehaviour
{
    public bool applyEnemyBonePhysics = false;

    public Material[] myMaterials;
    public MeshRenderer[] meshRenderers;
    public MeshRenderer enemyMask;
    void Start()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = myMaterials[Random.Range(0, myMaterials.Length)];
        }
    }

    public void ApplyDestructionToElements()
    {
        if (applyEnemyBonePhysics)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                AddPhysicsToEmptyMeshRenderer(meshRenderer);
            }
            AddPhysicsToEmptyMeshRenderer(enemyMask);
        }
    }

    private void AddPhysicsToEmptyMeshRenderer(MeshRenderer meshRenderer)
    {
        meshRenderer.transform.parent = null;

        meshRenderer.gameObject.AddComponent<MeshCollider>().convex = true;

        meshRenderer.gameObject.AddComponent<Rigidbody>();

        ScaleAndDisolve scaleAndDisolve = meshRenderer.gameObject.AddComponent<ScaleAndDisolve>();
        StartCoroutine(scaleAndDisolve.DisolveObject());
    }
}
