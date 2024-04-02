using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine;

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

    void Start()
    {
        // Check all visuals are initially inactive
        SetAllVisualsInactive();
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
    }

    void Stage2Action()
    {
        // Add actions for Stage 2 here
        Debug.Log("Stage 2 Action");
    }

    void Stage3Action()
    {
        // Add actions for Stage 3 here
        Debug.Log("Stage 3 Action");
    }

    void Stage4Action()
    {
        // Add actions for Stage 4 here
        Debug.Log("Stage 4 Action");
    }
}
