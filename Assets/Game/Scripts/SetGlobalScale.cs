using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetGlobalScaleScript
{
    // Start is called before the first frame update
    public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }
}
