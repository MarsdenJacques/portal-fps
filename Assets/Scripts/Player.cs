using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100f;
    public Gun gun;
    public MouseLook mouse;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.manager.player = this;
        GameManager.manager.restartGame.AddListener(Restart);
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
