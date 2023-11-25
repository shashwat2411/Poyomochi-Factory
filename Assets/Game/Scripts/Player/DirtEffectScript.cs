using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirtEffectScript : MonoBehaviour
{
    private DirtValue DirtValueScriptReference;
    Color temp;
    // Start is called before the first frame update
    void Start()
    {
        DirtValueScriptReference = FindObjectOfType<DirtValue>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        temp = GetComponent<Image>().color;
        temp.a = DirtValueScriptReference.Dirt/ DirtValueScriptReference.MaxDirt;
        GetComponent<Image>().color = temp;
    }
}
