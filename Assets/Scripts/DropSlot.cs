using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public string correctPieceName;
    private bool isOccupied = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && !isOccupied)
        {
            if (eventData.pointerDrag.name == correctPieceName)
            {
                // ����¹ parent ��͹
                eventData.pointerDrag.transform.SetParent(transform);

                // ���絵��˹����������ش�ٹ���ҧ�ͧ slot
                RectTransform pieceRect = eventData.pointerDrag.GetComponent<RectTransform>();
                pieceRect.anchoredPosition = Vector2.zero;

                // �������Ըչ�����ͤ���������ҡ���
                // pieceRect.localPosition = Vector3.zero;

                // ��͡�����ǹ���
                DragDrop dragComponent = eventData.pointerDrag.GetComponent<DragDrop>();
                if (dragComponent != null)
                {
                    dragComponent.LockPiece();
                }

                isOccupied = true;

                // ���¡��Ǩ�ͺ��һ��ȹ���������ó��������
                PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
                if (puzzleManager != null)
                {
                    puzzleManager.CheckComplete();
                }

                Debug.Log($"{correctPieceName} �١�ҧ� slot ���١��ͧ����!");
            }
            else
            {
                Debug.Log("�����ǹ���١��ͧ����Ѻ slot ���");
            }
        }
    }

    // �ѧ��ѹ����Ѻ������ slot (��ҵ�ͧ�������)
    public void ClearSlot()
    {
        isOccupied = false;
        if (transform.childCount > 0)
        {
            DragDrop dragComponent = transform.GetChild(0).GetComponent<DragDrop>();
            if (dragComponent != null)
            {
                dragComponent.UnlockPiece();
            }
        }
    }
}