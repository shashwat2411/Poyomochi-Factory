using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public ParticleSystem rice;
    private Score Count;
    private GameSoundPlayer soundPlayer;
    void Start()
    {
        Count = GameObject.FindAnyObjectByType<Score>().GetComponent<Score>();
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>();
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // ÉvÉåÉCÉÑÅ[Ç™ê⁄êGÇ≈älìæîªíË
        if (other.tag == "Player")
        {
            Instantiate(rice, transform.position, Quaternion.identity);
            soundPlayer.audioSource.PlayOneShot(soundPlayer.coin.file, soundPlayer.coin.volume);
            Count.score += 10;
            Destroy(gameObject);
        }
    }
}
