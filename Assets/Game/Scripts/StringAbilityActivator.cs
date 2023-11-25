using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StringAbilityActivator : MonoBehaviour
{
    public int counter = 0;
    public bool collect = false;
    private GameObject player;
    public ParticleSystem collectEffect;
    public ParticleSystem existEffect;
    private GameSoundPlayer soundPlayer;

    private void Awake()
    {
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>();
        player = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
        collectEffect.Stop();
    }
    private void FixedUpdate()
    {
        if(collect == true)
        {
            counter += 1;

            if(counter >= 90)
            {
                counter = 0;
                collect = false;
                player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
                player.GetComponent<PlayerMovement>().playerInputActions.Enable();
            }
        }
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (GameObject.FindAnyObjectByType<Observer>() != null)
            {
                GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().stringAbility = true;
            }
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
            player.GetComponent<PlayerMovement>().playerInputActions.Disable();
            Destroy(GetComponent<BoxCollider>());
            GetComponentInChildren<Animator>().Play("Collection");
            existEffect.loop = false;
            collectEffect.Play();
            soundPlayer.audioSource.PlayOneShot(soundPlayer.chopsticks.file, soundPlayer.chopsticks.volume);

            collect = true;
        }
    }
}
