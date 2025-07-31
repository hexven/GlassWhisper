using UnityEngine;
public class chooseSystem : MonoBehaviour
{
    public static bool confirmScren;
    public static int chooseSelect;
    public static bool item = false;
    public static bool letter = false;
    public static bool soap = false;
    public static bool trash = false;
    public static bool money = false;
    public static bool nahh3 = false;
    public static bool box = false;
    public static bool mouseTrap = false;
    public static bool book1 = false;
    public static bool book2 = false;
    public static bool diary = false;
    public static bool nahhNewspaper = false;
    public static bool note = false;
    public static bool oldPic = false;
    public static bool key = false;
    public GameObject confirmPanel;
    public GameObject Suspect04;
    public GameObject Suspect05;
    private void Awake()
    {
        confirmPanel.SetActive(false);
        Suspect04.SetActive(false);
        Suspect05.SetActive(false);
        chooseItem01.chooseItem = 1;
        chooseItem02.chooseItem = 1;
        chooseItem03.chooseItem3 = 1;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("HII");
    }
    // Update is called once per frame
    void Update()
    {
        if (confirmScren == true)
        {
            confirmPanel.SetActive(true);
        }
        if (confirmScren == false)
        {
            confirmPanel.SetActive(false);
        }
    }
    //{
    //if (confirmScren == true)
    //{
    //    confirmPanel.SetActive(true);
    //}
    //if (confirmScren == false)
    //{
    //    confirmPanel.SetActive(false);
    //}
    //if (nahh8 == true && oldPic == true)
    //{
    //    Suspect04.SetActive(true);
    //}
    //if (letter == true && nahh3 == true)
    //{
    //    Suspect05.SetActive(true);
    //}

}