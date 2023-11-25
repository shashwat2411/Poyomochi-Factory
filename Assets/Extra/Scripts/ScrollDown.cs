using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollDown : MonoBehaviour
{
    private Transform mesh;
    public float ScrollSpeed = 10f; 
    // Start is called before the first frame update
    void Awake()
    {
        mesh = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FindAnyObjectByType<Fade>().GetComponent<Fade>().FadeOut == false)
        {
            Vector3 pY = new Vector3(mesh.position.x, mesh.position.y + ScrollSpeed * Time.deltaTime, mesh.position.z);
            mesh.position = pY;
        }
    }
}
