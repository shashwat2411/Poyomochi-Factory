using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    private GameObject player;
    public PlayerInputActions resultInputActions;  //入力の制御クラスのインスタンス
    private GameSoundPlayer soundPlayer;
    public bool changed = false;
    public bool pause = false;
    int selected = 1;
    Vector2 direction;
    public float counter = 0f;
    public float coolDown = 0.3f;
    public GameObject[] options;
    public GameObject box;
    private GameObject stageClear;
    private GameObject gameOver;
    private GameObject paused;
    private new GameObject camera;

    public float waitTimer;
    bool wait;
    bool pressShrink;
    int shrinkCounter;

    bool appear = false;
    bool animate = false;

    public GameObject trashCan;

    public Vector2 selectedSize = new Vector2(120,120);
    public Vector2 originalSize = new Vector2(100,100);
    public Color selectedColor = Color.white;
    public Color originalColor = Color.black;
    // Start is called before the first frame update
    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;

        resultInputActions = new PlayerInputActions();
        resultInputActions.Player.Disable();

        resultInputActions.Player.Jump.started += Next;
        resultInputActions.Player.Pause.started += Pause;

        stageClear = GameObject.Find("StageClear");
        gameOver = GameObject.Find("GameOver");
        paused = GameObject.Find("Paused");

        //options = GameObject.FindGameObjectsWithTag("Option");
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>();

        camera = GameObject.FindAnyObjectByType<CameraFollow>().gameObject;

        waitTimer = 1f;
        wait = false;
        pressShrink = false;
        shrinkCounter = 0;

        appear = false;
        changed = false;

        animate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<WinCondition>().winCondition == true)
        {
            appear = true;
            stageClear.GetComponent<TextMeshProUGUI>().enabled = true;

            paused.GetComponent<TextMeshProUGUI>().enabled = false;
            gameOver.GetComponent<TextMeshProUGUI>().enabled = false;
            GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().volume = 0.1f;
        }

        if (player.GetComponent<WinCondition>().loseCondition == true && animate == false)
        {
            animate = true;
            trashCan.SetActive(true);
            trashCan.GetComponent<Animator>().enabled = true;
            //trashCan.GetComponent<Animator>().Play("ThrowOut");
            player.GetComponent<Rigidbody>().isKinematic = true;
            GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().volume = 0.1f;
        }
        if(player.GetComponent<WinCondition>().loseCondition == true && trashCan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            appear = true;
            gameOver.GetComponent<TextMeshProUGUI>().enabled = true;

            paused.GetComponent<TextMeshProUGUI>().enabled = false;
            stageClear.GetComponent<TextMeshProUGUI>().enabled = false;
        }

        if (player.GetComponent<PlayerMovement>().paused == true && pause == false)
        {
            appear = true;
            pause = true;
            paused.GetComponent<TextMeshProUGUI>().enabled = true;

            stageClear.GetComponent<TextMeshProUGUI>().enabled = false;
            gameOver.GetComponent<TextMeshProUGUI>().enabled = false;
        }
        else if(player.GetComponent<PlayerMovement>().paused == false && pause == true)
        {
            appear = false;
            pause = false;
        }

        if(appear == true && changed == false)
        {
            changed = true;
            player.GetComponent<PlayerMovement>().playerInputActions.Disable();
            resultInputActions.Player.Enable();

            for (int i = 0; i < options.Length; i++)
            {
                options[i].GetComponentsInChildren<Image>()[0].enabled = true;
                options[i].GetComponentsInChildren<Image>()[1].enabled = true;
            }
            box.GetComponentsInChildren<Image>()[0].enabled = true;
            box.GetComponentsInChildren<Image>()[1].enabled = true;

            box.GetComponentsInChildren<Animator>()[0].Play("MenuBoxAppear");
            box.GetComponentsInChildren<Animator>()[1].Play("MenuBoxAppear");
        }

        if(appear == false && changed == true)
        {
            changed = false;
            player.GetComponent<PlayerMovement>().playerInputActions.Enable();
            resultInputActions.Player.Disable();

            for (int i = 0; i < options.Length; i++)
            {
                //options[i].GetComponentsInChildren<Image>()[0].enabled = false;
                //options[i].GetComponentsInChildren<Image>()[1].enabled = false;
            }
            //paused.GetComponent<TextMeshProUGUI>().enabled = false;
            //stageClear.GetComponent<TextMeshProUGUI>().enabled = false;
            //gameOver.GetComponent<TextMeshProUGUI>().enabled = false;

            //box.GetComponentsInChildren<Image>()[0].enabled = false;
            //box.GetComponentsInChildren<Image>()[1].enabled = false;

            box.GetComponentsInChildren<Animator>()[0].Play("MenuBoxDissappear");
            box.GetComponentsInChildren<Animator>()[1].Play("MenuBoxDissappear");
        }

        if (box.GetComponentsInChildren<Animator>()[1].GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            direction = resultInputActions.Player.Direction.ReadValue<Vector2>();
        }
        if (counter >= 0f) { counter -= 1f / 60f; }
        else if (counter <= 0f)
        {
            if (direction.x > 0.5f) 
            {
                soundPlayer.audioSource.PlayOneShot(soundPlayer.optionSwitch.file, soundPlayer.optionSwitch.volume);
                counter = coolDown; 
                selected++; 
                if (selected > 3) { selected = 1; } 
            }
            else if (direction.x < -0.5f) 
            {
                soundPlayer.audioSource.PlayOneShot(soundPlayer.optionSwitch.file, soundPlayer.optionSwitch.volume);
                counter = coolDown; 
                selected--; 
                if (selected < 1) { selected = 3; } 
            }
        }

        if (pressShrink == false)
        {
            for (int i = 0; i < options.Length; i++)
            {
                options[i].GetComponentsInChildren<Image>()[0].rectTransform.sizeDelta = originalSize;

                options[i].GetComponentsInChildren<Image>()[1].color = new Color(originalColor.r, originalColor.g, originalColor.b, options[i].GetComponentsInChildren<Image>()[1].color.a);
                options[i].GetComponentsInChildren<Image>()[1].rectTransform.sizeDelta = originalSize;
            }

            options[selected - 1].GetComponentsInChildren<Image>()[0].rectTransform.sizeDelta = selectedSize;

            options[selected - 1].GetComponentsInChildren<Image>()[1].color = new Color(selectedColor.r, selectedColor.g, selectedColor.b, options[selected - 1].GetComponentsInChildren<Image>()[1].color.a);
            options[selected - 1].GetComponentsInChildren<Image>()[1].rectTransform.sizeDelta = selectedSize;
        }
        else
        {
            options[selected - 1].GetComponentsInChildren<Image>()[0].rectTransform.sizeDelta = selectedSize * 0.7f;
            options[selected - 1].GetComponentsInChildren<Image>()[1].rectTransform.sizeDelta = selectedSize * 0.7f;
            shrinkCounter++;
            if (shrinkCounter == 10) { pressShrink = false; shrinkCounter = 0; }
        }

        if (wait == true)
        {
            waitTimer -= Time.deltaTime;
        }
        if (waitTimer <= 0f && wait == true)
        {
            wait = false;
            waitTimer = 0f;

            string next;
            if (player.GetComponent<WinCondition>().winCondition == true)
            {
                if (GameObject.FindAnyObjectByType<Observer>() != null)
                {
                    GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().score[SceneManager.GetActiveScene().buildIndex - 3] = camera.GetComponent<Score>().score;
                    GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().time[SceneManager.GetActiveScene().buildIndex - 3] = (int)camera.GetComponent<Timer>().timeLimit;
                    GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().clear[SceneManager.GetActiveScene().buildIndex - 3] = true;
                }
            }
            else
            {
                if (GameObject.FindAnyObjectByType<Observer>() != null)
                {
                    GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().stringAbility = false;
                }
            }
            if (selected == 1) { next = SceneManager.GetActiveScene().name; }
            else if (selected == 2) { next = "NewStageSelect"; }
            else { next = "MainMenu"; }

            player.GetComponent<PlayerMovement>().paused = false;
            FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene(next);
        }
    }

    private void Next(InputAction.CallbackContext context)
    {
        if (context.started && box.GetComponentsInChildren<Animator>()[1].GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            soundPlayer.audioSource.PlayOneShot(soundPlayer.buttonPress.file, soundPlayer.buttonPress.volume);

            pressShrink = true;
            wait = true;
            resultInputActions.Player.Disable();
            Time.timeScale = 1f;

        }
    }
    private void Pause(InputAction.CallbackContext context)
    {
        if (context.started && box.GetComponentsInChildren<Animator>()[1].GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            if(player.GetComponent<PlayerMovement>().paused == true)
            {
                player.GetComponent<PlayerMovement>().paused = false;
                soundPlayer.audioSource.PlayOneShot(soundPlayer.pauseOut.file, soundPlayer.pauseOut.volume);
                GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().volume = 0.3f;
                Time.timeScale = 1f;    
            }
        }
    }
}
