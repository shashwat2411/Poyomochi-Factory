using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissappear : MonoBehaviour
{
    float counter = 0f;
    public float timeLimit = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;

        if(counter >= timeLimit)
        {
            counter = 0f;
            Destroy(this.gameObject);
        }
    }
}
