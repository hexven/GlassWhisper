using System.Collections.Generic;
using UnityEngine;

public class randomQtFD1 : MonoBehaviour
{
    public void ButoonPressed()
    {
        if (moveOnClickFD.delayFD == false && moveOnClickFD.limitFD != 0)
        {
            moveOnClickFD.qtFD = 1;
            moveOnClickFD.playyFD = true;
            moveOnClickFD.limitFD -= 1;

            Debug.Log(moveOnClickFD.limitFD);
        }
    }
}