using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTouch : MonoBehaviour
{
    public int _lineNumber;

    public void LineTouched()
    {
        Managers.Game.LineTouch(_lineNumber);
    }
}
