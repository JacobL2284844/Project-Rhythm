using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class OptionsMenu : MonoBehaviour
{
    public MenuManager sceneMenuManager;
    [Header("Audio")]
    public GameObject audioView;
    public float masterVolume = 1;
    public Slider masterVolumeSlider;

    [Header("Controls")]
    public GameObject controlsView;
    public CinemachineFreeLook cinemachineFreeLook;
    public Slider sensX_slider;
    public Slider sensY_slider;

    [Header("Video")]
    public GameObject videoView;

    public GameObject fpsUI;
    [Header("Calibrate Delay")]
    public GameObject calibrationView;

    private void Awake()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1));

        sensX_slider.value = PlayerPrefs.GetFloat("SensX", 600);
        sensY_slider.value = PlayerPrefs.GetFloat("SensY", 2);
        SetSensitivityX(PlayerPrefs.GetFloat("SensX", 600));
        SetSensitivityY(PlayerPrefs.GetFloat("SensY", 2));

        if(PlayerPrefs.GetInt("showFPS", 0) == 0)
        {
            fpsUI.SetActive(false);
        }
        else
        {
            fpsUI.SetActive(true);
        }
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
    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetMasterVolume(value);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    //sensitivity
    public void SetSensitivityX(float value)
    {
        if (cinemachineFreeLook)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = value;
        }
        PlayerPrefs.SetFloat("SensX", value);
    }
    public void SetSensitivityY(float value)
    {
        if (cinemachineFreeLook)
        {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = value;
        }
        PlayerPrefs.SetFloat("SensY", value);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void ToggleFPS()
    {
        if(fpsUI.activeInHierarchy)
        {
            fpsUI.SetActive(false);
        }
        else
        {
            fpsUI.SetActive(true);
        }
    }
    public void ShowUI_AudioView()
    {
        audioView.SetActive(true);
        controlsView.SetActive(false);
        calibrationView.SetActive(false);
        videoView.SetActive(false);
    }
    public void ShowUI_ControlsView()
    {
        audioView.SetActive(false);
        controlsView.SetActive(true);
        calibrationView.SetActive(false);
        videoView.SetActive(false);
    }
    public void ShowUI_VideoView()
    {
        audioView.SetActive(false);
        controlsView.SetActive(false);
        calibrationView.SetActive(false);
        videoView.SetActive(true);
    }
    public void ShowUI_CalibrationView()
    {
        audioView.SetActive(false);
        controlsView.SetActive(false);
        calibrationView.SetActive(true);
        videoView.SetActive(false);
    }

    public void CloseOptions()
    {
        sceneMenuManager.HideOptions();
    }
}
