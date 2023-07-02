using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class MathMover
{
    static public void Lookat(Vector2 T, Transform I)
    {
        float angle;
        angle = Mathf.Atan2(T.y - I.position.y, T.x - I.position.x) * Mathf.Rad2Deg;
        I.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
     static public Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

}
