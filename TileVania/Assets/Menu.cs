using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    GameObject gameSession;

    private void Start()
    {
        gameSession = GameObject.Find("Game Session");
    }

    public void FirstLevel() {
        
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        gameSession.GetComponent<GameSession>().LoadMainMenu();
    }
    

    
}
