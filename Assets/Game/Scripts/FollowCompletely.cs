using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCompletely : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = target.transform.position;
    }
}
