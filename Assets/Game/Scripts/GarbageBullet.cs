using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem MudEffect;
    public GameObject mud;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ぶつかったオブジェクトのタグを取得
        string objTag = collision.gameObject.tag;
        if (collision.gameObject.layer == 6)
        {
            if (objTag != "DirtyGround")
            {
                float normalAngle = Mathf.Atan2(collision.contacts[0].normal.y, collision.contacts[0].normal.x) * 180f / 3.14f;
                Quaternion ang = Quaternion.identity;
                ang.eulerAngles = new Vector3(0f, 0f, normalAngle - 90f);
                GameObject newGround = Instantiate(mud, transform.position, ang);
            }
        }

        Quaternion angle = Quaternion.identity;
        angle.eulerAngles = new Vector3(-90f, 0f, 0f);
        Instantiate(MudEffect, new Vector3(transform.position.x, transform.position.y, 0f), angle);
        if(GetComponentInChildren<Hook>() != null)
        {
            GetComponentInChildren<Hook>().GetComponent<Hook>().grab = false;
            GetComponentInChildren<Hook>().gameObject.transform.SetParent(null, true);
        }
        Destroy(gameObject);
    }
}
