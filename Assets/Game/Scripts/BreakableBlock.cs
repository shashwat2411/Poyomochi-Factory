using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem breakEffect;
    private PlayerMovement pm;
    private GameSoundPlayer soundPlayer;
    bool doubleJumpCheck = false;
    void Start()
    {
        pm = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<PlayerMovement>();
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>().GetComponent<GameSoundPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().jumpNum == 0)
            {
                doubleJumpCheck = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (doubleJumpCheck == true)
            {
                if (breakEffect != null)
                {
                    soundPlayer.audioSource.PlayOneShot(soundPlayer.OnCollisionWithBreakableWall.file, soundPlayer.OnCollisionWithBreakableWall.volume);
                    Instantiate(breakEffect, collision.gameObject.transform.position, transform.rotation);
                }
                collision.gameObject.GetComponent<PlayerMovement>().jumpNum = 0;
                Destroy(this.gameObject);
            }
        }
    }
}
