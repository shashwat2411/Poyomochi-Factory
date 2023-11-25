using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirtValue : MonoBehaviour
{
    private MeshRenderer PlayerRenderer;
    private GameSoundPlayer soundPlayer;
    private Material[] PlayerMaterials;
    private Rigidbody Player;
    public float Dirt = 0f;
    public float previousDirtValue = 0f;
    public float MaxDirt = 100f;
    public float InWaterCleaning = 0f;
    public Color color;
    private bool playSound = false;
    int cleaningCounter = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        playSound = false;
        PlayerRenderer = GetComponent<MeshRenderer>();//GameObject.Find("mochi").GetComponent<MeshRenderer>();
        Player = GetComponent<Rigidbody>();
        PlayerMaterials = PlayerRenderer.sharedMaterials;
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>().GetComponent<GameSoundPlayer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMaterials[1].color = new Color(color.r, color.g, color.b, (Dirt / MaxDirt));

        if (Dirt >= MaxDirt && playSound == false)
        {
            playSound = true;
            soundPlayer.audioSource.PlayOneShot(soundPlayer.gameOver.file, soundPlayer.gameOver.volume);
            Player.GetComponent<WinCondition>().loseCondition = true;
        }

        if (GetComponent<PlayerMovement>().inWater == true)
        {
            cleaningCounter += 1;
            if (cleaningCounter % 30 == 0)
            {
                cleaningCounter = 0;
                float dirt = GetComponent<DirtValue>().Dirt;
                if (dirt - InWaterCleaning >= 0)
                {
                    dirt -= InWaterCleaning;
                }
                else
                {
                    dirt = 0f;
                }

                GetComponent<DirtValue>().Dirt = dirt;
            }
        }
    }

    public void CameraShakeOnDirtyGround(float magnitude)
    {
        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.4f, magnitude));

    }
}
