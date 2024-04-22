using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public MenuManager sceneMenuManager;
    [Header("Audio")]
    public GameObject audioView;
    public float masterVolume = 1;
    public Slider masterVolumeSlider;

    [Header("Controls")]
    public GameObject controlsView;
    [Header("Calibrate Delay")]
    public GameObject calibrationView;

    private void Awake()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1));
    }
    private void OnEnable()
    {
        ShowUI_ControlsView();
    }
    private void OnDisable()
    {
        ShowUI_ControlsView();
    }
    //audio
    public void ShowUI_AudioView()
    {
        audioView.SetActive(true);
        controlsView.SetActive(false);
        calibrationView.SetActive(false);
    }
    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetMasterVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
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
