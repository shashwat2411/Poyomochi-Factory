using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyGround : MonoBehaviour
{
    public float AddValue = 0f;
    private GameSoundPlayer soundPlayer;
    private void Awake()
    {
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>().GetComponent<GameSoundPlayer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            soundPlayer.audioSource.PlayOneShot(soundPlayer.OnCollisionWithDirt.file, soundPlayer.OnCollisionWithDirt.volume);
            collision.gameObject.GetComponent<DirtValue>().CameraShakeOnDirtyGround(AddValue / collision.gameObject.GetComponent<DirtValue>().MaxDirt * 5f);
            if (collision.gameObject.GetComponent<DirtValue>().Dirt < collision.gameObject.GetComponent<DirtValue>().MaxDirt) 
            {
                float dirt = collision.gameObject.GetComponent<DirtValue>().Dirt;
                if (dirt + AddValue <= collision.gameObject.GetComponent<DirtValue>().MaxDirt)
                {
                    dirt += AddValue;
                }
                else
                {
                    dirt = collision.gameObject.GetComponent<DirtValue>().MaxDirt;
                }
                collision.gameObject.GetComponent<DirtValue>().Dirt = dirt;
            }
        }
    }
}
