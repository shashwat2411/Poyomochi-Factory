using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBlock : MonoBehaviour
{
    private WinCondition win;
    private GameSoundPlayer soundPlayer;
    // Start is called before the first frame update
    void Start()
    {
        win = GameObject.FindAnyObjectByType<WinCondition>().GetComponent<WinCondition>();
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>().GetComponent<GameSoundPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Win");
            win.winCondition = true;
            soundPlayer.audioSource.PlayOneShot(soundPlayer.stageClear.file, soundPlayer.stageClear.volume);
        }
    }
}
