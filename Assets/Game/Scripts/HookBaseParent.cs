using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBaseParent : MonoBehaviour
{
    // Start is called before the first frame update
    int counter = 0;
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Disappear"))
        {
            if(GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
