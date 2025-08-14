using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public DropSlot[] slots;

    public void CheckComplete()
    {
        foreach (var slot in slots)
        {
            if (slot.transform.childCount == 0 || slot.transform.GetChild(0).name != slot.correctPieceName)
            {
                return; // ยังไม่ครบ
            }
        }
        Debug.Log("Puzzle Completed!");
    }
}
