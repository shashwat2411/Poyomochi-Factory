 using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MainMenuControls : MonoBehaviour
{
    private PlayerInputActions playerInputActions;  //入力の制御クラスのインスタンス
    private bool pressed = false;
    public int selected = 1;
    public GameObject[] Options;
    public GameObject pressA;
    Vector2 direction;
    private float coolDown = 0.2f;
    public float counter = 0f;
    public Color selectedColor;
    public Color originalColor;
    public Vector2 selectedSize;
    public Vector2 originalSize;
    private MainMenuSoundPlayer soundPlayer;

    float waitTimer;
    bool wait;
    bool pressShrink;
    int shrinkCounter;

    // Start is called before the first frame update
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.started += Change;
        //Options = GameObject.FindGameObjectsWithTag("Option");
        soundPlayer = GetComponentInChildren<MainMenuSoundPlayer>();

        for (int i = 0; i < 2; i++) 
        {
            Options[i].GetComponent<TextMeshProUGUI>().enabled = false;
        }

        waitTimer = 1f;
        wait = false;
        pressShrink = false;
        shrinkCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed == true)
        {
            direction = playerInputActions.Player.Direction.ReadValue<Vector2>();

            if (counter >= 0f) { counter -= Time.deltaTime; }
            else if (counter <= 0f)
            {
                if (direction.y < -0.5f) 
                {
                    soundPlayer.audioSource.PlayOneShot(soundPlayer.optionSwitch.file, soundPlayer.optionSwitch.volume);
                    counter = coolDown; 
                    selected++; 
                    if (selected > 2) { selected = 1; } 
                }
                else if (direction.y > 0.5f)
                {
                    soundPlayer.audioSource.PlayOneShot(soundPlayer.optionSwitch.file, soundPlayer.optionSwitch.volume);
                    counter = coolDown; 
                    selected--; 
                    if (selected < 1) { selected = 2; } 
                }
            }

            if (pressShrink == false)
            {
                for (int i = 0; i < Options.Length; i++)
                {
                    Options[i].GetComponent<TextMeshProUGUI>().color = originalColor;
                    Options[i].GetComponent<TextMeshProUGUI>().rectTransform.localScale = originalSize;
                }

                Options[selected - 1].GetComponent<TextMeshProUGUI>().color = selectedColor;
                Options[selected - 1].GetComponent<TextMeshProUGUI>().rectTransform.localScale = selectedSize;
            }
            else
            {
                Options[selected - 1].GetComponent<TextMeshProUGUI>().rectTransform.localScale = selectedSize * 0.7f;
                shrinkCounter++;
                if (shrinkCounter == 10) { pressShrink = false; shrinkCounter = 0; }
            }


            if(wait == true)
            {
                waitTimer -= Time.deltaTime;
            }
            if(waitTimer <= 0f && wait == true)
            {
                wait = false;
                waitTimer = 0f;

                if (selected == 1)
                {
                    playerInputActions.Player.Disable();
                    FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("NewStageSelect");
                }
                else if(selected == 2)
                {
                    //UnityEditor.EditorApplication.isPlaying = false;
                    Application.Quit();
                }
                else
                {
                    playerInputActions.Player.Disable();
                    FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("NewStageSelect");
                }
            }
        }
    }

    private void Change(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            soundPlayer.audioSource.PlayOneShot(soundPlayer.buttonPress.file, soundPlayer.buttonPress.volume);
            if (pressed == true)
            {
                pressShrink = true;
                wait = true;
                playerInputActions.Player.Disable();

            }
            else
            {
                pressed = true;
                selected = 1;

                pressA.GetComponent<TextMeshProUGUI>().enabled = false;
                for (int i = 0; i < Options.Length; i++) { Options[i].GetComponent<TextMeshProUGUI>().enabled = true; }
            }
        }
    }
}