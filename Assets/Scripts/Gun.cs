using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public Camera cam;
    public int ammoCount = 30;

    public float FireTiming = 0.2f;

    private float WeaponDowntime = 0.0f;

    public GameObject portalPrefab;

    public GameObject leftPortal;

    public GameObject rightPortal;
    public LayerMask terrain;

    private FireModes mode;

    void Start()
    {
        mode = FireModes.Semi;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.manager.IsNotPaused())
        {
            if (WeaponDowntime == 0.0f)
            {
                if (Input.GetButton("Fire1")) //switch to getbutton and a timer later for autofire
                {
                    WeaponDowntime = FireTiming;
                    Fire1();
                }
                else if (Input.GetButton("Fire2")) //switch to getbutton and a timer later for autofire
                {
                    WeaponDowntime = FireTiming;
                    Fire2();
                }
            }
            else
            {
                WeaponDowntime = Mathf.Max(WeaponDowntime - Time.deltaTime, 0.0f);
            }
            if (Input.GetButtonDown("Fire3"))
            {
                NextFireMode();
                Debug.Log(mode);
            }
            if (Input.GetButtonDown("Reload"))
            {
                ammoCount = 30;
                GameManager.manager.getGameplayUI().UpdateAmmo(ammoCount);
            }
        }
        if (Input.GetButtonDown("Pause") && !GameManager.manager.gameOver)
        {
            GameManager.manager.PauseButton();
        }
    }
    void Fire1()
    {
        switch(mode)
        {
            case FireModes.Semi:
                if(Input.GetButtonDown("Fire1"))
                {
                    Fire();
                }
                break;
            case FireModes.Auto:
                Fire();
                break;
            case FireModes.Portal:
                if (Input.GetButtonDown("Fire1"))
                {
                    FirePortal(true);
                }
                break;
            default:
                Debug.Log("Unknown Fire Mode");
                break;
        }
    }
    void Fire2()
    {
        switch (mode)
        {
            case FireModes.Portal:
                if (Input.GetButtonDown("Fire2"))
                {
                    FirePortal(false);
                }
                break;
            default:
                Debug.Log("Unknown Fire Mode");
                break;
        }
    }
    void NextFireMode()
    {
        switch(mode)
        {
            case FireModes.Auto:
                mode = FireModes.Semi;
                break;
            case FireModes.Semi:
                mode = FireModes.Portal;
                break;
            case FireModes.Portal:
                mode = FireModes.Auto;
                break;
            default:
                Debug.Log("How do you have no fire mode wtf");
                break;
        }
    }
    void Fire()
    {
        if(ammoCount > 0)
        {
            RaycastHit ray;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out ray))
            {
                Damageable hit = ray.transform.GetComponent<Damageable>();
                Teleporter tele = ray.transform.GetComponent<Teleporter>();
                if (tele != null)
                {
                    Camera portalCam = tele.cam;
                    Vector2 texCoord = ray.textureCoord;
                    Ray portalRay = portalCam.ScreenPointToRay(new Vector2(texCoord.x * portalCam.pixelWidth, texCoord.y * portalCam.pixelHeight));
                    RaycastHit portalCast;
                    if (Physics.Raycast(portalRay, out portalCast))
                    {
                        hit = portalCast.transform.GetComponent<Damageable>();
                    }
                }
                if (hit != null)
                {
                    hit.GetHit(damage);
                }

            }
            ammoCount--;
            GameManager.manager.getGameplayUI().UpdateAmmo(ammoCount);
        }
    }
    void FirePortal(bool left)
    {
        RaycastHit ray;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out ray, 10000.0f, terrain))
        {
            //Portalable hit = ray.transform.GetComponent<Portalable>();
            //Teleporter tele = ray.transform.GetComponent<Teleporter>();
            /*if (tele != null)
            {
                Camera portalCam = tele.cam;
                Vector2 texCoord = ray.textureCoord;
                Ray portalRay = portalCam.ScreenPointToRay(new Vector2(texCoord.x * portalCam.pixelWidth, texCoord.y * portalCam.pixelHeight));
                RaycastHit portalCast;
                if (Physics.Raycast(portalRay, out portalCast))
                {
                    hit = portalCast.transform.GetComponent<Portalable>();
                }
            }*/
            //if (hit != null)
           // {
                Quaternion rotation = Quaternion.LookRotation(ray.normal);
                Vector3 newPos = ray.point + (ray.normal * 0.1f);
                GameObject potentialPortal = Object.Instantiate(portalPrefab, newPos, rotation);
                potentialPortal.transform.Rotate(0.0f, 180.0f, 0.0f);
                if(potentialPortal.GetComponent<Portal>().IsValidSpawn())
                {
                    if (left)
                    {
                        if (leftPortal != null)
                        {
                            Object.Destroy(leftPortal);
                        }
                        leftPortal = potentialPortal;
                        if (rightPortal == null)
                        {
                            leftPortal.SetActive(false);
                        }
                        else
                        {
                            ActivatePortals();
                        }
                    }
                    else
                    {
                        if (rightPortal != null)
                        {
                            Object.Destroy(rightPortal);
                        }
                        rightPortal = potentialPortal;
                        if (leftPortal == null)
                        {
                            rightPortal.SetActive(false);
                        }
                        else
                        {
                            ActivatePortals();
                        }
                    }
                }
                else
                {
                    GameObject.Destroy(potentialPortal);
                }
           // }

        }
    }
    private void ActivatePortals()
    {
        leftPortal.GetComponent<Portal>().partner = rightPortal.GetComponent<Portal>();
        rightPortal.GetComponent<Portal>().partner = leftPortal.GetComponent<Portal>();
        leftPortal.SetActive(true);
        rightPortal.SetActive(true);
    }
}

enum FireModes
{
    Semi,
    Auto,
    Portal
}
