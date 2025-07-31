using UnityEngine;
using TMPro;

public class chooseItem01 : MonoBehaviour
{
    public static int chooseItem = 0;

    public TextMeshProUGUI textButton;
    public GameObject bIMG;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        chooseItem = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (chooseItem == -1)
        {
            chooseItem = 13;
        }

        else if (chooseItem == 14)
        {
            chooseItem = 0;
        }



        if (chooseItem == 0 && chooseSystem.item == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 0 && chooseSystem.item == true)
        {
            textButton.text = "กุญแจ";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 1 && chooseSystem.letter == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 1 && chooseSystem.letter == true)
        {
            textButton.text = "จดหมาย";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 2 && chooseSystem.soap == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 2 && chooseSystem.soap == true)
        {
            textButton.text = "สบู่";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 3 && chooseSystem.trash == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 3 && chooseSystem.trash == true)
        {
            textButton.text = "ขยะ";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 4 && chooseSystem.money == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 4 && chooseSystem.money == true)
        {
            textButton.text = "เงิน";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 5 && chooseSystem.nahh3 == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 5 && chooseSystem.nahh3 == true)
        {
            textButton.text = "ใบแจ้งหนี้";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 6 && chooseSystem.box == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 6 && chooseSystem.box == true)
        {
            textButton.text = "กล่อง";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 7 && chooseSystem.mouseTrap == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 7 && chooseSystem.mouseTrap == true)
        {
            textButton.text = "ที่ดักหนู";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 8 && chooseSystem.book1 == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 8 && chooseSystem.book1 == true)
        {
            textButton.text = "หนังสือ";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 9 && chooseSystem.book2 == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 9 && chooseSystem.book2 == true)
        {
            textButton.text = "หนังสือ";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 10 && chooseSystem.diary == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 10 && chooseSystem.diary == true)
        {
            textButton.text = "ไดอารี่(?)";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 11 && chooseSystem.nahhNewspaper == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 11 && chooseSystem.nahhNewspaper == true)
        {
            textButton.text = "หนังสือพิมพ์";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 12 && chooseSystem.note == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 12 && chooseSystem.note == true)
        {
            textButton.text = "Note";
            bIMG.SetActive(false);
        }

        else if (chooseItem == 13 && chooseSystem.oldPic == false)
        {
            textButton.text = "???";
            bIMG.SetActive(true);
        }
        else if (chooseItem == 13 && chooseSystem.oldPic == true)
        {
            textButton.text = "ภาพถ่ายครอบครัว";
            bIMG.SetActive(false);
        }
    }
}
