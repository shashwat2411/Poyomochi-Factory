using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalPull : MonoBehaviour
{
    public float gravitationalPull;
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = transform.position - rb.transform.position;

            direction = Vector3.Normalize(direction);
            direction.z = 0f;
            direction *= gravitationalPull;

            rb.AddForce(direction, ForceMode.Force);
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        pull = false;
    //    }
    //}
}
