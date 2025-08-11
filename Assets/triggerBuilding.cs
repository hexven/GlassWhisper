using UnityEngine;

public class triggerBuilding : MonoBehaviour
{
    public GameObject wallPrefab;
    public Transform spawLocation;

    private bool wall = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            wall = true;
        }

        if (wall == true)
        {
            SpawnObjectAtPosition();
        }
    }

    public void SpawnObjectAtPosition()
    {
        // ตรวจสอบว่ามี Prefab และ Transform ต้นแบบที่เรากำหนดไว้หรือไม่
        if (wallPrefab != null && spawLocation != null)
        {
            // ใช้ Instantiate() เพื่อสร้าง GameObject ใหม่
            // โดยใช้ตำแหน่ง (position) และการหมุน (rotation) จาก sourceTransform
            Instantiate(wallPrefab, spawLocation.position, spawLocation.rotation);
            Debug.Log("สร้าง GameObject ใหม่จาก Transform ของ: " + spawLocation.gameObject.name);
        }
        else
        {
            
        }
    }
}
