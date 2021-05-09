using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleMove : MonoBehaviour
{
    [Range(-4, 4)]
    public int dir = 1;
    [Range(0, 30)]
    public float timerSaved = 1;
    float timer = 1;
    [Range(0, 30)]
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerSaved;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            dir *= -1;
            timer = timerSaved;
        }

        switch (dir)
        {
            case -4:
                transform.position -= Vector3.right * Time.deltaTime * speed;
                break;
            case -3:
                transform.position -= Vector3.down * Time.deltaTime * speed;
                break;
            case -2:
                transform.position -= Vector3.left * Time.deltaTime * speed;
                break;
            case -1:
                transform.position -= Vector3.up * Time.deltaTime * speed;
                break;
            case 1:
                transform.position += Vector3.up * Time.deltaTime * speed;
                break;
            case 2:
                transform.position += Vector3.left * Time.deltaTime * speed;
                break;
            case 3:
                transform.position += Vector3.down * Time.deltaTime * speed;
                break;
            case 4:
                transform.position += Vector3.right * Time.deltaTime * speed;
                break;

        }

    }
}
