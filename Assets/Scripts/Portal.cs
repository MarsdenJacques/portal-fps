using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public Portal partner;
    public Transform front;
    public Transform back;
    public Transform dropOff;
    public Camera cam;
    public Renderer viewRenderer;
    public bool receivingTele = false;
    public LayerMask terrain;
    private RenderTexture renderTexture;
    private Material material;
    private Camera mainCam;
    private Vector4 vectorPlane;
    private Collider[] overlapBoxCollision = new Collider[1];
    private Vector3 overlapBoxSize = new Vector3(5.0f, 5.0f, 0.01f);

    // Start is called before the first frame update
    void Start()
    {
        receivingTele = false;
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.DefaultHDR);
        renderTexture.Create();
        material = viewRenderer.material;
        material.mainTexture = renderTexture;
        cam.targetTexture = renderTexture;
        mainCam = Camera.main;

        Plane plane = new Plane(front.forward, transform.position);
        vectorPlane = new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 virtualPosition = TranslatePos(this, partner, mainCam.transform.position);
        Quaternion virtualRotation = TranslateRotation(this, partner, mainCam.transform.rotation);
        cam.transform.SetPositionAndRotation(virtualPosition, virtualRotation);

        // Calculate projection matrix

        Vector4 clipThroughSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(cam.worldToCameraMatrix)) * partner.vectorPlane;

        // Set portal camera projection matrix to clip walls between target portal and portal camera
        // Inherits main camera near/far clip plane and FOV settings

        Matrix4x4 obliqueProjectionMatrix = mainCam.CalculateObliqueMatrix(clipThroughSpace);
        cam.projectionMatrix = obliqueProjectionMatrix;

        // Render camera

        cam.Render();

        //bool hitPortable = Physics.CheckCapsule(bottom, top, enemyCheckDistance, enemyLayer);
        //if (hitEnemy)
        //{
        //    Debug.Log("raycast oof");
        //}
    }
    public Vector3 TranslatePos(Portal reference, Portal receiver, Vector3 position)
    {
        return(receiver.back.TransformPoint(reference.front.InverseTransformPoint(position)));
    }
    public Quaternion TranslateRotation(Portal reference, Portal receiver, Quaternion rotation)
    {
        return(receiver.back.rotation * Quaternion.Inverse(reference.front.rotation) * rotation);
    }
    private void OnTriggerEnter(Collider other) //change
    {
       // Teleport(other.gameObject);
    }
    public void Teleport(GameObject port)
    {
        if (!receivingTele)
        {
            Player player = port.GetComponent<Player>();
            if (player != null)
            {
                partner.receivingTele = true;
                player.transform.SetPositionAndRotation(partner.dropOff.position, partner.dropOff.rotation);
                Quaternion rotation = player.transform.rotation;
                rotation.x = 0;
                rotation.z = 0;
                player.transform.rotation = rotation;
                player.Teleported();
            }
            else
            {
                Debug.Log("How did you get this function " + port.name +"?");
            }
        }
        else
        {
            Debug.Log("just tpd");
        }
    }
    private void OnTriggerExit(Collider other) //change to timer coroutine (or just make the check for teleportation much smaller on the player and dump them outside the teleportation zone?
        //doesn't work for stuff on the ground, also have to maintain momentum
    {
        receivingTele = false;
    }
    private void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
        }
        Destroy(material);
        Destroy(renderTexture);
    }
    public bool IsValidSpawn()
    {
        int result = Physics.OverlapBoxNonAlloc(gameObject.transform.position, overlapBoxSize, overlapBoxCollision, gameObject.transform.rotation, terrain);
        Debug.Log(result);
        return result == 0;
    }
}
