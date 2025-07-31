using UnityEngine;
using TMPro;

public class RandomboxDt : MonoBehaviour
{
    public TextMeshProUGUI qtDt11;
    public TextMeshProUGUI qtDt12;
    public TextMeshProUGUI qtDt13;

    public static int getRaddomNub;
    public void ButoonPressed()
    {
        if (moveOnClick.delayDt == false)
        {
            System.Random random = new System.Random();
            int randomValue = random.Next(0, 3);
            getRaddomNub = randomValue;
            Debug.Log(getRaddomNub);
        }
    }

    public void Update()
    {
        if (getRaddomNub == 0)
        {
            qtDt11.text = "คุณชื่ออะไร";
            qtDt12.text = "คุณโกรธหรือเกลียดใครอยู่หรือไม่";
            qtDt13.text = "คุณเห็นหน้าคนร้ายหรือไม่";
        }

        else if (getRaddomNub == 1)
        {
            qtDt11.text = "คุณคิดว่าแม่ของคุณคือฆาตกรหรือไม่";
            qtDt12.text = "คุณคือฆาตกรหรือเปล่า";
            qtDt13.text = "คุณจำได้ไหมว่าอะไรเกิดขึ้นกับคุณ";
        }

        else if (getRaddomNub == 2)
        {
            qtDt11.text = "คุณอยากพูดหรือถามอะไรกับใครไหม";
            qtDt12.text = "บรรยากาศภายในบ้านเป็นยังไง";
            qtDt13.text = "คุณคิดว่าคุณสมควรตายไหม";
        }
    }
}