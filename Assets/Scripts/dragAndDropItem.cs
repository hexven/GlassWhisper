using UnityEngine;
using UnityEngine.EventSystems; // Make sure you have this using statement

public class dragAndDropItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform m_RectTransform;
    private Rigidbody2D rb;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Other variables and methods for your drag and drop logic

    // This is the method you need to add or correct
    public void OnPointerDown(PointerEventData eventData)
    {
        // This method is called when the pointer goes down on this UI element.
        // You can put any logic here that you want to happen at the start of a click/touch.
        // For example, you might want to bring the item to the front of the UI.
        Debug.Log("OnPointerDown called on: " + gameObject.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Your existing OnBeginDrag logic
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Your existing OnEndDrag logic
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Delete Zone"))
    //    {
    //        Debug.Log("TO");
    //        Destroy(gameObject);

    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Delete Zone"))
        {
            Debug.Log("TO");
            Destroy(gameObject);

        }
    }
}
