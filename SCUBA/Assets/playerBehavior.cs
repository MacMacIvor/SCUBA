using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class playerBehavior : MonoBehaviour
{
    public GameObject player;
    private GameObject rope;
    public Material mat;
    public GameObject water;
    public GameObject playerSprite;

    float distToGround;
    bool isGrounded = false;
    [Range(0, 20)]
    public float intensityMovementForce = 1;
    [Range(0, 300)]
    public float jumpStrength = 1;
    [Range(0, 20)]
    public float ropeRange = 1;
    [Range(0, 1)]
    public float ropeThinkness = 0.2f;
    [Range(0, 20)]
    public float ropeDuration = 1;
    private float ropeDurationAt = 0;
    [Range(0, 2000)]
    public float grapplePower = 1;

    Vector2 mousePosition;
    Vector3 maxDistance;

    private int dir = 0;

    Vector3 spawnPosition;
    Vector3 initSpawnPosition;

    //0  1  2
    //3  4  5
    //6  7  8


    private bool grapple = true;
    private bool pull = true;
    private bool inWater = false;
    RaycastHit watRay; //= Physics.SphereCastAll(Vector3.forward, 0, Vector3.forward);
    private Vector3 angle;
    bool pullingObject = false;
    GameObject pullingOBJ;
    Vector3 currentPullPos;
    Vector3 previousPullPos;

    [Range(0, 100)]
    public float maxBreath = 7;
    private float breath = 7;

    [Range(0, 100)]
    public float waterRisingRate = 0.2f;

    public Sprite[] textures;
    int currentSprite = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = player.transform.position;
        initSpawnPosition = spawnPosition;
        Vector3[] vecArray = { Vector3.zero, Vector3.zero };
        rope = createLine(vecArray);
        breath = maxBreath;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Physics.SphereCast(player.transform.position, 0.1f, Vector3.back, out watRay, 3f);
        if (watRay.collider != null)
        {
            if (watRay.collider.tag == "Water")
            {
                if (inWater == false)
                {
                    Debug.Log("WATER");
                    Physics.gravity = Physics.gravity / 2.0f;
                }
                breath -= Time.deltaTime;
                inWater = true;
                if(breath < 0)
                {
                    breath = maxBreath;
                    player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    player.transform.position = spawnPosition;
                    water.transform.localScale = new Vector3(water.transform.localScale.x, (player.transform.position.y == -3f ? 10f : (player.transform.position.y == 23.93f ? 53f : (player.transform.position.y == 34.92f ? 87f : 10f))), water.transform.localScale.z);
                }
            }
            else
            {
                try
                {
                    watRay.collider.gameObject.GetComponent<checkPoint>().updateSpawn(player);
                } 
                catch (Exception e)
                {

                }
                try
                {
                    watRay.collider.gameObject.GetComponent<Win>().updateSpawn(player);

                }
                catch (Exception e)
                {

                }
            }
        }
        else
        {
            if (inWater == true)
            {
                Physics.gravity = Physics.gravity * 2.0f;

            }
            inWater = false;
        }

        if (adda(1, 2))
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
            {
                player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpStrength);
            }

        }

        if (Input.GetKey(KeyCode.R))
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = spawnPosition;
            breath = maxBreath;
            water.transform.localScale = new Vector3(water.transform.localScale.x, (player.transform.position.y == -3f ? 10f : (player.transform.position.y == 23.93f ? 53f : (player.transform.position.y == 34.92f ? 87f : 10f))), water.transform.localScale.z);

        }

        if (Input.GetMouseButton(0) && grapple && pull)
        {
            grapple = false;
            ropeDurationAt = 0;
            angle = Vector3.Normalize(new Vector3(mousePosition.x, mousePosition.y, player.transform.position.z) - player.transform.position);
            maxDistance = player.transform.position + ropeRange * angle;
        }
        else if (Input.GetMouseButton(1) && grapple && pull)
        {
            pull = false;
            ropeDurationAt = 0;
            angle = Vector3.Normalize(new Vector3(mousePosition.x, mousePosition.y, player.transform.position.z) - player.transform.position);
            maxDistance = player.transform.position + ropeRange * angle;
        }



        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (grapple)
                dir = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (grapple)
                dir = 7;
        }
        else
        {
            if (grapple)
                dir = 4;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.left * intensityMovementForce);
            if (grapple)
                dir -= 1;
            playerSprite.GetComponent<SpriteRenderer>().flipX = true;

            playerSprite.GetComponent<SpriteRenderer>().sprite = textures[currentSprite];
            currentSprite++;
            if (currentSprite > 3)
            {
                currentSprite = 1;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.right * intensityMovementForce);
            if (grapple)
                dir += 1;
            playerSprite.GetComponent<SpriteRenderer>().flipX = false;
            playerSprite.GetComponent<SpriteRenderer>().sprite = textures[currentSprite];
            currentSprite++;
            if (currentSprite > 4)
            {
                currentSprite = 1;
            }
        }
        else
        {
            playerSprite.GetComponent<SpriteRenderer>().sprite = textures[0];
            currentSprite = 0;
        }

        if (!grapple)
        {
            ropeDurationAt += Time.deltaTime;
            Vector3[] newVerts = { new Vector3(), new Vector3()};
            RaycastHit ray; //= Physics.SphereCastAll(Vector3.forward, 0, Vector3.forward);
            Physics.SphereCast(transform.position, 0.13f, transform.forward, out ray, 0);
            newVerts[0] = player.transform.position;
            newVerts[1] = Vector3.Lerp(player.transform.position, maxDistance, ropeDurationAt / ropeDuration);
            Physics.SphereCast(player.transform.position, 0.13f, angle, out ray, Vector2.Distance(newVerts[0], newVerts[1]));
            
            rope.GetComponent<LineRenderer>().SetPositions(newVerts);
            
            if (ray.collider != null)
            {
                if (ray.collider.tag == "Hang")
                {
                    if (ropeDurationAt > 0.05f)
                    {
                        player.GetComponent<Rigidbody>().AddForce(angle * grapplePower);
                    }
                }
                newVerts[0] = Vector3.zero;
                newVerts[1] = Vector3.zero;
                rope.GetComponent<LineRenderer>().SetPositions(newVerts);
                grapple = true;

            }



            if (!(ropeDurationAt < ropeDuration))
            {
                newVerts[0] = Vector3.zero;
                newVerts[1] = Vector3.zero;
                rope.GetComponent<LineRenderer>().SetPositions(newVerts);
                grapple = true;
            }

        }
        else if (!pull)
        {
            Vector3[] newVerts = { new Vector3(), new Vector3() };
            RaycastHit ray; //= Physics.SphereCastAll(Vector3.forward, 0, Vector3.forward);
            Physics.SphereCast(Vector3.zero, 0, Vector3.zero, out ray);
            if (!pullingObject)
            {
                ropeDurationAt += Time.deltaTime;
                Physics.SphereCast(transform.position, 0.13f, transform.forward, out ray, 0);
                newVerts[0] = player.transform.position;
                newVerts[1] = Vector3.Lerp(player.transform.position, maxDistance, ropeDurationAt / ropeDuration);
                Physics.SphereCast(player.transform.position, 0.13f, angle, out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                rope.GetComponent<LineRenderer>().SetPositions(newVerts);
            }
            else
            {
                ropeDurationAt -= Time.deltaTime;
                Physics.SphereCast(transform.position, 0.13f, transform.forward, out ray, 0);
                newVerts[0] = player.transform.position;
                newVerts[1] = Vector3.Lerp(player.transform.position, maxDistance, ropeDurationAt / ropeDuration);
                Physics.SphereCast(player.transform.position, 0.13f, angle, out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                rope.GetComponent<LineRenderer>().SetPositions(newVerts);

                currentPullPos = newVerts[1];

                Vector3 distance = currentPullPos - previousPullPos;

                pullingOBJ.transform.position = pullingOBJ.transform.position + distance;

                previousPullPos = newVerts[1];


            }

            if (ray.collider != null)
            {
                if (ray.collider.tag == "Pullable")
                {
                    if (ropeDurationAt > 0.1f)
                    {
                        pullingOBJ = ray.collider.gameObject;
                        pullingObject = true;
                        currentPullPos = newVerts[1];
                        previousPullPos = newVerts[1];
                    }
                    else
                    {
                        newVerts[0] = Vector3.zero;
                        newVerts[1] = Vector3.zero;
                        rope.GetComponent<LineRenderer>().SetPositions(newVerts);
                        pull = true;
                        ropeDurationAt = 0;
                    }
                }
                else
                {
                    //player.GetComponent<Rigidbody>().AddForce(angle * grapplePower);

                    newVerts[0] = Vector3.zero;
                    newVerts[1] = Vector3.zero;
                    rope.GetComponent<LineRenderer>().SetPositions(newVerts);
                    pull = true;
                    ropeDurationAt = 0;
                }
            }



            if (!(ropeDurationAt < ropeDuration))
            {
                newVerts[0] = Vector3.zero;
                newVerts[1] = Vector3.zero;
                rope.GetComponent<LineRenderer>().SetPositions(newVerts);
                pull = true;
                currentPullPos = Vector3.zero;
                previousPullPos = Vector3.zero;
                pullingObject = false;
                pullingOBJ = null;
                ropeDurationAt = 0;

            }
            else if (ropeDurationAt < 0)
            {
                newVerts[0] = Vector3.zero;
                newVerts[1] = Vector3.zero;
                rope.GetComponent<LineRenderer>().SetPositions(newVerts);
                pull = true;
                pullingObject = false;
                currentPullPos = Vector3.zero;
                previousPullPos = Vector3.zero;
                pullingOBJ = null;
                ropeDurationAt = 0;

            }
        }

        if (true)//spawnPosition != initSpawnPosition)
        {
            water.transform.localScale += new Vector3(0, waterRisingRate * Time.deltaTime, 0);
        }

    }

    private GameObject createLine(Vector3[] verts)
    {
        GameObject hook = new GameObject();
        LineRenderer line = hook.AddComponent<LineRenderer>().GetComponent<LineRenderer>();
        line.material = mat;
        line.startWidth = ropeThinkness;
        line.endWidth = ropeThinkness;
        line.positionCount = verts.Length;
        line.SetPositions(verts);
        return hook;
    }

    public bool adda(int i, int b)
    {
        bool didHit = Physics.Raycast(player.transform.position, Vector3.down, 0.77f);
        return didHit;
    }

    public void updateCheckPoint(Vector3 pos)
    {
        spawnPosition = pos;
    }

    public void manualWaterIncrease(float value)
    {
        water.transform.localScale = new Vector3(water.transform.localScale.x, value, water.transform.localScale.z);
    }

    public void changeWaterRaisingRate(float value)
    {
        waterRisingRate = value;
    }

}
