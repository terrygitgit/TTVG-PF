using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Menu : MonoBehaviour {

    bool paused = false;
    bool help = false;

    static bool helpDealtWith = true;

    GameObject gameSession;

    public void ExitHelp()
    {

        gameSession.GetComponent<GameSession>().ExitHelp();
    }

    public void Help()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = false;

        GameObject HelpCanvas = GameObject.Find("Instructions Page");
        HelpCanvas.GetComponent<Canvas>().enabled = true;
    }



    public void FirstLevel() {
        gameSession.GetComponent<GameSession>().TurnOffOptions();
        SceneManager.LoadScene(1);
    }

    public void ResetCurrentLevel()
    {
        gameSession.GetComponent<GameSession>().TurnOffOptions();
        gameSession.GetComponent<GameSession>().ResetCurrentLevel();
    }

    private void Start()
    {

        gameSession = GameObject.Find("Game Session");
        gameSession.GetComponent<GameSession>().TurnOffOptions();

        

        if (SceneManager.GetActiveScene().buildIndex == 1)  //MAGIC
        {
            GameObject UICanvas = GameObject.Find("UI Canvas");
            UICanvas.GetComponent<Canvas>().enabled = true;
        }



    }

    public void QuitGame()
    {
        gameSession.GetComponent<GameSession>().TurnOffOptions();
        Application.Quit();
    }


    


    public void LoadMainMenu()
    {
        gameSession.GetComponent<GameSession>().TurnOffOptions();
        gameSession.GetComponent<GameSession>().LoadMainMenu();
    }
    

    
}
