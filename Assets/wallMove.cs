using UnityEngine;

public class wallMove : MonoBehaviour
{
    public float speed = 5.0f;

    // กำหนดเวลาหน่วงก่อนทำลายตัวเอง (หน่วยเป็นวินาที)
    public float delayInSeconds = 3.0f;

    void Start()
    {
        // เรียกใช้คำสั่ง Destroy() กับ GameObject นี้
        // โดยจะทำงานหลังจากเวลาที่กำหนดไว้ (delayInSeconds)
        Destroy(gameObject, delayInSeconds);
    }
    void Update()
    {
        // คำนวณการเคลื่อนที่บนแกน X โดยคูณความเร็วกับ Time.deltaTime เพื่อให้การเคลื่อนที่ราบรื่นและเป็นอิสระจาก Frame Rate
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
