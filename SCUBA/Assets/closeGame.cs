using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class closeGame : MonoBehaviour
{
    float waitTimer = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer < 0)
        {
            if (Input.anyKey || Input.GetMouseButtonDown(0))
            {
                    Application.Quit();

            }
        }
    }
}
