using UnityEngine;

public class deleteWall : MonoBehaviour
{
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "wall")
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
