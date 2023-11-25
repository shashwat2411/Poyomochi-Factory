 using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LogoControls : MonoBehaviour
{
    private PlayerInputActions playerInputActions;  //入力の制御クラスのインスタンス
    bool next = false;
    float counter = 0f;
    bool press = false;
    bool sound = false;
    public Animator logo;
    // Start is called before the first frame update
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.started += Change;

        next = false;
        press = false;
        counter = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;
        if ((int)counter == 2)
        {
            counter = 3f;
            next = true;
        }
        if (GameObject.FindAnyObjectByType<Fade>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            if (GetComponent<AudioSource>().isPlaying == false && sound == false)
            {
                GetComponent<AudioSource>().Play();
                logo.Play("logo");
                sound = true;
            }
        }

        if(next == true || press == true)
        {
            next = false;
            press = false;
            FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("MainMenu");
        }

    }

    private void Change(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            press = true;
        }
    }
}