using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPos;
    public GameObject Camera;

    public float parallaxEffect;
    public float parallaxMultiplier;
    float backUpY;
    // Start is called before the first frame update
    private void Awake()
    {
        //Camera = GameObject.FindAnyObjectByType<CameraFollow>().gameObject;
    }
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().size.x;

        parallaxEffect *= 2f;

        backUpY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;

        float temp = (Camera.transform.position.x * (1 - parallaxEffect));
        float distance = (Camera.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + distance, backUpY, transform.position.z);

        if(temp > startPos + length)
        {
            startPos += length;
        }
        else if(temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
