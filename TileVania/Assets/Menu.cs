using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    GameObject gameSession;

    private void Start()
    {
        gameSession = GameObject.Find("Game Session");
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject SettingsUI = GameObject.Find("Settings UI");
            SettingsUI.GetComponent<Canvas>().enabled = true;
        }

    }

    public void FirstLevel() {
        TurnOffOptions();
        SceneManager.LoadScene(1);
    }

    public void ResetCurrentLevel()
    {
        TurnOffOptions();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }


    public void TurnOnOptions()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = true;

        GameObject UICanvas = GameObject.Find("UI Canvas");
        UICanvas.GetComponent<Canvas>().enabled = false;

        GameObject SettingsUI = GameObject.Find("Settings UI");
        SettingsUI.GetComponent<Canvas>().enabled = false;

        Time.timeScale = 0;
    }


    public void TurnOffOptions()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = false;

        GameObject UICanvas = GameObject.Find("UI Canvas");
        UICanvas.GetComponent<Canvas>().enabled = true;

        GameObject SettingsUI = GameObject.Find("Settings UI");
        SettingsUI.GetComponent<Canvas>().enabled = true;

        Time.timeScale = 1;
    }


    public void QuitGame()
    {
        TurnOffOptions();
        Application.Quit();
    }


    public void Help()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = false;

        GameObject HelpCanvas = GameObject.Find("Instructions Page");
        HelpCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void ExitHelp()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = true;

        GameObject HelpCanvas = GameObject.Find("Instructions Page");
        HelpCanvas.GetComponent<Canvas>().enabled = false;
    }


    public void LoadMainMenu()
    {
        TurnOffOptions();
        gameSession.GetComponent<GameSession>().LoadMainMenu();
    }
    

    
}
