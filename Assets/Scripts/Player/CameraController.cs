using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Main")]
    public Rigidbody targetRigidbody;
    public CinemachineFreeLook[] cameras;

    public CinemachineFreeLook cinemachineFL;

    public CinemachineFreeLook startCam;
    public CinemachineFreeLook currentCam;

    [Header("Dynamic FOV")]
    public float hardmaxMinFOV = 20f;
    public float minFOV = 40f;
    public float maxFOV = 55f;
    public float definateMinFOV = 25f;
    public float minVelocity = 0f;
    public float maxVelocity = 10f;
    public float transitionSpeed = 5f; // Adjust the transition speed as needed

    private float currentVelocity = 0f;

    [Header("LockOnPresets")]
    public Transform targetToLock;
    [SerializeField] private EnemyChecker enemyChecker;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    public bool lockedOn = false;
    public CinemachineFreeLook cinemachine_LockOn;
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private int currentEnemyLockedIndex;

    [Header("Post-Processing")]
    [SerializeField] private PostProcessVolumeControl postProcessControll;
    private void Start()
    {
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

        if (currentCam.m_Lens.FieldOfView >= definateMinFOV)
        {
            // Smoothly transition the current FOV towards the target FOV
            float smoothedFOV = Mathf.SmoothDamp(currentCam.m_Lens.FieldOfView, targetFOV, ref currentVelocity, transitionSpeed * Time.deltaTime);

            currentCam.m_Lens.FieldOfView = smoothedFOV;
        }
        if (currentCam.m_Lens.FieldOfView < hardmaxMinFOV)
        {
            currentCam.m_Lens.FieldOfView = hardmaxMinFOV;
        }
    }

    public void ToggleLockOn(InputAction.CallbackContext context)
    {
        //lockoncheck
        if (context.started)
        {
            if (lockedOn)
            {
                lockedOn = false;
                attackManager.currentEnemyTarget = null;

                SwitchCamera(cinemachineFL);
            }
            else
            {
                SetTargetClosestAndLockOn();
            }
        }
    }
    public void SetTargetClosestAndLockOn()
    {
        if (TargetClosestEnemy() != null)
        {
            targetToLock = TargetClosestEnemy();
            targetGroup.m_Targets[1].target = targetToLock;
            currentEnemyLockedIndex = enemyChecker.enemiesInRange.IndexOf(attackManager.currentEnemyTarget);

            lockedOn = true;
            SwitchCamera(cinemachine_LockOn);
        }
    }
    public Transform TargetClosestEnemy()
    {
        Transform closestEnemie = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = targetRigidbody.transform.position;

        foreach (Transform enemie in enemyChecker.enemiesInRange)
        {
            if (enemie != null)
            {
                float distance = Vector3.Distance(enemie.transform.position, currentPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemie = enemie;
                }
            }
        }

        attackManager.SetLockOnTarget(closestEnemie);//set target in attak manager
        return closestEnemie;
    }
    public void TargetNextInList(InputAction.CallbackContext context)
    {
        if (targetToLock != null && context.started)
        {
            currentEnemyLockedIndex = enemyChecker.enemiesInRange.IndexOf(attackManager.currentEnemyTarget);

            currentEnemyLockedIndex++;

            if (currentEnemyLockedIndex == enemyChecker.enemiesInRange.Count)
            {
                currentEnemyLockedIndex = 0;
            }
            targetToLock = enemyChecker.enemiesInRange[currentEnemyLockedIndex];
            targetGroup.m_Targets[1].target = targetToLock;
            attackManager.SetLockOnTarget(targetToLock);
        }
    }
    public void TargetPreviusInList(InputAction.CallbackContext context)
    {
        if (targetToLock != null && context.started)
        {
            currentEnemyLockedIndex = enemyChecker.enemiesInRange.IndexOf(attackManager.currentEnemyTarget);

            currentEnemyLockedIndex--;

            if (currentEnemyLockedIndex < 0)
            {
                currentEnemyLockedIndex = enemyChecker.enemiesInRange.Count - 1;
            }
            targetToLock = enemyChecker.enemiesInRange[currentEnemyLockedIndex];
            targetGroup.m_Targets[1].target = targetToLock;
            attackManager.SetLockOnTarget(targetToLock);
        }
    }
    public void SwitchCamera(CinemachineFreeLook newCamera)
    {
        //fade vignette
        if (newCamera == cinemachineFL)
        {
            postProcessControll.FadeOutVignette();
        }
        else if (newCamera == cinemachine_LockOn)
        {
            postProcessControll.FadeInVignette();
        }

        currentCam = newCamera;

        currentCam.Priority = 20;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != currentCam)
            {
                cameras[i].Priority = 0;
            }
        }
    }
}
