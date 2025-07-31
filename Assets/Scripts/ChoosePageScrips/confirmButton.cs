using UnityEngine;
using UnityEngine.SceneManagement; // ต้องมีบรรทัดนี้เพื่อใช้ SceneManager

public class confirmButton : MonoBehaviour
{
    private bool clickIt;

    private void Awake()
    {
        clickIt = false;
    }

    public void clickOnButton()
    {
        clickIt = true;
        
        if (chooseSystem.chooseSelect == 1 && clickIt == true)
        {
            Debug.Log("คุณเลือกคนที่ " + chooseSystem.chooseSelect);
            SceneManager.LoadScene("Good Ending");
        }

        else if (chooseSystem.chooseSelect == 2 && clickIt == true)
        {
            Debug.Log("คุณเลือกคนที่ " + chooseSystem.chooseSelect);
            SceneManager.LoadScene("Bad Ending");
        }

        else if (chooseSystem.chooseSelect == 3 && clickIt == true)
        {
            Debug.Log("คุณเลือกคนที่ " + chooseSystem.chooseSelect);
            SceneManager.LoadScene("Bad Ending");
        }

        else if (chooseSystem.chooseSelect == 4 && clickIt == true)
        {
            Debug.Log("คุณเลือกคนที่ " + chooseSystem.chooseSelect);
            SceneManager.LoadScene("Bad Ending");
        }

        else if (chooseSystem.chooseSelect == 5 && clickIt == true)
        {
            Debug.Log("คุณเลือกคนที่ " + chooseSystem.chooseSelect);
            SceneManager.LoadScene("Bad Ending");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
