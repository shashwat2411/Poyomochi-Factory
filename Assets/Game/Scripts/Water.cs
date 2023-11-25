using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Water : MonoBehaviour
{
    public ParticleSystem waterEffect;
    public ParticleSystem bubbleEffect;
    public float waterDrag = 3.0f;
    private GameSoundPlayer soundPlayer;
    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            soundPlayer.audioSource.PlayOneShot(soundPlayer.waterEnter.file, soundPlayer.waterEnter.volume);
            other.gameObject.GetComponent<Rigidbody>().drag = waterDrag;
            other.gameObject.GetComponent<PlayerMovement>().inWater = true;
            Instantiate(waterEffect, other.transform.position, Quaternion.identity);
            GameObject particle = Instantiate(bubbleEffect).gameObject;
            particle.transform.SetParent(other.transform, false);
            particle.transform.localPosition = new Vector3(0f, -0.4f, 0f);
            soundPlayer.audioSource.PlayOneShot(soundPlayer.waterEnter.file, soundPlayer.waterEnter.volume);
            GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().volume = 0.2f;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().drag = 0.0f;
            other.gameObject.GetComponent<PlayerMovement>().inWater = false;
            Instantiate(waterEffect, other.transform.position, Quaternion.identity);
            ParticleSystem[] list = other.GetComponentsInChildren<ParticleSystem>();
            soundPlayer.audioSource.PlayOneShot(soundPlayer.waterExit.file, soundPlayer.waterExit.volume);
            GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().volume = 0.3f;
            for (int i = 0; i < list.Length; i++)
            {
                Destroy(list[i].gameObject);
            }
        }
    }
}