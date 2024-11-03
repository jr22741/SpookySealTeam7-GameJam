using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Singleplayer()
    {
        Debug.Log("Loading single player");
        SceneManager.LoadScene(1);
    }

    public void Coop()
    {
        Debug.Log("Loading coop");
        SceneManager.LoadScene(2);
    }

    public void Versus()
    {
        Debug.Log("Loading versus");
        SceneManager.LoadScene(3);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void Update()
    {
        
    }
}
