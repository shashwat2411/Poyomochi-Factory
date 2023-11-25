using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public float counter = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter -= Time.deltaTime;
        if(counter < 0f) { Destroy(gameObject); }
        this.transform.localScale = new Vector3(this.transform.localScale.x, counter / 10f, this.transform.localScale.z);
    }
}
