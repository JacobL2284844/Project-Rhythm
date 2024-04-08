using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] myEnemys;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            foreach(GameObject enemy in myEnemys)
            {
                enemy.SetActive(true);
            }
        }
    }
}
