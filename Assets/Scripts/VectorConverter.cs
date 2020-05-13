using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorConverter
{
    public static Vector3 V2ToV3(Vector2 vector2, float y = 0)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }
    public static Vector2 V3ToV2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }
}
