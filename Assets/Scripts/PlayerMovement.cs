using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Gun gun;
    public Player player;

    public float moveSpeed = 12f;
    public float jumpHeight = 5.0f;
    public float gravityMulti = 1.0f;
    public float rateOfVelocityReturn = 0.1f;

    float gravity = -9.81f;
    public Transform groundCheck;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundLayer;
    private Vector3 velocity;
    bool isGrounded;

    public float enemyCheckDistance = 0.6f;
    public LayerMask[] scanForLayers;
    private int scanForMask;
    private Collider[] collisionResults = new Collider[20];
    private float xMove = 0.0f;//Input.GetAxis("Horizontal");
    private float zMove = 0.0f;//Input.GetAxis("Vertical");

    private void Start()
    {
        scanForMask = 0;
        /*for(int layer = 0; layer < scanForLayers.Length; layer++)
        {
            int mask = 1 << scanForLayers[layer];
            Debug.Log(scanForLayers[layer] + "");
            scanForMask |= mask;
        }*/
        int mask = 3072;// 1 << 10;
        //scanForMask &= mask;
        //mask &= 2047;//1 << 11;
        Debug.Log(mask);
        scanForMask |= mask;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.manager.IsNotPaused())
        {
            Movement();
            Collision();
        }
    }
    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(-2 * jumpHeight * gravity);
        }
        xMove = 0.0f;
        zMove = 0.0f;
        if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("left"))) //maybe inefficient, save and refresh values?
        {
            xMove -= 1.0f;
        }
        if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("right"))) //maybe inefficient, save and refresh values?
        {
            xMove += 1.0f;
        }
        if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("forward"))) //maybe inefficient, save and refresh values?
        {
            zMove += 1.0f;
        }
        if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("backward"))) //maybe inefficient, save and refresh values?
        {
            zMove -= 1.0f;
        }
        Vector3 movement = transform.right * (velocity.x + xMove) * moveSpeed + transform.up * velocity.y + transform.forward * (velocity.z + zMove) * moveSpeed;
        controller.Move(movement  * Time.deltaTime);
        velocity.y += gravity * gravityMulti * Time.deltaTime;
        if (velocity.x > 0)
        {
            velocity.x = Mathf.Max(velocity.x + -1 * rateOfVelocityReturn,0);
        }
        else if(velocity.x < 0)
        {
            velocity.x = Mathf.Min(velocity.x + rateOfVelocityReturn, 0);
        }
        if (velocity.z > 0)
        {
            velocity.z = Mathf.Max(velocity.z + -1 * rateOfVelocityReturn, 0);
        }
        else if (velocity.z < 0)
        {
            velocity.z = Mathf.Min(velocity.z + rateOfVelocityReturn, 0);
        }
    }
    private void Collision()
    {
        Vector3 bottom;
        Vector3 top;
        bottom.x = transform.position.x;
        bottom.y = transform.position.y - 1.8f;
        bottom.z = transform.position.z;
        top.x = transform.position.x;
        top.y = transform.position.y + 1.8f;
        top.z = transform.position.z;
        int collisions = Physics.OverlapCapsuleNonAlloc(bottom, top, enemyCheckDistance, collisionResults, scanForMask);
        for (int collision = 0; collision < collisions; collision++)
        {
            Damageable enemy = collisionResults[collision].gameObject.GetComponent<Damageable>();
            if (enemy != null)
            {
                enemy.parent.HitPlayer(player);
            }
            Portal portal = collisionResults[collision].gameObject.GetComponent<Portal>();
            if (portal != null)
            {
                portal.Teleport(player.gameObject);
            }
        }
    }
    private Vector3 convertWorldForceToLocalVelocity(Vector3 force)
    {
        Vector3 result = new Vector3(0,0,0);
        Debug.Log(transform.forward);
        Debug.Log(force);
        result.x = VectorTools.Magnitude3(VectorTools.VectorProjection3(force, transform.TransformVector(transform.right)));
        Debug.Log(result);
        result.y = VectorTools.Magnitude3(VectorTools.VectorProjection3(force, transform.TransformVector(transform.up)));
        Debug.Log(result);
        result.z = VectorTools.Magnitude3(VectorTools.VectorProjection3(force, transform.TransformVector(transform.forward)));
        Debug.Log(result);
        return result;
    }
    public void ApplyForce(Vector3 force)
    {
        force = convertWorldForceToLocalVelocity(force);
        velocity += force;
    }
    public Vector3 getCurrentVelocity()
    {
        Vector3 result = new Vector3(velocity.x + xMove, velocity.y, velocity.z + zMove);
        return result;
    }
}
