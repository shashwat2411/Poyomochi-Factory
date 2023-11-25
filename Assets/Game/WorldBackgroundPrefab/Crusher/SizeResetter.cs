using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeResetter : MonoBehaviour
{
    private void Awake()
    {
        transform.parent.localScale = new Vector3(75f, 75f, 75f);
    }
}
