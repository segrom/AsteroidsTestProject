using System;
using UnityEngine;

namespace Extensions
{
    public static class MathExtensions
    {
        public static float Pow(this float a, float d) =>Mathf.Pow(a, d);
        public static float ToRad(this float deg) => deg * Mathf.PI / 180f;
        public static float ToDeg(this float rad) => rad / Mathf.PI * 180f;

        /// <summary>
        /// Return normalized vector from rotation
        /// </summary>
        /// <param name="rotation">Rotation in degree</param>
        /// <returns>Normalized vector</returns>
        public static Vector2 RotationToVector(this float rotation) =>
            new Vector2(Mathf.Cos(rotation.ToRad()), Mathf.Sin(rotation.ToRad())).normalized;

        public static float GetAngleToPoint(this Vector2 from, Vector2 to)
        {
            var dir = (to - from).normalized;
            return Mathf.Acos(dir.x).ToDeg() * (dir.y/ Mathf.Abs(dir.y));
        }
        
        public static float DistanceToLine(this Vector2 o, Vector2 p1, Vector2 p2)
        {
            return Mathf.Abs((p2.y - p1.y) * o.x - (p2.x - p1.x) * o.y + p2.x * p1.y - p2.y * p1.x) / (Vector2.Distance(p1, p2) + 0.000000001f);
        }
    
        public static float DistanceToSegment(this Vector2 o, Vector2 p1, Vector2 p2, out Vector2 closestPoint)
        {
            var distanceToLine = o.DistanceToLine(p1, p2);
            closestPoint = p1 + (p2 - p1).normalized * Mathf.Sqrt((Vector2.Distance(o,p1).Pow(2) - distanceToLine.Pow(2)));
            if (Vector2.Distance(o,closestPoint) > Vector2.Distance(o,p1)|| Vector2.Distance(o,closestPoint) >  Vector2.Distance(o,p2))
            {
                closestPoint = new Vector2();
                return float.MaxValue;
            }

            if ( Vector2.Distance(p1,closestPoint) +  Vector2.Distance(p2,closestPoint) <=  Vector2.Distance(p1,p2) + 0.000000001f ) return distanceToLine;
        
            closestPoint = new Vector2();
            return float.MaxValue;
        }
    }
}