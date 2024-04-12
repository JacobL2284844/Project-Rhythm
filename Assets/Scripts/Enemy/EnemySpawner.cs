using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyMaster enemyMaster;
    public GameObject[] enemiesToSpawn;
    public NavigationWanderPoints navigationWanderPoints;
    public int amountOfEnemys;
    bool spawnEnemysOnEntry = true;
    public List<GameObject> myActiveEnemies;
    public CameraController cameraController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyMaster.currentSpawnerInUse = this;

            if (spawnEnemysOnEntry)
            {
                spawnEnemysOnEntry = false;

                for (int i = 0; i < amountOfEnemys; i++)
                {
                    //spawn random enemy
                    GameObject enemy = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], navigationWanderPoints.GetRandomPoint().position, Quaternion.identity);

                    NPCStateManager stateManager = enemy.GetComponent<NPCStateManager>();

                    //set variables
                    stateManager.navigationWanderPoints = navigationWanderPoints;
                    stateManager.enemyMaster = enemyMaster;
                    stateManager.player = enemyMaster.player;
                    stateManager.playerHealth = enemyMaster.player.GetComponent<Health>();
                    stateManager.mySpawner = this;
                    enemyMaster.enemys.Add(stateManager);
                    myActiveEnemies.Add(enemy);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyMaster.currentSpawnerInUse = null;
        }
    }
    public void CheckBeat()//destroy spawner on beat
    {
        if (myActiveEnemies.Count == 0)
        {
            enemyMaster.currentSpawnerInUse = null;
            cameraController.SwitchCamera(cameraController.cinemachineFL);
            Destroy(gameObject);
        }
    }
}
