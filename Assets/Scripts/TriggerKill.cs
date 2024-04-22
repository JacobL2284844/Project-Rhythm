using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerKill : MonoBehaviour
{
    public float damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
