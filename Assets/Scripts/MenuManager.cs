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

    [Header("Playmode Pause Menu")]
    public bool gameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject hud;
    public Button resumeButton;

    [Header("Options")]
    public OptionsMenu optionsMenuManager;
    public GameObject optionsUI;
    private void Start()
    {
        ResumeGame();
    }
    //button universal
    public void ButtonSelect()
    {

    }
    public void ButtonClick()
    {

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

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1.0f;
        }
    }

    //options
    public void ShowOptions()
    {
        optionsUI.SetActive(true);
    }
    public void HideOptions()
    {
        optionsUI.SetActive(false);
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
}
