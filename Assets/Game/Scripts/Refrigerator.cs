using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    public GameObject ice;
    private float counter;
    public float interval;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;
        if(counter >= interval)
        {
            SpitOut();
            counter -= interval;
        }
    }

    void SpitOut()
    {
        Vector3 Position = this.gameObject.transform.position;
        Position.y += this.gameObject.transform.localScale.y / 2 + ice.transform.localScale.y / 2;
        GameObject clone = Instantiate(ice, Position, this.gameObject.transform.rotation);
        clone.GetComponent<Rigidbody>().AddForce(transform.up * 10f, ForceMode.Impulse);
    }
}
