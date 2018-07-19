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

    public void ExitHelp2()
    {

        
    }

    public void ExitHelp()
    {
        float scenes = SceneManager.GetActiveScene().buildIndex;
        if (scenes == 0)
        {
            gameSession.GetComponent<GameSession>().ExitHelp3();
        }
        else if (scenes == 1) // MAGIC
        {
            gameSession.GetComponent<GameSession>().ExitHelp();
        }
        else
        {

            gameSession.GetComponent<GameSession>().ExitHelp2();
        }
    }

    public void Help()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = false;

        GameObject HelpCanvas = GameObject.Find("Instructions Page");
        HelpCanvas.GetComponent<Canvas>().enabled = true;

        if (GameObject.Find("Main Menu Canvas"))
        {
            GameObject Title = GameObject.Find("Main Menu Canvas");
            Title.GetComponent<Canvas>().enabled = false;
        }

    }
    

    public void FirstLevel() {
        gameSession.GetComponent<GameSession>().TurnOffOptions();
        
        SceneManager.LoadScene(1);
        print("what");
        gameSession.GetComponent<GameSession>().EstablishValues();
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
        else
        {
            GameObject UICanvas = GameObject.Find("UI Canvas");
            UICanvas.GetComponent<Canvas>().enabled = false;
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
