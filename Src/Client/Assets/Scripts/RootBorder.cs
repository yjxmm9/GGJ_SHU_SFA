using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBorder : MonoBehaviour
{
    //public GM gM;
    public LineRenderer borderLine, modelLine;
    public float thickness;

    // Update is called once per frame
    void Update()
    {
        //border跟随主root前进
        Vector3[] newPos = new Vector3[modelLine.positionCount];
        for (var i = 0; i < newPos.Length; i++)
        {
            newPos[i] = modelLine.GetPosition(i);
        }
        borderLine.positionCount = modelLine.positionCount;
        borderLine.SetPositions(newPos);
        borderLine.widthCurve = modelLine.widthCurve;
        borderLine.widthMultiplier = modelLine.widthMultiplier * thickness + 0.05f * 1;
    }
}
