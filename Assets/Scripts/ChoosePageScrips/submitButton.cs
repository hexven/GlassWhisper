using UnityEngine;
using TMPro;

public class submitButton : MonoBehaviour
{
    public GameObject chooseUI;
    public GameObject chooseItemUI;
    public TextMeshProUGUI summitBT;

    private bool itemUI;
   
    public void clickIt()
    {
        if (chooseUI.activeSelf == false)
        {
            chooseUI.SetActive(true);
            chooseItemUI.SetActive(false);
        }

        else if (chooseUI.activeSelf == true)
        {
            chooseUI.SetActive(false);
            chooseItemUI.SetActive(true);
        }
    }

    public void OnMouseEnter()
    {
        summitBT.text = "แน่ใจหรือไม่?";
    }

    public void OnMouseExit()
    {
        summitBT.text = "SUBMIT";
    }

    private void Update()
    {
        if (chooseUI.activeSelf == true)
        {
            summitBT.text = "ย้อนกลับ";
        }
    }
}
