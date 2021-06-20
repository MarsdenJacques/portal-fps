using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float multiplier = 1.0f;
    public Enemy parent;
    public void GetHit(float damage)
    {
        Debug.Log("I'm hit!");
        parent.TakeDamage(damage * multiplier);
    }
}
