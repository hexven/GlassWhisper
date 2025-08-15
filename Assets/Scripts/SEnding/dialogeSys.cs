using UnityEngine;
using TMPro;
using System.Collections;

public class dialogeSys : MonoBehaviour
{
    public GameObject dialogeBox;
    public TextMeshProUGUI textDialoge;
    public string[] lines;

    public float textSpeed;

    private int index;

    public bool start;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogeBox.SetActive(false);
        start = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (start == true)
        //{
        //    //textDialoge.text = string.Empty;
        //    //startDialoge();
        //    start = false;
        //}

        if (Input.GetMouseButtonDown(0) && start == true)
        {
            start = false;
            StopAllCoroutines();

            //if (textDialoge.text == lines[index])
            //{
            //    nextLine();
            //}
            //else
            //{
            //    StopAllCoroutines();
            //    textDialoge.text = lines[index];
            //}
        }

        if (start == false)
        {
            dialogeBox.SetActive (false);
        }
    }

    void startDialoge()
    {
        index = 0;
        StartCoroutine(Typeline());
        //print("lol");
    }

    void nextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textDialoge.text = string.Empty;
            StartCoroutine(Typeline());
        }
        else
        {
            dialogeBox.SetActive(false);
            //start = false;
            ////index = 0;
            //end = true;
            //start = false;
            //print(onCenterN);
        }
    }

    IEnumerator Typeline()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textDialoge.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (collision.CompareTag("dialoge"))
        //{
        //    start = true;
        //}

        if (chooseSystem.note2 == false && collision.CompareTag("Player"))
        {
            start = true;
            textDialoge.text = string.Empty;
            startDialoge();
            dialogeBox.SetActive(true);
            print("onTrigger");
        }
        //startDialoge();
        //if (chooseSystem.note2 == false && start == true && collision.CompareTag("dialoge"))
        //{
        //    start = false;
        //    startDialoge();
        //    dialogeBox.SetActive(true);
        //    print("onTrigger");
        //}
    }

}
