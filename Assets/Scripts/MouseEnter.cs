using UnityEngine;

public class MouseEnter : MonoBehaviour
{
    public GameObject bg;
    //private void OnMouseEnter()
    //{
    //    gameObject.SetActive(false);
    //}

    //private void OnMouseExit()
    //{
    //    gameObject.SetActive(true);
    //}
    public void enter()
    {
        bg.SetActive(false);
        Debug.Log("IN");
    }

    public void exit()
    {
        bg.SetActive(true);
        Debug.Log("OUT");
    }
}
