using System.Collections.Generic;
using UnityEngine;

public class randomQtM1 : MonoBehaviour
{
    public void ButoonPressed()
    {
        if (moveOnClickM.delayM == false && moveOnClickM.limitM != 0)
        {
            moveOnClickM.qtM = 1;
            moveOnClickM.playyM = true;
            moveOnClickM.limitM -= 1;

            Debug.Log(moveOnClickM.limitM);
        }
    }
}