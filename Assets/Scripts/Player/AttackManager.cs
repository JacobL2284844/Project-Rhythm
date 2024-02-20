using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    [Header("Lock on Target")]
    [SerializeField] CameraController cameraController;
    public Transform currentEnemyTarget;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLockOnTarget(Transform targetEnemy)
    {
        currentEnemyTarget = targetEnemy;
    }
}
