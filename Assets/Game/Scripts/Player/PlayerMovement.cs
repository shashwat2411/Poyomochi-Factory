using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //ジャンプ　→　未入力ジャンプ
    //ハイジャンプ　→　プレーヤーの入力でのジャンプ
    //空中ジャンプ　→　名前通りです
    [Header("Pendulum")]
    public bool debugStringAbilityActivate = false;
    public bool stringAbility = false;
    public GameObject rope;
    private GameObject instantiateCoordinates;
    public float throwForce = 10f;
    public bool throwPress = false;
    public float swingForce = 30f;
    public float hookMaxDistance = 10f;
    public bool distanceLimit = false;

    [Header("Jump")]
    public bool normalJump = true;
    public bool countDown = false;
    public int jumpReturnCounter = 0;
    public float JumpPower = 1f;    //空中ジャンプの威力
    public int jumpNum = 1;    //空中ジャンプの回数
    private bool stickToWall = false;

    [Header("矢印")]
    public GameObject arrow;    //矢印のオブジェクト

    [Header("Bounce")]
    public LayerMask whatIsWall;    //跳ねるサーフェスのレイヤー
    public float bounceForce = 6;//未入力のジャンプ力
    public float jumpForce;     //ハイジャンプの威力
    public float leftForce = 0.7f;
    private bool stay = false;
    private Collision stayWall;
    private float waitCounter = 0f;

    private MeshRenderer playerRenderer;    //プレーヤーの色を変えるため
    private GameSoundPlayer soundPlayer;
    public float Attack = 1;    //攻撃力
    public Vector2 direction;  //スティック入力を保存する変数

    public int MaxCounter = 0;  //ハイジャンプののクールタイムの上限
    public int counter = 0;     //ハイジャンプのクールタイム
    public bool add = false;    //ハイジャンプのクールダウンのフラグ

    public bool inWater = false;
    public float inWaterJumpCoolDown = 0f;
    public float inWaterCoolDown = 0.5f;

    [Header("References")]
   // public Renderer backgroundRenderer; //背景のオブジェクト
    private Rigidbody rb;   //プレーヤーのRigidbody
    public GameObject DustParticle;
    public GameObject doubleJumpParticle;

    [Header("Controls")]
    public PlayerInputActions playerInputActions;  //入力の制御クラスのインスタンス

    public bool paused = false;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.0f;
        //playerRenderer = GetComponent<MeshRenderer>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.started += Jump;
        playerInputActions.Player.Pause.started += Pause;
        playerInputActions.Player.Back.started += Back;

        playerInputActions.Player.Back.performed += Back;
        instantiateCoordinates = GameObject.Find("HookSpawner");
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>();

        inWater = false;
        inWaterJumpCoolDown = inWaterCoolDown;

        if (debugStringAbilityActivate == false)
        {
            if (GameObject.FindAnyObjectByType<Observer>() != null)
            {
                stringAbility = GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().stringAbility;
            }
        }
        else
        {
            stringAbility = true;
        }
    }

    private void Start()
    {
        soundPlayer.audioSource.PlayOneShot(soundPlayer.stageStart.file, soundPlayer.stageStart.volume);
    }

    private void FixedUpdate()
    {
        //スティックの入力
        direction = playerInputActions.Player.Direction.ReadValue<Vector2>();
        if ((Mathf.Abs(direction.x) > 0.2f || Mathf.Abs(direction.y) > 0.2f)) 
        {
            if (throwPress == true)
            {
                if(GameObject.FindFirstObjectByType<Hook>().GetComponent<Hook>().grab == false)
                {
                    arrow.GetComponentInChildren<SpriteRenderer>().enabled = true;
                }
                else
                {
                    arrow.GetComponentInChildren<SpriteRenderer>().enabled = false;
                }
            }
            else
            {
                arrow.GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
        }
        else { arrow.GetComponentInChildren<SpriteRenderer>().enabled = false; }

        //矢印の回転
        float angle = Mathf.Atan2(direction.y, direction.x) * 180f / 3.14f;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        //ハイジャンプのクールダウン
        if (add == true) { counter++; /*playerRenderer.material.color = Color.blue;*/ }
        if (counter >= MaxCounter) { add = false; counter = 0; /*playerRenderer.material.color = Color.red;*/ }

        if (normalJump == false && countDown == true)
        {
            jumpReturnCounter++;
            Vector3 originalSize = transform.localScale;
            originalSize.y += 0.011f;
            transform.localScale = originalSize;
            if (jumpReturnCounter >= 60)
            {
                originalSize = new Vector3(1f, 1f, 1f);
                transform.localScale = originalSize;
                jumpReturnCounter = 0;
                normalJump = true;
                countDown = false;
                playerInputActions.Player.Enable();
            }
        }

        if (stringAbility == true)
        {
            if (playerInputActions.Player.Shoot_n_Grab.ReadValue<float>() > 0.2f && distanceLimit == false)
            {
                if (Mathf.Abs(direction.x) > 0.2f || Mathf.Abs(direction.y) > 0.2f)
                {
                    if (throwPress == false)
                    {
                        Quaternion rot = new Quaternion(0f, 0f, Mathf.Atan2(direction.y, direction.x) * 180f / 3.14f, 0f);
                        GameObject str = Instantiate(rope, instantiateCoordinates.transform.position, rot);
                        str.GetComponent<Rigidbody>().velocity = direction * throwForce;
                        normalJump = false;
                        soundPlayer.audioSource.PlayOneShot(soundPlayer.StringShoot.file, soundPlayer.StringShoot.volume);
                    }
                    throwPress = true;
                }
            }
            if (playerInputActions.Player.Shoot_n_Grab.ReadValue<float>() <= 0.2f && throwPress == true)
            {
                GameObject[] ropes = GameObject.FindGameObjectsWithTag("Rope");
                foreach (GameObject r in ropes)
                {
                    if (r.GetComponentInChildren<ParticleSystem>() != null)
                    {
                        GameObject trailEffect = r.GetComponentInChildren<ParticleSystem>().gameObject;
                        trailEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        trailEffect.transform.SetParent(null, true);
                        trailEffect.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    }
                    if (r.GetComponentInChildren<HookBaseParent>() != null)
                    {
                        GameObject childMochi = r.GetComponentInChildren<HookBaseParent>().gameObject;
                        childMochi.transform.SetParent(null, true);
                        childMochi.transform.localScale = new Vector3(0.32f, 0.32f, 0.32f);
                        childMochi.GetComponentInChildren<Animator>().Play("Disappear");
                    }
                    Destroy(r);
                    soundPlayer.audioSource.PlayOneShot(soundPlayer.StringReturn.file, soundPlayer.StringReturn.volume);
                }
                throwPress = false;
                normalJump = true;

                if (this.gameObject.GetComponent<HingeJoint>() != null) { Destroy(this.gameObject.GetComponent<HingeJoint>()); }
            }

            if (distanceLimit == true)
            {
                if (playerInputActions.Player.Shoot_n_Grab.ReadValue<float>() <= 0.2f)
                {
                    distanceLimit = false;
                }
                GameObject[] ropes = GameObject.FindGameObjectsWithTag("Rope");
                foreach (GameObject r in ropes)
                {
                    if (r.GetComponentInChildren<ParticleSystem>() != null)
                    {
                        GameObject trailEffect = r.GetComponentInChildren<ParticleSystem>().gameObject;
                        trailEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        trailEffect.transform.SetParent(null, true);
                        trailEffect.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    }
                    if (r.GetComponentInChildren<HookBaseParent>() != null)
                    {
                        GameObject childMochi = r.GetComponentInChildren<HookBaseParent>().gameObject;
                        childMochi.transform.SetParent(null, true);
                        childMochi.transform.localScale = new Vector3(0.32f, 0.32f, 0.32f);
                        childMochi.GetComponentInChildren<Animator>().Play("Disappear");
                    }
                    Destroy(r);
                    soundPlayer.audioSource.PlayOneShot(soundPlayer.StringReturn.file, soundPlayer.StringReturn.volume);
                }
                throwPress = false;
                normalJump = true;

                if (this.gameObject.GetComponent<HingeJoint>() != null) { Destroy(this.gameObject.GetComponent<HingeJoint>()); }
            }
            if (throwPress == true)
            {
                if (GameObject.FindFirstObjectByType<Hook>().GetComponent<Hook>().grab == true)
                {
                    Vector3 force = direction * swingForce;
                    rb.AddForce(force);
                }
            }

            if (this.gameObject.GetComponent<HingeJoint>() != null)
            {
                if (GetComponent<HingeJoint>().connectedBody == null || GameObject.FindAnyObjectByType<Hook>() == null)
                {
                    Destroy(this.gameObject.GetComponent<HingeJoint>());
                }
            }
        }
        //if (stickToWall == true) {  }
        //else { rb.isKinematic = false; }
        //Debug.Log(angle);

        if(inWater == true)
        {
            if (inWaterJumpCoolDown > 0f)
            {
                inWaterJumpCoolDown -= Time.deltaTime;
            }
        }

    }



    //スペース押した瞬間の処理
    private void Jump(InputAction.CallbackContext context)
    {
        if (context.started && (jumpNum == 1 || (inWater == true && inWaterJumpCoolDown <= 0f)) && throwPress == false)
        {
            inWaterJumpCoolDown = inWaterCoolDown;
            if (inWater == false)
            {
                soundPlayer.audioSource.PlayOneShot(soundPlayer.DoubleJump.file, soundPlayer.DoubleJump.volume);
            }
            else
            {
                soundPlayer.audioSource.PlayOneShot(soundPlayer.bubbles.file, soundPlayer.bubbles.volume);
            }
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            stickToWall = false;
            //rb.isKinematic = false;
            //空中ジャンプ
            rb.velocity = rb.velocity * 0.3f;
            Vector2 input = direction;
            if (Mathf.Abs(input.x) <= 0f && Mathf.Abs(input.y) <= 0f) { input.y = 1f; }
            rb.AddForce(input * JumpPower, ForceMode.Impulse);

            Quaternion rot = new Quaternion(0f, 0f, Mathf.Atan2(direction.y, direction.x) * 180f / 3.14f, 0f);
            GameObject effect = Instantiate(doubleJumpParticle);
            effect.transform.SetParent(transform);
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localRotation = rot;
            jumpNum = 0;
        }
    }
    private void Pause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (paused == false)
            {
                paused = true;
                soundPlayer.audioSource.PlayOneShot(soundPlayer.pauseIn.file, soundPlayer.pauseIn.volume);
                GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().volume = 0.15f;
                Time.timeScale = 0f;
            }
        }
    }
    private void Back(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (paused == true)
            {
                //playerInputActions.Player.Disable();
                //Time.timeScale = 1f;
                //FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("NewStageSelect");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //6　はwhatIsWallのマスク番号だけどなぜか直接値を入れないちゃんと機能しない謎
        if (other.gameObject.layer == 6 || other.gameObject.layer == 7 || other.gameObject.layer == 8)
        {
            if (other.gameObject.tag == "StickyGround")
            {
                rb.constraints = RigidbodyConstraints.FreezePosition;
                rb.angularVelocity = new Vector3(0f, 0f, 0f);
                stickToWall = true;
                jumpNum = 1;
                soundPlayer.audioSource.PlayOneShot(soundPlayer.OnCollisionWithStickyWall.file, soundPlayer.OnCollisionWithStickyWall.volume);
            }
            if (Mathf.Abs(direction.x) > 0.2f || Mathf.Abs(direction.y) > 0.2f)
            {
                Quaternion rot = new Quaternion(Mathf.Atan2(other.contacts[0].normal.y, other.contacts[0].normal.x) * 180f / 3.14f, 90f, -90f, 0f);
                Instantiate(DustParticle, transform.position, rot);
            }
        }
    }
    void OnCollisionStay(Collision other)
    {
        //6　はwhatIsWallのマスク番号だけどなぜか直接値を入れないちゃんと機能しない謎
        if (other.gameObject.layer == 6 || other.gameObject.layer == 7 || other.gameObject.layer == 8)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= 2f)
            {
                waitCounter = 0f;
                jumpNum = 1;
            }
            if (other.gameObject.tag != "StickyGround" && normalJump == true)
            {
                jumpNum = 1;
                if (Mathf.Abs(direction.x) <= 0.2f && Mathf.Abs(direction.y) <= 0.2f)
                {
                    //未入力ジャンプ
                    rb.velocity = other.contacts[0].normal * bounceForce;
                    GameObject.FindAnyObjectByType<MeshDeformer>().GetComponent<MeshDeformer>().AddDeformingForce(other.contacts[0].point, GameObject.FindAnyObjectByType<MeshDeformerInput>().GetComponent<MeshDeformerInput>().force / 2f);
                    if (GetComponent<AudioSource>().isPlaying == false) { GetComponent<AudioSource>().Play(); }
                }
                else if (add == false)
                {
                    //ハイジャンプ
                    soundPlayer.audioSource.PlayOneShot(soundPlayer.HighJump.file, soundPlayer.HighJump.volume);
                    rb.velocity = (new Vector2(0, 0));
                    Vector2 input = direction;
                    float inputAngle = Mathf.Atan2(direction.y, direction.x) * 180f / 3.14f;    //プレーヤーの入力の角度
                    float normalAngle = Mathf.Atan2(other.contacts[0].normal.y, other.contacts[0].normal.x) * 180f / 3.14f;    //当たったグラウンドの直線の角度
                    float difference = normalAngle - inputAngle;    //上の二つの減算

                    if (Mathf.Abs(difference) < 75f)
                    {
                        jumpForce = 10f; 
                        rb.AddForce(input * jumpForce, ForceMode.Impulse);
                        GameObject.FindAnyObjectByType<MeshDeformer>().GetComponent<MeshDeformer>().AddDeformingForce(other.contacts[0].point, GameObject.FindAnyObjectByType<MeshDeformerInput>().GetComponent<MeshDeformerInput>().force);
                    }
                    else
                    {
                        jumpForce = 7f;
                        float anglePower = 1f;
                        if (Mathf.Abs(difference) > 90f) { anglePower = Mathf.Abs(180f - difference) / 90f; }
                        else { anglePower = 1f; }
                        if (anglePower >= 1f) { anglePower = 1f; }
                        /*rb.velocity = other.contacts[0].normal * bounceForce;*/
                        Vector2 corrected = Vector2.zero;
                        if (Mathf.Abs((int)normalAngle) == 90)
                        {
                            corrected = new Vector2(input.x * anglePower, other.contacts[0].normal.y);
                        }
                        else
                        {
                            corrected = new Vector2(other.contacts[0].normal.x, input.y * anglePower);
                        }

                        rb.AddForce(corrected * jumpForce * leftForce, ForceMode.Impulse);
                        GameObject.FindAnyObjectByType<MeshDeformer>().GetComponent<MeshDeformer>().AddDeformingForce(other.contacts[0].point, GameObject.FindAnyObjectByType<MeshDeformerInput>().GetComponent<MeshDeformerInput>().force / 2f);
                    }
                    add = true;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            if (collision.gameObject.tag == "StickyGround")
            {
                jumpNum = 0;
            }
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
            //GameObject.FindAnyObjectByType<MeshDeformer>().GetComponent<MeshDeformer>().AddDeformingForce(-collision.contacts[0].point, GameObject.FindAnyObjectByType<MeshDeformerInput>().GetComponent<MeshDeformerInput>().force);
        }
    }
}