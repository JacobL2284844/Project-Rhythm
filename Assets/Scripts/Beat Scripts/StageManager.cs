using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;
using FMODUnity;

public class StageManager : MonoBehaviour
{
    public BeatClicker beatClicker; // Reference to the BeatClicker script
    public GameObject stage1Visuals; // Visuals for Stage 1
    public GameObject stage2Visuals; // Visuals for Stage 2
    public GameObject stage3Visuals; // Visuals for Stage 3
    public GameObject stage4Visuals; // Visuals for Stage 4

    private bool stage1ActionExecuted = false;
    private bool stage2ActionExecuted = false;
    private bool stage3ActionExecuted = false;
    private bool stage4ActionExecuted = false;

    [FMODUnity.EventRef]
    public string bgmEventPath = "event:/LEVEL 1/BGM/BGM"; // FMOD event path

    private FMOD.Studio.EventInstance bgmEventInstance; // FMOD event instance

    void Start()
    {
        // Check all visuals are initially inactive
        SetAllVisualsInactive();

        // Initialize FMOD event instance
        bgmEventInstance = FMODUnity.RuntimeManager.CreateInstance(bgmEventPath);
        bgmEventInstance.start(); // Start the FMOD event
    }

    void Update()
    {
        // Update visuals based on the current stage
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        // Deactivate all stage visuals first
        SetAllVisualsInactive();

        // Activate visuals for all stages up to the current stage
        switch (beatClicker.currentStage)
        {
            case BeatClicker.Stage.Stage1:
                stage1Visuals.SetActive(true);
                if (!stage1ActionExecuted)
                {
                    Stage1Action();
                    stage1ActionExecuted = true;
                    stage2ActionExecuted = false;
                    stage3ActionExecuted = false;
                    stage4ActionExecuted = false;
                }
                break;
            case BeatClicker.Stage.Stage2:
                stage1Visuals.SetActive(true);
                stage2Visuals.SetActive(true);
                if (!stage2ActionExecuted)
                {
                    stage1ActionExecuted = false;
                    Stage2Action();
                    stage2ActionExecuted = true;
                    stage3ActionExecuted = false;
                    stage4ActionExecuted = false;
                }
                break;
            case BeatClicker.Stage.Stage3:
                stage1Visuals.SetActive(true);
                stage2Visuals.SetActive(true);
                stage3Visuals.SetActive(true);
                if (!stage3ActionExecuted)
                {
                    stage1ActionExecuted = false;
                    stage2ActionExecuted = false;
                    Stage3Action();
                    stage3ActionExecuted = true;
                    stage4ActionExecuted = false;
                }
                break;
            case BeatClicker.Stage.Stage4:
                stage1Visuals.SetActive(true);
                stage2Visuals.SetActive(true);
                stage3Visuals.SetActive(true);
                stage4Visuals.SetActive(true);
                if (!stage4ActionExecuted)
                {
                    stage1ActionExecuted = false;
                    stage2ActionExecuted = false;
                    stage3ActionExecuted = false;
                    Stage4Action();
                    stage4ActionExecuted = true;
                }
                break;
        }
    }

    void SetAllVisualsInactive()
    {
        // Deactivate all stage visuals
        stage1Visuals.SetActive(false);
        stage2Visuals.SetActive(false);
        stage3Visuals.SetActive(false);
        stage4Visuals.SetActive(false);
    }

    // Methods for actions specific to each stage
    void Stage1Action()
    {
        // Add actions for Stage 1 here
        Debug.Log("Stage 1 Action");

        // Set parameter for Stage 1
        bgmEventInstance.setParameterByName("COMBOS", 0f); // Change the value as needed
    }

    void Stage2Action()
    {
        // Add actions for Stage 2 here
        Debug.Log("Stage 2 Action");

        // Set parameter for Stage 2
        bgmEventInstance.setParameterByName("COMBOS", 2f); // Change the value as needed
    }

    void Stage3Action()
    {
        // Add actions for Stage 3 here
        Debug.Log("Stage 3 Action");

        // Set parameter for Stage 3
        bgmEventInstance.setParameterByName("COMBOS", 2f); // Change the value as needed
    }

    void Stage4Action()
    {
        // Add actions for Stage 4 here
        Debug.Log("Stage 4 Action");

        // Set parameter for Stage 4
        bgmEventInstance.setParameterByName("COMBOS", 2f); // Change the value as needed
    }

    // Call this method when the stage manager is destroyed
    private void OnDestroy()
    {
        // Release the FMOD event instance
        bgmEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // Stop the FMOD event with fade out
        bgmEventInstance.release();
    }
}
