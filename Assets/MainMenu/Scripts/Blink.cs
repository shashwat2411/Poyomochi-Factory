using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blink : MonoBehaviour
{
    private float counter = 1f;
    public float speed = -1f;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        counter += speed * Time.deltaTime;
        if (counter <= 0f) { counter = 0f; speed *= -1; }
        else if(counter >= 1f) { counter = 1f;speed *= -1; }

        //text = Color.green;
        text.color = new Color(text.color.r, text.color.g, text.color.b, counter);
    }
}
