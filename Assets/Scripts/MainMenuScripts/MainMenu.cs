using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string Cutscene;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Cutscene);
        
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
    

}
