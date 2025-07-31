using System.Collections.Generic;
using UnityEngine;

public class randomQtFD2 : MonoBehaviour
{
    public void ButoonPressed()
    {
        if (moveOnClickFD.delayFD == false && moveOnClickFD.limitFD != 0)
        {
            moveOnClickFD.qtFD = 2;
            moveOnClickFD.playyFD = true;
            moveOnClickFD.limitFD -= 1;

            Debug.Log(moveOnClickFD.limitFD);
        }
    }
}