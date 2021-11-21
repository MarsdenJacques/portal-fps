using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorTools
{ 
    public static float DotProduct3(Vector3 vecA, Vector3 vecB)
    {
        return (vecA.x * vecB.x) + (vecA.y * vecB.y) + (vecA.z * vecB.z);
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
    public static Vector3 CrossProduct3(Vector3 vecA, Vector3 vecB)
    {
        Vector3 crossProduct = new Vector3();
        crossProduct.x = vecA.y * vecB.z - vecA.z * vecB.y;
        crossProduct.y = vecA.z * vecB.x - vecA.x * vecB.z;
        crossProduct.z = vecA.x * vecB.y - vecA.y - vecB.x;
        return crossProduct;
    }
    //projection of vector A onto vector B
    public static Vector3 VectorProjection3(Vector3 vecA, Vector3 vecB)
    {
        Vector3 bUnitVector = UnitVector3(vecB);
        float aDotB = DotProduct3(vecA, bUnitVector);
        bUnitVector.x *= aDotB;
        bUnitVector.y *= aDotB;
        bUnitVector.z *= aDotB;
        return bUnitVector;
    }
    //projection of vector A onto the plane defined by planeA and planeB
    public static Vector3 PlaneProjection3(Vector3 vecA, Vector3 planeA, Vector3 planeB)
    {
        Vector3 planeNormalVector = CrossProduct3(planeA, planeB);
        Vector3 projectionVector = VectorProjection3(vecA, planeNormalVector);
        return projectionVector;
    }
}
