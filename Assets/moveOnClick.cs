using System.Collections; // สำคัญ: ต้องมี namespace นี้สำหรับ Coroutine
using UnityEngine;
using TMPro;

public class moveOnClick : MonoBehaviour // เปลี่ยนชื่อคลาสให้ตรงกับชื่อไฟล์ Script ของคุณ
{
    public TextMeshProUGUI qtDt14;
    public TextMeshProUGUI askBoxDt;
    public Animator animator; // ลาก Animator Component ของคุณมาใส่ใน Inspector
    public static bool playy = false; // แฟล็กควบคุมการเล่น
    public static bool delayDt;
    public static int qtDt;
    public static int limitDt;
    public int inputLimitDt;
    public GameObject gHand;

    private void Awake()
    {
        limitDt = inputLimitDt;
        gHand.SetActive(false);
    }

    void Update()
    {
        // ตรวจสอบเงื่อนไขว่าควรจะเริ่มเล่นแอนิเมชันหรือไม่
        if (playy == true)
        {
            // ตั้งค่า Bool parameter ใน Animator เพื่อเริ่มเล่นแอนิเมชัน
            animator.SetBool("isMove", true);
            delayDt = true;
            gHand.SetActive(true);

            // *** นี่คือการเรียก Coroutine ที่ถูกต้องที่สุดและแนะนำ ***
            // เรียกฟังก์ชัน ResetAnim() เพื่อให้มันคืนค่า IEnumerator ออกมา
            // แล้วส่งค่า IEnumerator นั้นให้กับ StartCoroutine()
            StartCoroutine(ResetAnim());

            // *** สำคัญมาก: ตั้งค่า playy กลับเป็น false ทันที ***
            // เพื่อป้องกันไม่ให้ StartCoroutine() ถูกเรียกซ้ำๆ ในทุกเฟรมที่ Update() ทำงาน
            // ซึ่งจะทำให้เกิดปัญหาด้านประสิทธิภาพและพฤติกรรมที่ไม่คาดคิด
            playy = false;
        }
        askBoxDt.text = limitDt.ToString();
    }

    // ฟังก์ชัน Coroutine ของคุณ
    // ต้องคืนค่าเป็น IEnumerator เสมอสำหรับ Coroutine ใน Unity
    IEnumerator ResetAnim()
    {
        // รอเป็นเวลา 2.3 วินาที (เวลาในเกม)
        // ถ้าต้องการให้รอตามเวลาจริงแม้เกมหยุด ให้ใช้ yield return new WaitForSecondsRealtime(2.3f);
        yield return new WaitForSecondsRealtime(2.3f);
        // เมื่อรอครบแล้ว ให้ตั้งค่า Bool parameter กลับเป็น false เพื่อหยุดแอนิเมชัน
        animator.SetBool("isMove", false);
        delayDt = false;
        gHand.SetActive(false);

        // คุณไม่จำเป็นต้องตั้งค่า playy = false; ที่นี่อีกครั้ง
        // เพราะเราตั้งค่าไปแล้วใน Update() เพื่อป้องกันการเรียกซ้ำ
        // แต่ถ้า Logic ของคุณต้องการให้ playy กลับมาเป็น false เมื่อ Coroutine นี้ "เสร็จสิ้น" จริงๆ
        // คุณสามารถใส่ไว้ที่นี่ได้ แต่ต้องระวังไม่ให้ Update() เรียกซ้ำก่อน

        if (RandomboxDt.getRaddomNub == 0 && qtDt == 1)
        {
            qtDt14.text = "แก้ว";
        }

        else if (RandomboxDt.getRaddomNub == 1 && qtDt == 1)
        {
            qtDt14.text = "แล้วคุณคิดว่าผีฆ่าคนได้ไหมล่ะ";
        }

        else if (RandomboxDt.getRaddomNub == 2 && qtDt == 1)
        {
            qtDt14.text = "ฉันอยากถามพ่อว่ามันไม่จริงใช่ไหม";
        }

        if (RandomboxDt.getRaddomNub == 0 && qtDt == 2)
        {
            qtDt14.text = "ฉันเกลียดหนึ่ง แต่อีกหนึ่งไมรู้จะเกลียดหรือสงสาร";
        }

        else if (RandomboxDt.getRaddomNub == 1 && qtDt == 2)
        {
            qtDt14.text = "แค่นี้มองไม่ออก ก็เลิกเป็นนักสืบเถอะ";
        }

        else if (RandomboxDt.getRaddomNub == 2 && qtDt == 2)
        {
            qtDt14.text = "น่าอึดอัด ขยะแขยง";
        }

        if (RandomboxDt.getRaddomNub == 0 && qtDt == 3)
        {
            qtDt14.text = "ถึงจะไม่รู้ว่าใคร แต่ฉันมั่นใจว่ามันไม่ใช่คนในครอบครัวฉัน";
        }

        else if (RandomboxDt.getRaddomNub == 1 && qtDt == 3)
        {
            qtDt14.text = "ฉันกำลังจะเขียนไดอารี่บนที่นอน แต่อยู่ๆก็ถูกกระชากจากข้างหลัง";
        }

        else if (RandomboxDt.getRaddomNub == 2 && qtDt == 3)
        {
            qtDt14.text = "มีคนสมควรตายมากกว่าฉัน แต่ก็ไม่ได้แปลว่าฉันสมควรอยู่หรอก";
        }

        else if (limitDt == 0 )
        {
            qtDt14.text = "วิญญาณตนนี้ ไม่อยากจะสื่อสารกับคุณอีกต่อไป";
        }
    }
}