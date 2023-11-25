using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private Rigidbody rb;
    private GameSoundPlayer soundPlayer;
    private GameObject player;
    private LineRenderer lineRenderer;
    private Vector3 previousPosition;
    public float totalDistanceTravelled;
    public bool grab = false;
    public float returnSpeed = 20f;
    float returnSpeedBackup;
    public GameObject hookBase;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
        lineRenderer = GetComponentInChildren<LineRenderer>();

        lineRenderer.enabled = false;
        grab = false;

        previousPosition = transform.position;

        returnSpeedBackup = returnSpeed;

        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>();
    }

    private void Start()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(grab == false)
        {
            float distanceTravelled = Vector3.Distance(transform.position, previousPosition);
            totalDistanceTravelled += distanceTravelled;
            previousPosition = transform.position;

            //Debug.Log("Distance Travelled : " + totalDistanceTravelled);

            if(totalDistanceTravelled > player.GetComponent<PlayerMovement>().hookMaxDistance)
            {
                player.GetComponent<PlayerMovement>().distanceLimit = true;
            }

        }

        if (lineRenderer.enabled == true)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && player.GetComponent<PlayerMovement>().throwPress == true)
        {
            Debug.Log(other.gameObject.name);
            grab = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            GameObject Base = Instantiate(hookBase, transform.position, Quaternion.identity);
            Base.transform.SetParent(transform, true);
            Base.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //float size = 5.0f;
            //Base.transform.localScale = new Vector3(size, size, size);
            if (other.tag == "Rotate" || other.tag == "Ice" || other.tag == "DirtyGround" || other.tag == "handle" || other.tag == "Crusher")
            {
                transform.SetParent(other.transform, true);
                //transform.localScale *= 5.0f;
            }
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            player.AddComponent<HingeJoint>();
            HingeJoint joint = player.GetComponent<HingeJoint>();
            joint.connectedBody = rb;
            joint.axis = new Vector3(0f, 0f, 1f);
            joint.useMotor = true;
            joint.useAcceleration = true;
            lineRenderer.enabled = true;
            soundPlayer.audioSource.PlayOneShot(soundPlayer.StringHook.file, soundPlayer.StringHook.volume);
            //Debug.Break();
        }
    }

}
