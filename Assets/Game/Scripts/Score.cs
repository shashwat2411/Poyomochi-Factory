using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    private Vector3 targetpos;
    private TextMeshProUGUI Scoretext; 
    public int score = 0;


    private void Awake()
    {
        Scoretext = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (score < 0) { score = 0; }
        if(score > 999) { score = 999; }
        Scoretext.text = "Score: " + (int)score;
    }
}
