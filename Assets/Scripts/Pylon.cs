using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pylon : MonoBehaviour
{
    public bool active = false;
    public float hp = 100.0f;
    public LayerMask scanForMask;
    public bool pylon1;
    private Collider[] collisionResults = new Collider[40];
    private void Awake()
    {
        GameManager.manager.enemyManager.pylons.Add(this);
    }
    private void Update()
    {
        Collision(); //change to coroutine
    }
    private void Collision()
    {
        Vector3 bottom;
        Vector3 top;
        bottom.x = transform.position.x;
        bottom.y = transform.position.y - 1.0f;
        bottom.z = transform.position.z;
        top.x = transform.position.x;
        top.y = transform.position.y + 1.0f;
        top.z = transform.position.z;
        int collisions = Physics.OverlapCapsuleNonAlloc(bottom, top, .05f, collisionResults, scanForMask);
        for (int collision = 0; collision < collisions; collision++)
        {
            Debug.Log("??");
            Damageable enemy = collisionResults[collision].GetComponent<Damageable>();
            if(enemy.parent.target.gameObject == gameObject)
            {
                enemy.parent.HitPylon(this);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Damageable enemy = other.GetComponent<Damageable>();
        Debug.Log(enemy == null);
        if (enemy.parent.target.gameObject == gameObject)
        {
            enemy.parent.HitPylon(this);
        }
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(pylon1)
        {
            GameManager.manager.getGameplayUI().UpdatePylon1(hp);
        }
        else
        {
            GameManager.manager.getGameplayUI().UpdatePylon2(hp);
        }
        GameManager.manager.Lose();
    }
}
