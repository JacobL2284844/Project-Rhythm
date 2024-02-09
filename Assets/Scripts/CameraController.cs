using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody targetRigidbody;
    public CinemachineFreeLook cinemachineFL;

    public float minFOV = 40f;
    public float maxFOV = 55f;
    public float minVelocity = 0f;
    public float maxVelocity = 10f;
    public float transitionSpeed = 5f; // Adjust the transition speed as needed

    private float currentVelocity = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        float smoothedFOV = Mathf.SmoothDamp(cinemachineFL.m_Lens.FieldOfView, targetFOV, ref currentVelocity, transitionSpeed * Time.deltaTime);

        cinemachineFL.m_Lens.FieldOfView = smoothedFOV;
    }
}
