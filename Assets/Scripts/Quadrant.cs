using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadrant : MonoBehaviour
{
    public bool currentQuad = false;
    public int val;
    //coroutine for checking if player entered
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(player != null)
        {
            currentQuad = true;
            Debug.Log("entered quad" + val);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            currentQuad = false;
            Debug.Log("left quad" + val);
        }
    }
}
