using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorTools
{ 
    public static float DotProduct3(Vector3 vec1, Vector3 vec2)
    {
        return (vec1.x * vec2.x) + (vec1.y * vec2.y) + (vec1.z * vec2.z);
    }
    public static float Magnitude3(Vector3 vector)
    {
        return Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2));
    }
    public static Vector3 UnitVector3(Vector3 vector)
    {
        float mag = Magnitude3(vector);
        return new Vector3(vector.x / mag, vector.y / mag, vector.z / mag);
    }
}
