using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrusherBackground : MonoBehaviour
{
    public bool on = false;

    private void Awake()
    {
        transform.localScale = new Vector3(75f, 75f, 75f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Crusher")
        {
            if (other.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Crush"))
            {
                if (transform.localScale.y * 0.8f > 0.0f)
                {
                    on = true;
                    float backUp = -(transform.localScale.y * 0.2f) * 0.01f;
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.8f, transform.localScale.z);
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + backUp, transform.localPosition.z);
                }
            }
        }
    }
}
