using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPStaime : MonoBehaviour
{
    public int FPS = 30;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = FPS; // 30fps�ɐݒ�
    }

    // Update is called once per frame
    void Update()
    {

    }
}