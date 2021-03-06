﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameSession : MonoBehaviour {

    private int playerLives = 5;
    private int playerScore = 0;  //MAGIC
    bool paused = false;
    bool canPause = false;
    bool originalExitHelpType = false;

    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    private void Start()
    {
        EstablishValues();

        print(SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            canPause = true;
            originalExitHelpType = true;
        }
    }

    public void EstablishValues()
    {
        int startLives = playerLives;
        int startScore = playerScore;
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) //MAGIC
        {
            canPause = true;
            originalExitHelpType = true;
        }
        else
        {
            canPause = false;
            originalExitHelpType = false;
        }
        Pause();
    }

    public void AddToScore(int pointsToAdd)
    {
        playerScore += pointsToAdd;
        scoreText.text = playerScore.ToString();
    }


    private void Awake()
    {
        int playerLivesAtStart = playerLives;
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void ResetGameSession()
    {
        SceneManager.LoadScene(3);
        Destroy(gameObject);
    }

    public void ResetCurrentLevel()
    {
        Destroy(GameObject.Find("Scene Persist"));

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject);
    }


    public void Pause()
    {
        if (CrossPlatformInputManager.GetButtonDown("Pause")) //Custom Pause Button
        {
            if (canPause)
            {
                if (!GameObject.Find("Instructions Page").GetComponent<Canvas>().enabled)
                {
                    if (paused)
                    {
                        TurnOffOptions();
                        paused = false;
                    }
                    else
                    {
                        TurnOnOptions();
                        paused = true;
                    }
                }
                else
                {
                    if (originalExitHelpType == true)
                    {
                        ExitHelp();
                    } else
                    {
                        
                        ExitHelp2();
                    }
                }
            } 

        }
    }

    public void ExitHelp2()
    {

        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = false;

        GameObject HelpCanvas = GameObject.Find("Instructions Page");
        HelpCanvas.GetComponent<Canvas>().enabled = false;

        if (GameObject.Find("Main Menu Canvas"))
        {
            GameObject TitleCanvas = GameObject.Find("Main Menu Canvas");
            TitleCanvas.GetComponent<Canvas>().enabled = true;
        }
    }

    public void ExitHelp3()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = false;

        GameObject HelpCanvas = GameObject.Find("Instructions Page");
        HelpCanvas.GetComponent<Canvas>().enabled = false;

        GameObject Title = GameObject.Find("Main Menu Canvas");
        Title.GetComponent<Canvas>().enabled = true;
    }

    public void ExitHelp()
    {

        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = true;

        GameObject HelpCanvas = GameObject.Find("Instructions Page");
        HelpCanvas.GetComponent<Canvas>().enabled = false;
    }


    public void TurnOnOptions()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = true;

        GameObject UICanvas = GameObject.Find("UI Canvas");
        UICanvas.GetComponent<Canvas>().enabled = false;

        Time.timeScale = 0;
    }


    public void TurnOffOptions()
    {
        GameObject PauseCanvas = GameObject.Find("Pause Canvas");
        PauseCanvas.GetComponent<Canvas>().enabled = false;

        GameObject UICanvas = GameObject.Find("UI Canvas");
        UICanvas.GetComponent<Canvas>().enabled = true;

        Time.timeScale = 1;
    }




    private void TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();

        FindObjectOfType<Player>().Closer();
    }

    public void LoadNextLevel()
    {
;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
       
        Destroy(gameObject);
    }


    public void PlayAgain()
    {
        playerLives = 3;
        playerScore = 0;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

}
