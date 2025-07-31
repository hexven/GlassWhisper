using UnityEngine;

public class cancelButton : MonoBehaviour
{
    public void clickOnButton()
    {
        if (chooseSystem.confirmScren == true)
        {
            chooseSystem.confirmScren = false;
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
