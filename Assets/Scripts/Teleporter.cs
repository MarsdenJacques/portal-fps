using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Camera cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CamSwitch(bool trigger)
    {
        cam.gameObject.SetActive(trigger);
    }
}
