using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
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
    [SerializeField] private int sensX_value;// stage in array
    [SerializeField] private int sensY_value; // stage in array
    [SerializeField] private float[] sensesArray_X; // for each set sensitivity value
    [SerializeField] private float[] sensesArray_Y;

    bool invertYAxis;
    public TextMeshProUGUI isInvertedText;

    [Header("Video")]
    public GameObject videoView;

    public GameObject fpsUI;
    [Header("Calibrate Delay")]
    public GameObject calibrationView;

    private void Awake()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1));

        sensX_value = PlayerPrefs.GetInt("SensX", sensX_value);
        sensY_value = PlayerPrefs.GetInt("SensY", sensY_value);

        sensX_slider.value = PlayerPrefs.GetFloat("SensX", sensX_value);
        sensY_slider.value = PlayerPrefs.GetFloat("SensY", sensY_value);

        SetSensitivityX(PlayerPrefs.GetFloat("SensX", sensX_value));
        SetSensitivityY(PlayerPrefs.GetFloat("SensY", sensX_value));

        if(PlayerPrefs.GetInt("SensYInverted", 0) == 0)
        {
            SetInvertYAxis(false);
            invertYAxis = false;
            isInvertedText.text = "No";
        }
        else
        {
            SetInvertYAxis(true);
            invertYAxis = true;
            isInvertedText.text = "Yes";
        }

        if (PlayerPrefs.GetInt("showFPS", 0) == 0)
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
    public void SetSensitivityX(float selectedSenseFloat)
    {
        sensX_value = Mathf.RoundToInt(selectedSenseFloat);

        if (cinemachineFreeLook)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = sensesArray_X[sensX_value];
        }
        PlayerPrefs.SetFloat("SensX", sensX_value);
    }
    public void SetSensitivityY(float selectedSenseFloat)
    {
        sensX_value = Mathf.RoundToInt(selectedSenseFloat);

        if (cinemachineFreeLook)
        {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = sensesArray_Y[sensY_value];
        }
        PlayerPrefs.SetFloat("SensY", sensY_value);
    }
    private void SetInvertYAxis(bool setIsInverted)
    {
        if (cinemachineFreeLook)
        {
            if (setIsInverted)
            {
                cinemachineFreeLook.m_YAxis.m_InvertInput = true;
                PlayerPrefs.SetInt("SensYInverted", 1);
                isInvertedText.text = "Yes";
                invertYAxis = true;
            }
            else
            {
                cinemachineFreeLook.m_YAxis.m_InvertInput = false;
                PlayerPrefs.SetInt("SensYInverted", 0);
                isInvertedText.text = "No";
                invertYAxis = false;
            }
        }
    }
    public void ToggleInvertYaxis()
    {
        if (invertYAxis)
        {
            SetInvertYAxis(false);
        }
        else
        {
            SetInvertYAxis(true);
        }
    }
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void ToggleFPS()
    {
        if (fpsUI.activeInHierarchy)
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
