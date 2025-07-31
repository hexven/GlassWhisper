using System.Collections.Generic;
using UnityEngine;

public class randomQtDt3 : MonoBehaviour
{
    public void ButoonPressed()
    {
        if (moveOnClick.delayDt == false && moveOnClick.limitDt != 0)
        {
            moveOnClick.qtDt = 3;
            moveOnClick.playy = true;
            moveOnClick.limitDt -= 1;

            Debug.Log(moveOnClick.limitDt);
        }
    }
}