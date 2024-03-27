using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour
{
    public NPCStateManager[] enemys;
    public QuickTimeManager quickTimeManager;

    public void UpdateAllEnemyBeatCheck()
    {
        foreach(NPCStateManager enemy in enemys)
        {
            enemy.LocalBeatCheck();
        }
    }
}
