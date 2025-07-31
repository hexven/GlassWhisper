using UnityEngine;
using UnityEngine.UI;

public class changeImage02 : MonoBehaviour
{
    public Image targetImage;

    public Sprite item;
    public Sprite letter;
    public Sprite nahh;
    public Sprite nahh1;
    public Sprite nahh2;
    public Sprite nahh3;
    public Sprite nahh4;
    public Sprite nahh5;
    public Sprite nahh6;
    public Sprite nahh7;
    public Sprite nahh8;
    public Sprite newspaper;
    public Sprite note;
    public Sprite oldPic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        ChangeImage(item);
    }

    // Update is called once per frame
    void Update()
    {
        if (chooseItem02.chooseItem == 0)
        {
            ChangeImage(item);
        }
        else if (chooseItem02.chooseItem == 1)
        {
            ChangeImage(letter);
        }
        else if (chooseItem02.chooseItem == 2)
        {
            ChangeImage(nahh);
        }
        else if (chooseItem02.chooseItem == 3)
        {
            ChangeImage(nahh1);
        }
        else if (chooseItem02.chooseItem == 4)
        {
            ChangeImage(nahh2);
        }
        else if (chooseItem02.chooseItem == 5)
        {
            ChangeImage(nahh3);
        }
        else if (chooseItem02.chooseItem == 6)
        {
            ChangeImage(nahh4);
        }
        else if (chooseItem02.chooseItem == 7)
        {
            ChangeImage(nahh5);
        }
        else if (chooseItem02.chooseItem == 8)
        {
            ChangeImage(nahh6);
        }
        else if (chooseItem02.chooseItem == 9)
        {
            ChangeImage(nahh7);
        }
        else if (chooseItem02.chooseItem == 10)
        {
            ChangeImage(nahh8);
        }
        else if (chooseItem02.chooseItem == 11)
        {
            ChangeImage(newspaper);
        }
        else if (chooseItem02.chooseItem == 12)
        {
            ChangeImage(note);
        }
        else if (chooseItem02.chooseItem == 13)
        {
            ChangeImage(oldPic);
        }
    }

    public void ChangeImage(Sprite newSprite)
    {
        targetImage.sprite = newSprite;
    }
}
