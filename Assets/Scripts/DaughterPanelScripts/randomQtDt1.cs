using System.Collections.Generic;
using UnityEngine;

public class randomQtDt1 : MonoBehaviour
{
    public void ButoonPressed()
    {
        if (moveOnClick.delayDt == false && moveOnClick.limitDt != 0)
        {
            moveOnClick.qtDt = 1;
            moveOnClick.playy = true;
            moveOnClick.limitDt -= 1;

            Debug.Log(moveOnClick.limitDt);
        }
    }
}