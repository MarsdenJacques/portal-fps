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
        float xMove = 0.0f;//Input.GetAxis("Horizontal");
        float zMove = 0.0f;//Input.GetAxis("Vertical");
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
        Vector3 movement = transform.right * xMove + transform.forward * zMove;
        controller.Move(movement * moveSpeed * Time.deltaTime);
        velocity.y += gravity * gravityMulti * Time.deltaTime;
        velocity.x = xMove;
        velocity.z = zMove;
        controller.Move(velocity * Time.deltaTime);
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
    public Vector3 getVelocity()
    {
        return velocity;
    }
}
