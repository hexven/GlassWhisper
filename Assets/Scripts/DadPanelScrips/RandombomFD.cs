using UnityEngine;
using TMPro;

public class RandomboxFD : MonoBehaviour
{
    public TextMeshProUGUI qtFD11;
    public TextMeshProUGUI qtFD12;
    public TextMeshProUGUI qtFD13;

    public static int getRaddomNubFD;
    public void ButoonPressed()
    {
        if(moveOnClickFD.delayFD == false)
        {
            System.Random random = new System.Random();
            int randomValue = random.Next(0, 3);
            getRaddomNubFD = randomValue;
            Debug.Log(getRaddomNubFD);
        }
    }

    public void Update()
    {
        if (getRaddomNubFD == 0)
        {
            qtFD11.text = "คุณชื่ออะไร";
            qtFD12.text = "คุณคิดว่าทำไมฆาตรกรถึงฆ่าคุณ";
            qtFD13.text = "คุณจำได้ไหมว่าอะไรเกิดขึ้นกับคุณ";
        }

        else if (getRaddomNubFD == 1)
        {
            qtFD11.text = "คุณเห็นหน้าคนร้ายหรือเปล่า";
            qtFD12.text = "คุณคือฆาตกรหรือเปล่า";
            qtFD13.text = "คุณโกรธหรือเกลียดใครอยู่หรือเปล่า";
        }

        else if (getRaddomNubFD == 2)
        {
            qtFD11.text = "คุณอยากพูดหรือถามอะไรกับใครไหม";
            qtFD12.text = "บรรยากาศภายในบ้านเป็นยังไง";
            qtFD13.text = "คุณคิดว่าคุณสมควรตายไหม";
        }
    }
}