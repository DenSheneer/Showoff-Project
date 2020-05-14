using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class LineSegment : MonoBehaviour
{

    public Vector2 start;
    public Vector2 end;

    public uint lineWidth = 1;

    public LineSegment(float pStartX, float pStartY, float pEndX, float pEndY, uint pLineWidth = 1, bool pGlobalCoords = false)
        : this(new Vector2(pStartX, pStartY), new Vector2(pEndX, pEndY), pLineWidth)
    {
    }

    public LineSegment(Vector2 pStart, Vector2 pEnd, uint pLineWidth = 1)
    {
        lineWidth = pLineWidth;
    }

    private void Start()
    {
        
        Linecap[] caps = GetComponentsInChildren<Linecap>();
        foreach (Linecap cap in caps)
        {
            if (cap.startEndCap == "start")
                start = VectorConverter.V3ToV2(cap.WorldPosition);
            if (cap.startEndCap == "end")
                end = VectorConverter.V3ToV2(cap.WorldPosition);
        }
    }

    //public Linecap startCap()
    //{
    //    Linecap lc = new Linecap();
    //    lc.position = start;
    //    lc.radius = 0;
    //    return lc;
    //}

    //public Linecap endCap()
    //{
    //    Linecap lc = new Linecap();
    //    lc.position = end;
    //    lc.radius = 0;
    //    return lc;
    //}

    //public Linecap FindClosestCap(Vec2 point)
    //{
    //    float ballStartCapDistance = (startCap().position - point).Length();
    //    float ballEndCapDistance = (endCap().position - point).Length();

    //    if (ballEndCapDistance < ballStartCapDistance)
    //    {
    //        return endCap();
    //    }
    //    else
    //    {
    //        return startCap();
    //    }
    //}
}
