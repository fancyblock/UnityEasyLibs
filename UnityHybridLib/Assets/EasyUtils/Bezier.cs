using UnityEngine;

namespace EasyLibs
{
    public static class Bezier
    {
        // vector2 one control point
        public static Vector2 GetPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector2 p = uu * p0; // (1-t)^2 * p0
            p += 2 * u * t * p1; // 2 * (1-t) * t * p1
            p += tt * p2; // t^2 * p2

            return p;
        }

        // vector2 two control points
        public static Vector2 GetPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 p = uuu * p0; // (1-t)^3 * p0
            p += 3 * uu * t * p1; // 3 * (1-t)^2 * t * p1
            p += 3 * u * tt * p2; // 3 * (1-t) * t^2 * p2
            p += ttt * p3; // t^3 * p3

            return p;
        }
    }
}