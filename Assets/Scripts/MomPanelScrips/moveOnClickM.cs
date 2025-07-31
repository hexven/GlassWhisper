using System.Collections; // สำคัญ: ต้องมี namespace นี้สำหรับ Coroutine
using UnityEngine;
using TMPro;

public class moveOnClickM : MonoBehaviour // เปลี่ยนชื่อคลาสให้ตรงกับชื่อไฟล์ Script ของคุณ
{
    public TextMeshProUGUI qtM14;
    public TextMeshProUGUI askBoxM;
    public Animator animator; // ลาก Animator Component ของคุณมาใส่ใน Inspector
    public static bool playyM = false; // แฟล็กควบคุมการเล่น
    public static bool delayM;
    public static int qtM;
    public static int limitM;
    public int inputLimitM;
    public GameObject gHand;

    private void Awake()
    {
        limitM = inputLimitM;
        gHand.SetActive(false);
    }
    void Update()
    {
        // ตรวจสอบเงื่อนไขว่าควรจะเริ่มเล่นแอนิเมชันหรือไม่
        if (playyM == true)
        {
            // ตั้งค่า Bool parameter ใน Animator เพื่อเริ่มเล่นแอนิเมชัน
            animator.SetBool("isMove", true);
            delayM = true;
            gHand.SetActive(true);

            // *** นี่คือการเรียก Coroutine ที่ถูกต้องที่สุดและแนะนำ ***
            // เรียกฟังก์ชัน ResetAnim() เพื่อให้มันคืนค่า IEnumerator ออกมา
            // แล้วส่งค่า IEnumerator นั้นให้กับ StartCoroutine()
            StartCoroutine(ResetAnim());

            // *** สำคัญมาก: ตั้งค่า playy กลับเป็น false ทันที ***
            // เพื่อป้องกันไม่ให้ StartCoroutine() ถูกเรียกซ้ำๆ ในทุกเฟรมที่ Update() ทำงาน
            // ซึ่งจะทำให้เกิดปัญหาด้านประสิทธิภาพและพฤติกรรมที่ไม่คาดคิด
            playyM = false;
        }
        askBoxM.text = limitM.ToString();
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
        delayM = false;
        gHand.SetActive(false);

        // คุณไม่จำเป็นต้องตั้งค่า playy = false; ที่นี่อีกครั้ง
        // เพราะเราตั้งค่าไปแล้วใน Update() เพื่อป้องกันการเรียกซ้ำ
        // แต่ถ้า Logic ของคุณต้องการให้ playy กลับมาเป็น false เมื่อ Coroutine นี้ "เสร็จสิ้น" จริงๆ
        // คุณสามารถใส่ไว้ที่นี่ได้ แต่ต้องระวังไม่ให้ Update() เรียกซ้ำก่อน

        if (RandomboxM.getRaddomNubM == 0 && qtM == 1)
        {
            qtM14.text = "ยังไม่ได้ตั้ง";
        }

        else if (RandomboxM.getRaddomNubM == 1 && qtM == 1)
        {
            qtM14.text = "แล้วฉันจะทำไปทำไม";
        }

        else if (RandomboxM.getRaddomNubM == 2 && qtM == 1)
        {
            qtM14.text = "เธอรูัไหมว่าฉันจะเจอคนที่ตายแล้วได้ยังไง";
        }

        if (RandomboxM.getRaddomNubM == 0 && qtM == 2)
        {
            qtM14.text = "ฉันอยู่ที่ระเบียง แต่จู่ๆก็ถูกผลัก";
        }

        else if (RandomboxM.getRaddomNubM == 1 && qtM == 2)
        {
            qtM14.text = "ถ้าไม่เชื่อก็ไม่ต้องถาม";
        }

        else if (RandomboxM.getRaddomNubM == 2 && qtM == 2)
        {
            qtM14.text = "อึดอัดเล็กน้อย แต่ฉันมีความสุขกับสามีดี";
        }

        if (RandomboxM.getRaddomNubM == 0 && qtM == 3)
        {
            qtM14.text = "ถ้าแกถูกผลักจากด้านหลัง แกจะมองเห็นหรือไง";
        }

        else if (RandomboxM.getRaddomNubM == 1 && qtM == 3)
        {
            qtM14.text = "ฉันไม่ได้เกลียดใคร แต่เธอคงไม่ค่อยชอบใจฉัน";
        }

        else if (RandomboxM.getRaddomNubM == 2 && qtM == 3)
        {
            qtM14.text = "ก็รู้สึกผิดนะ แต่ไม่ถึงกับรู้สึกว่าตัวเองสมควรตายน่ะ";
        }

        else if (limitM == 0)
        {
            qtM14.text = "วิญญาณตนนี้ ไม่อยากจะสื่อสารกับคุณอีกต่อไป";
        }
    }
}