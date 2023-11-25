using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Observer : MonoBehaviour
{
    public static Observer instance = null;
    public int previousStage;

    public int[] score = { 0 };
    public int[] time = { 0 };
    public bool[] clear = { false };
    public bool gameClear = false;

    public bool stringAbility = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            previousStage = 2;
            score = new int[12];
            time = new int[12];
            clear = new bool[12];
            gameClear = false;
            stringAbility = false;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
