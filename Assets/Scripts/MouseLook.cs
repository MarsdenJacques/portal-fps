using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sens = 5000f;
    public Transform playerController;

    float xOrientation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UpdateSens();
    }
    public void UpdateSens()
    {
        float sensTest = PlayerPrefs.GetFloat("sensitivity", -1.0f);
        if (sensTest >= 0.0f)
        {
            sens = 100.0f + sensTest;
        }
    }
    public void ToggleMouse()
    {
        if(Cursor.lockState == CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        xOrientation -= mouseY;
        xOrientation = Mathf.Clamp(xOrientation, -90f, 90f);
        playerController.Rotate(Vector3.up, mouseX);
        transform.localRotation = Quaternion.Euler(xOrientation, 0, 0);
    }
}
