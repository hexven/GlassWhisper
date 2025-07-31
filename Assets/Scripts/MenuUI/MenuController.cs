using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    private bool menuActivated;
    public GameObject ImageCanvas;
    public string MainMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
        ImageCanvas.SetActive(false);
    }

    public void BackToMainmenu()
    {
        SceneManager.LoadScene(MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && menuActivated)
        {
            Time.timeScale = 1;
            menuCanvas.SetActive(false);
            ImageCanvas.SetActive(false);
            menuActivated = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !menuActivated)
        {
            Time.timeScale = 0;
            menuCanvas.SetActive(true);
            ImageCanvas.SetActive(true);
            menuActivated = true;
        }
    }
}
