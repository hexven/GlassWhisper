using UnityEngine;

public class clickOnPicture : MonoBehaviour
{
    
    public void clicking()
    {
        if (gameObject.CompareTag("Suspect1"))
        {
            chooseSystem.chooseSelect = 1;
            chooseSystem.confirmScren = true;
            Debug.Log("You choose " + chooseSystem.chooseSelect);
        }

        else if (gameObject.CompareTag("Suspect2"))
        {
            chooseSystem.chooseSelect = 2;
            chooseSystem.confirmScren = true;
            Debug.Log("You choose " + chooseSystem.chooseSelect);
        }

        else if (gameObject.CompareTag("Suspect3"))
        {
            chooseSystem.chooseSelect = 3;
            chooseSystem.confirmScren = true;
            Debug.Log("You choose " + chooseSystem.chooseSelect);
        }

        else if (gameObject.CompareTag("Suspect4"))
        {
            chooseSystem.chooseSelect = 4;
            chooseSystem.confirmScren = true;
            Debug.Log("You choose " + chooseSystem.chooseSelect);
        }

        else if (gameObject.CompareTag("Suspect5"))
        {
            chooseSystem.chooseSelect = 5;
            chooseSystem.confirmScren = true;
            Debug.Log("You choose " + chooseSystem.chooseSelect);
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
