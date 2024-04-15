using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    //button universal
    public void ButtonSelect()
    {

    }
    public void ButtonClick()
    {

    }

    //pause menu
    public void PauseGame()
    {

    }
    public void ResumeGame()
    {

    }

    //options
    public void ShowOptions()
    {

    }
    public void HideOptions()
    {

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
