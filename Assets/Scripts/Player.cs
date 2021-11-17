using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100f;
    public Gun gun;
    public MouseLook mouse;
    private Vector3 startPos = new Vector3();

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.manager.player = this;
        GameManager.manager.restartGame.AddListener(Restart);
        startPos.x = transform.position.x;
        startPos.y = transform.position.y;
        startPos.z = transform.position.z;
    }
    public void TakeDamage(float amount)
    {
        //Debug.Log("Oof");
        health -= amount;
        GameManager.manager.getGameplayUI().UpdateHP(health);
        if (health <= 0)
        {
            Die();
        }
    }
    //have to disable character controller temporarily to hard-reset the position of the player back to the starting point
    public void OnGameplayStart()
    {
        CharacterController controller = gameObject.GetComponent<CharacterController>();
        controller.enabled = false;
        gameObject.transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
        controller.enabled = true;
    }
    public void Teleported()
    {
        Physics.SyncTransforms();
    }
    void Die()
    {
        GameManager.manager.Lose();
    }
    public void UpdateSens()
    {
        mouse.UpdateSens();
    }
    public void Pause()
    {
        mouse.ToggleMouse();
    }
    private void Restart()
    {
        health = 100.0f;
        gun.ammoCount = 30;
    }
    private void OnDestroy()
    {
        GameManager.manager.restartGame.RemoveListener(Restart);
    }
}
