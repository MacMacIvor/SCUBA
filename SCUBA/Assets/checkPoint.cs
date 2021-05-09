using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateSpawn(GameObject player)
    {
        player.GetComponent<playerBehavior>().updateCheckPoint(new Vector3(transform.position.x, transform.position.y, 0));
    }
}
