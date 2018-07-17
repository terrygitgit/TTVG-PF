using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    [SerializeField] GameObject gameSession;

    public void FirstLevel() {

        gameSession.GetComponent<GameSession>().PlayAgain();
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        gameSession.GetComponent<GameSession>().LoadMainMenu();
    }
    

    
}
