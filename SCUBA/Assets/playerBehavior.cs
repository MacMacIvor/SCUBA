using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior : MonoBehaviour
{
    public GameObject player;
    private GameObject rope;
    public Material mat;
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

    public int dir = 0;

    //0  1  2
    //3  4  5
    //6  7  8


    private bool grapple = true;
    private bool inWater = false;
    RaycastHit watRay; //= Physics.SphereCastAll(Vector3.forward, 0, Vector3.forward);

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] vecArray = { Vector3.zero, Vector3.zero };
        rope = createLine(vecArray);
    }

    // Update is called once per frame
    void Update()
    {
        Physics.SphereCast(player.transform.position, 0.1f, Vector3.back, out watRay, 0.2f);
        if (watRay.collider != null)
        {
            if (inWater == false)
            {
                Physics.gravity = Physics.gravity / 8.0f;
            }
            inWater = true;
        }
        else
        {
            if (inWater == true)
            {
                Physics.gravity = Physics.gravity * 8.0f;

            }
            inWater = false;
        }


        if (adda(1,2))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpStrength);
            }

            if (Input.GetKey(KeyCode.LeftShift) && grapple)
            {
                grapple = false;
                ropeDurationAt = 0;
            }
        }



        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (grapple)
                dir = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (grapple)
                dir = 7;
        }
        else
        {
            if (grapple)
                dir = 4;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.left * intensityMovementForce);
            if (grapple)
                dir -= 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.right * intensityMovementForce);
            if (grapple)
                dir += 1;
        }
        
        drawLine();

        if (!grapple)
        {
            ropeDurationAt += Time.deltaTime;
            Vector3[] newVerts = { new Vector3(), new Vector3()};
            RaycastHit ray; //= Physics.SphereCastAll(Vector3.forward, 0, Vector3.forward);
            Physics.SphereCast(transform.position, 0.13f, transform.forward, out ray, 0);
            switch (dir)
            {
                case 0:
                    newVerts[0] = player.transform.position;
                    newVerts[1] = Vector3.Lerp(player.transform.position, player.transform.position + ropeRange * new Vector3(-0.707f, 0.707f, 0), ropeDurationAt / ropeDuration);
                    Physics.SphereCast(player.transform.position, 0.13f, new Vector3(-0.707f, 0.707f, 0), out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                    break;
                case 1:
                    newVerts[0] = player.transform.position;
                    newVerts[1] = Vector3.Lerp(player.transform.position, player.transform.position + ropeRange * new Vector3(0, 1, 0), ropeDurationAt / ropeDuration);
                    Physics.SphereCast(player.transform.position, 0.13f, new Vector3(0, 1, 0), out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                    break;
                case 2:
                    newVerts[0] = player.transform.position;
                    newVerts[1] = Vector3.Lerp(player.transform.position, player.transform.position + ropeRange * new Vector3(0.707f, 0.707f, 0), ropeDurationAt / ropeDuration);
                    Physics.SphereCast(player.transform.position, 0.13f, new Vector3(0.707f, 0.707f, 0), out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                    break;
                case 3:
                    newVerts[0] = player.transform.position;
                    newVerts[1] = Vector3.Lerp(player.transform.position, player.transform.position + ropeRange * new Vector3(-1, 0, 0), ropeDurationAt / ropeDuration);
                    Physics.SphereCast(player.transform.position, 0.13f, new Vector3(-1, 0, 0), out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                    break;
                case 4:
                    break;
                case 5:
                    newVerts[0] = player.transform.position;
                    newVerts[1] = Vector3.Lerp(player.transform.position, player.transform.position + ropeRange * new Vector3(1, 0, 0), ropeDurationAt / ropeDuration);
                    Physics.SphereCast(player.transform.position, 0.13f, new Vector3(-1, 0, 0), out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                    break;
                case 6:
                    newVerts[0] = player.transform.position;
                    newVerts[1] = Vector3.Lerp(player.transform.position, player.transform.position + ropeRange * new Vector3(-0.707f, -0.707f, 0), ropeDurationAt / ropeDuration);
                    Physics.SphereCast(player.transform.position, 0.13f, new Vector3(-0.707f, -0.707f, 0), out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                    break;
                case 7:
                    newVerts[0] = player.transform.position;
                    newVerts[1] = Vector3.Lerp(player.transform.position, player.transform.position + ropeRange * new Vector3(0, -1, 0), ropeDurationAt / ropeDuration);
                    Physics.SphereCast(player.transform.position, 0.13f, new Vector3(0, -1, 0), out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                    break;
                case 8:
                    newVerts[0] = player.transform.position;
                    newVerts[1] = Vector3.Lerp(player.transform.position, player.transform.position + ropeRange * new Vector3(0.707f, -0.707f, 0), ropeDurationAt / ropeDuration);
                    Physics.SphereCast(player.transform.position, 0.13f, new Vector3(0.707f, -0.707f, 0), out ray, Vector2.Distance(newVerts[0], newVerts[1]));
                    break;
            }
            rope.GetComponent<LineRenderer>().SetPositions(newVerts);
            
            if (ray.collider != null)
            {

                switch (dir)
                {
                    case 0:
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(-0.707f, 0.707f, 0) * grapplePower);
                        break;
                    case 1:
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * grapplePower);

                        break;
                    case 2:
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(0.707f, 0.707f, 0) * grapplePower);

                        break;
                    case 3:
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * grapplePower);

                        break;
                    case 4:
                        ;
                        break;
                    case 5:
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * grapplePower);

                        break;
                    case 6:
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(-0.707f, -0.707f, 0) * grapplePower);

                        break;
                    case 7:
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * grapplePower);

                        break;
                    case 8:
                        player.GetComponent<Rigidbody>().AddForce(new Vector3(0.707f, -0.707f, 0) * grapplePower);

                        break;

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

    public void drawLine()
    {
        switch (dir)
        {
            //0  1  2
            //3  4  5
            //6  7  8
            case 0:
                
                Debug.DrawLine(player.transform.position, player.transform.position + ropeRange * new Vector3(-0.707f, 0.707f, 0));

                break;
            case 1:
                Debug.DrawLine(player.transform.position, player.transform.position + ropeRange * new Vector3(0, 1, 0));

                break;
            case 2:
                Debug.DrawLine(player.transform.position, player.transform.position + ropeRange * new Vector3(0.707f, 0.707f, 0));

                break;
            case 3:
                Debug.DrawLine(player.transform.position, player.transform.position + ropeRange * new Vector3(-1, 0, 0));

                break;
            case 4:
                ;
                break;
            case 5:
                Debug.DrawLine(player.transform.position, player.transform.position + ropeRange * new Vector3(1, 0, 0));

                break;
            case 6:
                Debug.DrawLine(player.transform.position, player.transform.position + ropeRange * new Vector3(-0.707f, -0.707f, 0));

                break;
            case 7:
                Debug.DrawLine(player.transform.position, player.transform.position + ropeRange * new Vector3(0, -1, 0));

                break;
            case 8:
                Debug.DrawLine(player.transform.position, player.transform.position + ropeRange * new Vector3(0.707f, -0.707f, 0));

                break;
        }
    }

}
