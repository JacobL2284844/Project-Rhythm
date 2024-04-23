using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAndDisolve : MonoBehaviour
{
    [Header("Disolve")]
    public bool disolveOnEnable = true;//turn of for enemy
    public float durationAlive = 5;
    public float disolveDuration = 3;
    public bool destroyGameobject = true;
    public Vector3 targetScale = new Vector3(0.01f, 0.01f, 0.01f);

    [Header("Force and direction")]
    public bool useForce = false;
    public Vector3 forceDirection = Vector3.forward;
    public float forceMagnitude = 10f;
    private void OnEnable()
    {
        if (disolveOnEnable)
        {
            StartCoroutine(DisolveObject());
        }
    }
    public IEnumerator DisolveObject()
    {
        yield return new WaitForSeconds(durationAlive);

        float timeElapsed = 0f;
        Vector3 startingScale = transform.localScale;

        if (useForce)
        {//fix
            GetComponent<Rigidbody>().AddForce(forceDirection.normalized * forceMagnitude, ForceMode.Impulse);
        }

        while (timeElapsed < disolveDuration)
        {
            timeElapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startingScale, targetScale, timeElapsed / disolveDuration);
            yield return null;
        }

        transform.localScale = targetScale;

        if (destroyGameobject)
        {
            Destroy(gameObject);
        }
    }
}