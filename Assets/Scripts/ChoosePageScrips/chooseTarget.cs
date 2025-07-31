using UnityEngine;

public class chooseTarget : MonoBehaviour
{
    public int[] targetItem1 = {0, 0, 0};
    public int targetItem2;
    
    public GameObject Suspect1;
    public GameObject Suspect2;
    public GameObject Suspect3;
    public GameObject Suspect4;
    public GameObject Suspect5;

    public GameObject bItem1;
    public GameObject bItem2;
    public GameObject bItem3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (chooseItem01.chooseItem == targetItem1[0] && chooseItem02.chooseItem == targetItem1[1] && chooseItem03.chooseItem3 == targetItem1[2] && bItem1.activeSelf == false && bItem2.activeSelf == false && bItem3.activeSelf == false)
        {
            Suspect1.SetActive(true);
            Suspect2.SetActive(true);
            Suspect3.SetActive(true);
            Suspect4.SetActive(false);
            Suspect5.SetActive(true);
        }

        else if (chooseItem01.chooseItem == targetItem2 && bItem1.activeSelf == false)
        {
            Suspect1.SetActive(false);
            Suspect2.SetActive(true);
            Suspect3.SetActive(true);
            Suspect4.SetActive(true);
            Suspect5.SetActive(true);
        }

        else if (chooseItem02.chooseItem == targetItem2  && bItem2.activeSelf == false)
        {
            Suspect1.SetActive(false);
            Suspect2.SetActive(true);
            Suspect3.SetActive(true);
            Suspect4.SetActive(true);
            Suspect5.SetActive(true);
        }

        else if (chooseItem03.chooseItem3 == targetItem2  && bItem3.activeSelf == false)
        {
            Suspect1.SetActive(false);
            Suspect2.SetActive(true);
            Suspect3.SetActive(true);
            Suspect4.SetActive(true);
            Suspect5.SetActive(true);
        }

        else
        {
            Suspect1.SetActive(false);
            Suspect2.SetActive(true);
            Suspect3.SetActive(true);
            Suspect4.SetActive(false);
            Suspect5.SetActive(true);
        }
    }
}
