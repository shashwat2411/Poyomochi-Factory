using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundPlayer : MonoBehaviour
{
    [System.Serializable]
    public struct SoundFile
    {
        public AudioClip file;
        public float volume;
    }


    public AudioSource audioSource;
    [Header("Jump")]
    public SoundFile DoubleJump;
    public SoundFile HighJump;

    [Header("String")]
    public SoundFile StringShoot;
    public SoundFile StringHook;
    public SoundFile StringReturn;
    public SoundFile StringSwing;

    [Header("Result")]
    public SoundFile buttonPress;
    public SoundFile optionSwitch;

    [Header("Collision")]
    public SoundFile OnCollisionWithDirt;
    public SoundFile OnCollisionWithStickyWall;
    public SoundFile OnCollisionWithBreakableWall;

    [Header("Stage")]
    public SoundFile stageStart;
    public SoundFile stageClear;
    public SoundFile gameOver;

    [Header("Collectable")]
    public SoundFile coin;
    public SoundFile chopsticks;

    [Header("Water")]
    public SoundFile waterEnter;
    public SoundFile bubbles;
    public SoundFile waterExit;

    [Header("Time")]
    public SoundFile tick;
    public SoundFile horn;

    [Header("Pause")]
    public SoundFile pauseIn;
    public SoundFile pauseOut;

    [Header("Garbage")]
    public SoundFile shoot;
    public SoundFile land;

    
    // Start is called before the first frame update
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
