using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDrag;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool isLocked = false; // �����������͡

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��ͧ�ѹ����ҡ��Ҷ١��͡����
        if (isLocked) return;

        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        // ��Ǩ�ͺ��Ҷ١�ҧ� slot ���١��ͧ�������
        bool droppedInCorrectSlot = false;

        // ����Ҷ١ drop � slot �˹
        foreach (var result in eventData.hovered)
        {
            DropSlot slot = result.GetComponent<DropSlot>();
            if (slot != null && gameObject.name == slot.correctPieceName)
            {
                droppedInCorrectSlot = true;
                break;
            }
        }

        // ���������ҧ� slot ���١��ͧ ����Ѻ仵��˹����
        if (!droppedInCorrectSlot)
        {
            transform.SetParent(parentAfterDrag);
        }

        canvasGroup.blocksRaycasts = true;
    }

    // �ѧ��ѹ����Ѻ��͡�����ǹ
    public void LockPiece()
    {
        isLocked = true;
        canvasGroup.interactable = false; // ������������ö��ͺ��
    }

    // �ѧ��ѹ����Ѻ�Ŵ��͡ (��ҵ�ͧ���)
    public void UnlockPiece()
    {
        isLocked = false;
        canvasGroup.interactable = true;
    }
}