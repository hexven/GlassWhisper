using UnityEngine;
using TMPro;

public class RandomboxM : MonoBehaviour
{
    public TextMeshProUGUI qtM11;
    public TextMeshProUGUI qtM12;
    public TextMeshProUGUI qtM13;

    public static int getRaddomNubM;
    public void ButoonPressed()
    {
        if(moveOnClickM.delayM == false)
        {
            System.Random random = new System.Random();
            int randomValue = random.Next(0, 3);
            getRaddomNubM = randomValue;
            Debug.Log(getRaddomNubM);
        }
    }

    public void Update()
    {
        if (getRaddomNubM == 0)
        {
            qtM11.text = "คุณชื่ออะไร";
            qtM12.text = "คุณจำได้ไหมว่าอะไรเกิดขึ้นกับคุณ";
            qtM13.text = "คุณเห็นหน้าคนร้ายหรือเปล่า";
        }

        else if (getRaddomNubM == 1)
        {
            qtM11.text = "คุณคือฆาตกรหรือเปล่า";
            qtM12.text = "คุณโกหฉันอยู่รึเปล่า";
            qtM13.text = "คุณโกรธหรือเกลียดใครอยู่หรือเปล่า";
        }

        else if (getRaddomNubM == 2)
        {
            qtM11.text = "คุณอยากพูดหรือถามอะไรกับใครไหม";
            qtM12.text = "บรรยากาศภายในบ้านเป็นยังไง";
            qtM13.text = "คุณคิดว่าคุณสมควรตายไหม";
        }
    }
}