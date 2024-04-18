using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public MenuManager sceneMenuManager;
    [Header("Audio")]
    public GameObject audioView;
    [Header("Controls")]
    public GameObject controlsView;
    [Header("Calibrate Delay")]
    public GameObject calibrationView;

    private void OnEnable()
    {
        ShowUI_ControlsView();
    }
    private void OnDisable()
    {
        ShowUI_ControlsView();
    }

    public void ShowUI_AudioView()
    {
        audioView.SetActive(true);
        controlsView.SetActive(false);
        calibrationView.SetActive(false);
    }

    public void ShowUI_ControlsView()
    {
        audioView.SetActive(false);
        controlsView.SetActive(true);
        calibrationView.SetActive(false);
    }

    public void ShowUI_CalibrationView()
    {
        audioView.SetActive(false);
        controlsView.SetActive(false);
        calibrationView.SetActive(true);
    }

    public void CloseOptions()
    {
        sceneMenuManager.HideOptions();
    }
    public void SaveChanges()
    {

    }
}
