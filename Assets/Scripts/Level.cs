using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    private Quadrant[] quadrants;
    public void SetQuadrants(Quadrant[] newQuadrants)
    {
        quadrants = newQuadrants;
    }
    public void EnableQuadrants()
    {
        foreach(Quadrant quadrant in quadrants)
        {
            quadrant.gameObject.SetActive(true);
        }
    }
    public void DisableQuadrants()
    {
        foreach (Quadrant quadrant in quadrants)
        {
            quadrant.gameObject.SetActive(false);
        }
    }
}