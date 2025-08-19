using System.Collections; // สำคัญ: ต้องมี namespace นี้สำหรับ Coroutine
using UnityEngine;
using TMPro;

public class moveOnClickFD : MonoBehaviour // เปลี่ยนชื่อคลาสให้ตรงกับชื่อไฟล์ Script ของคุณ
{
    public TextMeshProUGUI qtFD14;
    public TextMeshProUGUI askBoxFD;
    public Animator animator; // ลาก Animator Component ของคุณมาใส่ใน Inspector
    public static bool playyFD = false; // แฟล็กควบคุมการเล่น
    public static bool delayFD;
    public static int qtFD;
    public static int limitFD;
    public int inputLimitFD;
    public GameObject gHand;

    private void Awake()
    {
        limitFD = inputLimitFD;
        gHand.SetActive(false);
    }

    void Update()
    {
        // ตรวจสอบเงื่อนไขว่าควรจะเริ่มเล่นแอนิเมชันหรือไม่
        if (playyFD == true)
        {
            // ตั้งค่า Bool parameter ใน Animator เพื่อเริ่มเล่นแอนิเมชัน
            animator.SetBool("isMove", true);
            delayFD = true;
            gHand.SetActive(true);

            // *** นี่คือการเรียก Coroutine ที่ถูกต้องที่สุดและแนะนำ ***
            // เรียกฟังก์ชัน ResetAnim() เพื่อให้มันคืนค่า IEnumerator ออกมา
            // แล้วส่งค่า IEnumerator นั้นให้กับ StartCoroutine()
            StartCoroutine(ResetAnim());

            // *** สำคัญมาก: ตั้งค่า playy กลับเป็น false ทันที ***
            // เพื่อป้องกันไม่ให้ StartCoroutine() ถูกเรียกซ้ำๆ ในทุกเฟรมที่ Update() ทำงาน
            // ซึ่งจะทำให้เกิดปัญหาด้านประสิทธิภาพและพฤติกรรมที่ไม่คาดคิด
            playyFD = false;
        }
        askBoxFD.text = limitFD.ToString();
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
        delayFD = false;
        gHand.SetActive(false);

        // คุณไม่จำเป็นต้องตั้งค่า playy = false; ที่นี่อีกครั้ง
        // เพราะเราตั้งค่าไปแล้วใน Update() เพื่อป้องกันการเรียกซ้ำ
        // แต่ถ้า Logic ของคุณต้องการให้ playy กลับมาเป็น false เมื่อ Coroutine นี้ "เสร็จสิ้น" จริงๆ
        // คุณสามารถใส่ไว้ที่นี่ได้ แต่ต้องระวังไม่ให้ Update() เรียกซ้ำก่อน

        if (RandomboxFD.getRaddomNubFD == 0 && qtFD == 1)
        {
            qtFD14.text = "ถ้ารู้แล้วทำให้จับคนร้ายได้กูจะบอกให้";
        }

        else if (RandomboxFD.getRaddomNubFD == 1 && qtFD == 1)
        {
            qtFD14.text = "กูมีตา";
        }

        else if (RandomboxFD.getRaddomNubFD == 2 && qtFD == 1)
        {
            qtFD14.text = "กูไม่สนใจความคิดเห็นของใครอยู่แล้ว";
        }

        if (RandomboxFD.getRaddomNubFD == 0 && qtFD == 2)
        {
            qtFD14.text = "เอาอีแก้วตาไปเข้าคุกให้ได้";
            //qtFD14.text = "ก็แค่อีคนเนรคุณ";
        }

        else if (RandomboxFD.getRaddomNubFD == 1 && qtFD == 2)
        {
            qtFD14.text = "ถ้ากูไม่ตายก่อนกูอาจจะฆ่ามันแทนก็ได้";
        }

        else if (RandomboxFD.getRaddomNubFD == 2 && qtFD == 2)
        {
            qtFD14.text = "เหมือนสวรรค์บนดินเลยล่ะ";
        }

        if (RandomboxFD.getRaddomNubFD == 0 && qtFD == 3)
        {
            qtFD14.text = "อีผีบ้านั่นมันตัดของกูไป";
        }

        else if (RandomboxFD.getRaddomNubFD == 1 && qtFD == 3)
        {
            qtFD14.text = "ก็ไม่นี่";
        }

        else if (RandomboxFD.getRaddomNubFD == 2 && qtFD == 3)
        {
            qtFD14.text = "กูไม่ได้ทำอะไรผิด ก็เลยไม่เคยคิดน่ะ";
        }

        else if (limitFD == 0)
        {
            qtFD14.text = "วิญญาณตนนี้ ไม่อยากจะสื่อสารกับคุณอีกต่อไป";
        }
    }
}