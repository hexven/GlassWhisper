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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (textDialoge.text == lines[index])
        {
            nextLine();
        }
        else
        {
            StopAllCoroutines();
            textDialoge.text = lines[index];
        }
    }

    void startDialoge()
    {
        index = 0;
        StartCoroutine(Typeline());
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
        if (chooseSystem.note2 == false)
        {
            startDialoge();
        }
        //startDialoge();
    }

}
