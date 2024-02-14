using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    [Header("Lock on Target")]
    [SerializeField] CameraController cameraController;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LockOnTarget(InputAction.CallbackContext context)
    {
        if (context.started)
        {
        }
    }
}
