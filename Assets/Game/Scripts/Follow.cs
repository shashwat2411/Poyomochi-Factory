using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public bool yAxis = false;
    public Transform target;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (yAxis == false)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}
