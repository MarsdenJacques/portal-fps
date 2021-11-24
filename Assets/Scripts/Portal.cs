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
    private Vector3 CalculatePlayerTeleportVelocity(Player player, PlayerMovement movement)
    {
        //all this was pointless, jsut need to invert gravity if necessary   
        Vector3 playerVelocity = movement.getCurrentVelocity();
        Vector3 translatedVelocity = player.gameObject.transform.TransformVector(playerVelocity);
        Vector3 translatedForward = back.TransformVector(back.forward);
        Vector3 translatedUp = back.TransformVector(back.up);
        Vector3 translatedRight = back.TransformVector(back.right);
        Debug.Log(translatedVelocity);
        Debug.Log(translatedUp);
        Debug.Log(translatedForward);
        Debug.Log(translatedRight);
        Vector3 entrySpeedVector = VectorTools.VectorProjection3(translatedVelocity, translatedForward);
        float entrySpeed = VectorTools.Magnitude3(entrySpeedVector);
        Debug.Log(entrySpeedVector);
        Debug.Log(entrySpeed);
        Vector3 entryUpVector = VectorTools.VectorProjection3(translatedVelocity, translatedUp);
        Debug.Log(entryUpVector);
        Vector3 entryRightVector = VectorTools.VectorProjection3(translatedVelocity, translatedRight);
        Debug.Log(entryRightVector);
        Vector3 translatedPartnerForward = partner.front.TransformVector(partner.front.forward);
        Debug.Log(translatedPartnerForward);
        Vector3 translatedPartnerUp = partner.front.TransformVector(partner.front.up);
        Vector3 translatedPartnerRight = partner.front.TransformVector(partner.front.right);
        translatedPartnerForward = VectorTools.UnitVector3(translatedPartnerForward);
        translatedPartnerUp = VectorTools.UnitVector3(translatedPartnerUp);
        translatedPartnerRight = VectorTools.UnitVector3(translatedPartnerRight);
        translatedPartnerForward *= VectorTools.Magnitude3(entrySpeedVector);
        translatedPartnerUp *= VectorTools.Magnitude3(entryUpVector);//this is wrong
        if(entryUpVector.y < 0.0f) { translatedPartnerUp *= -1; }
        translatedPartnerRight *= VectorTools.Magnitude3(entryRightVector);
        if (entryRightVector.x < 0.0f) { translatedPartnerRight *= -1; }
        Debug.Log(translatedPartnerForward);
        Debug.Log(translatedPartnerUp);
        Debug.Log(translatedPartnerRight);
        Vector3 force = translatedPartnerForward + translatedPartnerUp + translatedPartnerRight;
        Debug.Log(force);
        Debug.Log(player.gameObject.transform.InverseTransformVector(force));
        Debug.Log(player.gameObject.transform.TransformPoint(player.gameObject.transform.position));
        Debug.Log(player.gameObject.transform.position);
        return force;
    }
    public void Teleport(GameObject port)
    {
        if (!receivingTele)//fix this by only teleporting when full object inside portal, until then just occlude
        {
            Player player = port.GetComponent<Player>();
            if (player != null)
            {
                PlayerMovement movement = player.gameObject.transform.GetChild(0).gameObject.GetComponent<PlayerMovement>();
                Vector3 force = CalculatePlayerTeleportVelocity(player, movement);
                partner.receivingTele = true;
                player.transform.SetPositionAndRotation(partner.dropOff.position, partner.dropOff.rotation);
                Quaternion rotation = player.transform.rotation;
                rotation.x = 0;
                rotation.z = 0;
                player.transform.rotation = rotation;
                //apply force to player
                movement.ApplyForce(force);
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
