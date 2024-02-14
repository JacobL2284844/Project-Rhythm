using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Rigidbody targetRigidbody;
    public CinemachineFreeLook[] cameras;

    public CinemachineFreeLook cinemachineFL;
    public CinemachineFreeLook cinemachine_LockOn;

    public CinemachineFreeLook startCam;
    public CinemachineFreeLook currentCam;

    public float minFOV = 40f;
    public float maxFOV = 55f;
    public float minVelocity = 0f;
    public float maxVelocity = 10f;
    public float transitionSpeed = 5f; // Adjust the transition speed as needed

    private float currentVelocity = 0f;

    [Header("LockOnPresets")]
    [SerializeField] private bool lockedOn = false;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentCam = startCam;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] == currentCam)
            {
                cameras[i].Priority = 20;
            }
            else
            {
                cameras[i].Priority = 10;
            }
        }
    }
    void Update()
    {
        DynamicFOV();
    }

    private void DynamicFOV()
    {

        float targetNormalizedVelocity = Mathf.Clamp01((targetRigidbody.velocity.magnitude - minVelocity) / (maxVelocity - minVelocity));
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, targetNormalizedVelocity);

        // Smoothly transition the current FOV towards the target FOV
        float smoothedFOV = Mathf.SmoothDamp(currentCam.m_Lens.FieldOfView, targetFOV, ref currentVelocity, transitionSpeed * Time.deltaTime);

        currentCam.m_Lens.FieldOfView = smoothedFOV;
    }

    public void ToggleLockOn(InputAction.CallbackContext context)
    {
        //lockoncheck
        if(context.started)
        {
            if(lockedOn)
            {
                lockedOn = false;
                SwitchCamera(cinemachineFL);
            }
            else
            {
                lockedOn = true;
                SwitchCamera(cinemachine_LockOn);
            }
        }

    }
    public void SwitchCamera(CinemachineFreeLook newCamera)
    {
        currentCam = newCamera;

        currentCam.Priority = 20;
        for (int i = 0; i < cameras.Length; i++)
        {
            if(cameras[i] != currentCam)
            {
                cameras[i].Priority = 0;
            }
        }
    }
}
