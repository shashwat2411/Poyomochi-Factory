using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewStageSelectController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;  //入力の制御クラスのインスタンス
    private NewStageSelectSoundPlayer soundPlayer;
    private Vector2 direction;
    private int[] levelScore = { 0 };
    private int[] levelTime = { 0 };
    private bool[] levelClear = { false };
    private GameObject player;
    private GameObject playerParent;
    public TextMeshProUGUI level;
    public TextMeshProUGUI score;
    public TextMeshProUGUI time;
    private float coolDown = 0.2f;
    public float counter = 0f;
    public int maxLevels = 12;
    public int selected;
    private int currentSelected;
    bool moving = false;
    Vector3 nextLocation;
    Vector3 speed;
    Vector3 difference;
    bool prevStage = false;
    int prevStageCounter = 0;
    private ParticleSystem landingEffect;
    public GameObject[] shadows;

    [Header("GameClear")]
    public bool GameClear = false;
    float gameClearCounter = 0f;
    public bool AllStageClear = false;
    public Animator gameOverAnimation;
    public GameObject mochi;
    bool playAnimation = false;
    bool playAnimation2 = false;
    bool perfectClear = false;

    [Header("Confirmation Menu")]
    bool confirmationMenu;
    bool zoom;
    Animator cameraAnimator;
    float transitionCounterZoomIn = 0f;
    float transitionCounterZoomOut = 0f;

    public GameObject[] levelOptions;
    // Start is called before the first frame update
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.started += Change;
        playerInputActions.Player.Back.started += MainMenu;

        levelScore = new int[maxLevels];
        levelTime = new int[maxLevels];
        levelClear = new bool[maxLevels];
        //levelOptions = GameObject.FindGameObjectsWithTag("Level");
        player = GameObject.FindGameObjectWithTag("Player");
        playerParent = GameObject.FindGameObjectWithTag("PlayerParent");
        cameraAnimator = GetComponent<Animator>();
        soundPlayer = GetComponentInChildren<NewStageSelectSoundPlayer>();
        landingEffect = GameObject.Find("LandingEffect").GetComponent<ParticleSystem>();

        landingEffect.Stop();

        GameClear = false;
        gameClearCounter = 0f;
    }
    
    private void Start()
    {
        selected = currentSelected = 1;
        Vector3 temp = levelOptions[selected - 1].transform.position;
        temp.y = playerParent.transform.position.y;
        playerParent.transform.position = temp;
        confirmationMenu = false;
        zoom = false;

        score.enabled = false;
        time.enabled = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameClear == false)
        {
            prevStageCounter++;
            if (prevStage == false && prevStageCounter == 2)
            {
                prevStage = true;
                Debug.Log("1. selected : " + selected + ", PreviousStage : " + (GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().previousStage - 1));
                selected = GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().previousStage - 2;
                Debug.Log("2. selected : " + selected + ", PreviousStage : " + (GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().previousStage - 1));

                for (int i = 0; i < maxLevels; i++)
                {
                    levelScore[i] = GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().score[i];
                    levelTime[i] = GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().time[i];
                    levelClear[i] = GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().clear[i];
                }
                if (selected < 1)
                {
                    selected = 1;
                }
                if(selected >= maxLevels)
                {
                    selected = maxLevels;
                }
                currentSelected = selected;
                Debug.Log("3. selected : " + selected + ", PreviousStage : " + (GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().previousStage - 1));
                Vector3 temp = levelOptions[selected - 1].transform.position;
                temp.y = playerParent.transform.position.y;
                playerParent.transform.position = temp;

                //Shadows
                {
                    if (levelClear[2] == true)
                    {
                        shadows[0].SetActive(false);
                    }
                    if (levelClear[5] == true)
                    {
                        shadows[1].SetActive(false);
                    }
                    if (levelClear[8] == true)
                    {
                        shadows[2].SetActive(false);
                    }
                    if (levelClear[maxLevels - 1] == true)
                    {
                        shadows[3].SetActive(false);
                    }
                }

                if (levelClear[maxLevels - 1] == true)
                {
                    if (GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().gameClear == false)
                    {
                        GameClear = true;
                        levelClear[maxLevels - 1] = false;
                        GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().gameClear = true;
                        int total = 0;
                        for(int i=0; i<maxLevels;i++)
                        {
                            total += GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().score[i];
                        }
                        if(total >= 600)
                        {
                            perfectClear = true;
                        }
                    }
                }
            }

            int world = ((selected - 1) / 3) + 1;
            int select = ((selected - 1) % 3) + 1;
            level.text = "World " + world + " - Level " + select;
            if (confirmationMenu == false && transitionCounterZoomIn <= 0f && transitionCounterZoomOut <= 0f) { direction = playerInputActions.Player.Direction.ReadValue<Vector2>(); }
            if (counter >= 0f) { counter -= Time.deltaTime; }
            else if (counter <= 0f && moving == false)
            {
                if (Mathf.Abs(direction.x) > 0.2f || Mathf.Abs(direction.y) > 0.2f)
                {
                    Vector2 previousPosition = new Vector2(levelOptions[selected - (selected - 2 >= 0 ? 2 : 1)].transform.position.x, levelOptions[selected - (selected - 2 >= 0 ? 2 : 1)].transform.position.z);
                    Vector2 currentPosition = new Vector2(levelOptions[selected - 1].transform.position.x, levelOptions[selected - 1].transform.position.z);
                    Vector2 nextPosition = new Vector2(levelOptions[selected - (selected < maxLevels ? 0 : 1)].transform.position.x, levelOptions[selected - (selected < maxLevels ? 0 : 1)].transform.position.z);

                    Vector2 nextDirection = nextPosition - currentPosition;
                    Vector2 previousDirection = previousPosition - currentPosition;
                    nextDirection.Normalize();
                    previousDirection.Normalize();

                    float inputAngle = Mathf.Atan2(direction.y, -direction.x) * 180f / 3.14f;
                    float nextAngle = Mathf.Atan2(nextDirection.y, -nextDirection.x) * 180f / 3.14f;
                    float previousAngle = Mathf.Atan2(previousDirection.y, -previousDirection.x) * 180f / 3.14f;

                    if (inputAngle < 0) { inputAngle = inputAngle + 360; }
                    if (nextAngle < 0) { nextAngle = nextAngle + 360; }
                    if (previousAngle < 0) { previousAngle = previousAngle + 360; }

                    Debug.Log("Next Direction : " + nextAngle + ",    Name : " + levelOptions[selected - (selected < maxLevels ? 0 : 1)].name + "    |     Input : " + inputAngle + "   |     Previous Direction : " + previousAngle + ",     Name : " + levelOptions[selected - (selected - 2 >= 0 ? 2 : 1)].name);

                    if (inputAngle <= nextAngle + 45f && inputAngle >= nextAngle - 45f && selected < maxLevels)
                    {
                        counter = coolDown;
                        if (selected < maxLevels)
                        {
                            soundPlayer.audioSource.PlayOneShot(soundPlayer.optionSwitch.file, soundPlayer.optionSwitch.volume);
                            if (levelClear[selected - 1] == true || AllStageClear == true)
                            {
                                selected++;
                            }
                        }
                    }
                    else if (inputAngle <= previousAngle + 45f && inputAngle >= previousAngle - 45f && selected >= 2)
                    {
                        counter = coolDown;
                        if (selected > 1)
                        {
                            soundPlayer.audioSource.PlayOneShot(soundPlayer.optionSwitch.file, soundPlayer.optionSwitch.volume);
                            selected--;
                        }
                    }
                }
            }


            if (currentSelected != selected && moving == false)
            {
                moving = true;
                nextLocation = levelOptions[selected - 1].transform.position;
                nextLocation.y = playerParent.transform.position.y;
                speed = nextLocation - playerParent.transform.position;
                player.GetComponent<Animator>().Play("StageSelectAnimation");
            }
            if (moving == true)
            {
                Vector3 faceDirection = nextLocation - playerParent.transform.position;
                Quaternion rotation = Quaternion.LookRotation(-faceDirection);
                GameObject.Find("mochi").transform.rotation = rotation;

                difference = playerParent.transform.position - nextLocation;
                Vector3 dir = -difference;
                dir.Normalize();
                dir *= Mathf.Sqrt(speed.sqrMagnitude) * Time.deltaTime;
                playerParent.transform.position += dir;

                if (difference.sqrMagnitude < 0.02f)
                {
                    //Vector3 cameraDirection = GameObject.FindAnyObjectByType<Camera>().transform.position - playerParent.transform.position;
                    Vector3 rotation2 = new Vector3(0f, 0f, -20f);
                    GameObject.Find("mochi").transform.eulerAngles = rotation2;

                    moving = false;
                    playerParent.transform.position = nextLocation;
                    difference = Vector3.zero;
                    nextLocation = Vector3.zero;
                    speed = Vector3.zero;
                    currentSelected = selected;
                    landingEffect.Play();
                }
            }

            if (cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("CameraZoomIn")) { transitionCounterZoomIn -= Time.deltaTime; }
            else { transitionCounterZoomIn = 0f; }
            if (cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("CameraZoomOut")) { transitionCounterZoomOut -= Time.deltaTime; }
            else { transitionCounterZoomOut = 0f; }

            if (zoom == true)
            {
                zoom = false;
                if (confirmationMenu == true)
                {
                    cameraAnimator.Play("CameraZoomIn");
                    transitionCounterZoomIn = 1f;
                }
                else if (confirmationMenu == false)
                {
                    cameraAnimator.Play("CameraZoomOut");
                    transitionCounterZoomOut = 1f;
                }
            }

            if (confirmationMenu == true)
            {
                if (cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    score.enabled = true;
                    time.enabled = true;
                    score.GetComponent<Animator>().Play("PopIn");
                    time.GetComponent<Animator>().Play("PopIn");

                    score.text = "Score : " + (levelScore[selected - 1] <= 0 ? "--" : levelScore[selected - 1]);
                    time.text = "Time : " + (levelTime[selected - 1] <= 0 ? "--" : levelTime[selected - 1]);
                }
            }
            else if (confirmationMenu == false)
            {
                //score.enabled = false;
                //time.enabled = false;
                score.GetComponent<Animator>().Play("PopOut");
                time.GetComponent<Animator>().Play("PopOut");
            }
        }
        else
        {
            gameClearCounter += Time.deltaTime;

            if (GameObject.FindAnyObjectByType<Fade>().gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && playAnimation == false)
            {
                cameraAnimator.Play("GameClear");
                playAnimation = true;
            }
            if (cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("GameClear") && cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && playAnimation2 == false && playAnimation == true)
            {
                mochi.SetActive(false);
                gameOverAnimation.enabled = true;
                if (perfectClear == false) { gameOverAnimation.Play("GameClearAnimation"); }
                else if(perfectClear == true) { gameOverAnimation.Play("PerfectClearAnimation"); }
                playAnimation2 = true;
            }
            //if((int)gameClearCounter == 2)
            //{
            //    gameClearCounter = 3f;
            //    GameClear = false;
            //    FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("Ending");
            //}
            if (gameOverAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                //gameClearCounter = 3f;
                GameClear = false;
                playerInputActions.Player.Disable();
                FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("Ending");
            }
        }
    }

    private void Change(InputAction.CallbackContext context)
    {
        if (context.started && transitionCounterZoomIn <= 0f)
        {
            if (confirmationMenu == false && moving == false)
            {
                soundPlayer.audioSource.PlayOneShot(soundPlayer.zoomIn.file, soundPlayer.zoomIn.volume);
                confirmationMenu = true;
                zoom = true;
            }
            else if (confirmationMenu == true)  
            {
                if (moving == false)
                {
                    soundPlayer.audioSource.PlayOneShot(soundPlayer.buttonPress.file, soundPlayer.buttonPress.volume);
                    int world = ((selected - 1) / 3) + 1;
                    int select = ((selected - 1) % 3) + 1;
                    string nextStage = "W " + world + " - L " + select;
                    playerInputActions.Player.Disable();
                    FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene(nextStage);
                }
            }
        }
    }

    private void MainMenu(InputAction.CallbackContext context)
    {
        if (context.started && transitionCounterZoomOut <= 0f)
        {
            if (confirmationMenu == true)
            {
                confirmationMenu = false;
                zoom = true;
                soundPlayer.audioSource.PlayOneShot(soundPlayer.zoomOut.file, soundPlayer.zoomOut.volume);
            }
            else if (confirmationMenu == false)
            {
                if (moving == false)
                {
                    playerInputActions.Player.Disable();
                    FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("MainMenu");
                }
            }
        }
    }
}
