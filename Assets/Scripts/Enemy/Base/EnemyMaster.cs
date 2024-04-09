using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour
{
    public List<NPCStateManager> enemys;
    public QuickTimeManager quickTimeManager;
    public Transform player;
    public EnemySpawner currentSpawnerInUse;

    public void UpdateAllEnemyBeatCheck()
    {
        foreach(NPCStateManager enemy in enemys)
        {
            enemy.LocalBeatCheck();
        }
        if(currentSpawnerInUse)
        {
            currentSpawnerInUse.CheckBeat();
        }
    }
}
