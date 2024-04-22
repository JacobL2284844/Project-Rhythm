using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class MenuManager : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject deathMenu;
    public Camera camera;
    public bool menusActive;

    [Header("Playmode Pause Menu")]
    public bool gameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject pauseMenuMenu;
    public GameObject hud;
    public Button resumeButton;

    public BeatClicker beatClicker;
    private float musicCombatHealthValueDuringPlay = 100;

    [Header("Options")]
    public OptionsMenu optionsMenuManager;
    public GameObject optionsUI;
    private void Awake()
    {
        AudioManager.instance.SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1));
    }
    private void Start()
    {
        ResumeGame();
    }
    //button universal
    public void ButtonClick()
    {
        AudioManager.instance.PLayOneShot(AudioManager.instance.uiClickSound, camera.transform.position);
    }

    //pause menu
    public void PauseGame(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
        if (!gameIsPaused && context.started)
        {
            gameIsPaused = true;
            pauseMenu.SetActive(true);
            hud.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;


            resumeButton.Select();
            AudioManager.instance.PLayOneShot(AudioManager.instance.uiPauseSound, camera.transform.position);

            //music
            beatClicker.bgmEventInstance.getParameterByName("Health", out float value);
            musicCombatHealthValueDuringPlay = value;

            beatClicker.SetMusicParamaterCombat(0);
            return;
        }
        if (gameIsPaused && context.started)
        {
            ResumeGame();
        }
    }
    public void ResumeGame()
    {
        if (gameIsPaused)
        {
            gameIsPaused = false;
            pauseMenu.SetActive(false);
            hud.SetActive(true);
            HideOptions();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1.0f;
            AudioManager.instance.PLayOneShot(AudioManager.instance.uiBackSound, camera.transform.position);
            beatClicker.SetMusicParamaterCombat(musicCombatHealthValueDuringPlay);
        }
    }

    //options
    public void ShowOptions()
    {
        optionsUI.SetActive(true);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            pauseMenuMenu.SetActive(false);
        }
    }
    public void HideOptions()
    {
        optionsUI.SetActive(false);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            pauseMenuMenu.SetActive(true);
        }
    }

    //main menu
    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    //game death
    public void ShowDeathMenu()
    {
        deathMenu.SetActive(true);

        pauseMenu.SetActive(false);
        hud.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
}
