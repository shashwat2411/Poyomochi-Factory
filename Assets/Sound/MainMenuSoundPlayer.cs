using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundPlayer : MonoBehaviour
{
    [System.Serializable]
    public struct SoundFile
    {
        public AudioClip file;
        public float volume;
    }


    public AudioSource audioSource;
    public SoundFile buttonPress;
    public SoundFile optionSwitch;

    // Start is called before the first frame update
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
