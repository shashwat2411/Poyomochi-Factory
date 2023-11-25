using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeLimit = 180.0f; // �����l��180�b�ɐݒ�
    private TextMeshProUGUI timerText; // �������Ԃ�\������e�L�X�g�I�u�W�F�N�g
    private WinCondition win;
    private GameSoundPlayer soundPlayer;
    float countDownCounter = 0f;
    bool endHorn = false;

    [Header("Timer Settings")]
    public Color originalColor;
    public float belowTime1;
    public Color belowTimeColor1;
    public float belowTime2;
    public Color belowTimeColor2;

    private void Awake()
    {
        win = GameObject.FindAnyObjectByType<WinCondition>().GetComponent<WinCondition>();
        timerText = GameObject.Find("time").GetComponent<TextMeshProUGUI>();
        soundPlayer = GameObject.FindAnyObjectByType<GameSoundPlayer>().GetComponent<GameSoundPlayer>();
    }

    void FixedUpdate()
    {
        if (win.winCondition == false && win.loseCondition == false && GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<PlayerMovement>().paused == false)
        {
            timeLimit -= Time.deltaTime; // �������Ԃ����炷
        }

        if (timeLimit <= 0f)
        {
            timeLimit = 0f; // �������Ԃ�0�ȉ��ɂȂ�Ȃ��悤�ɂ���
            if (win.loseCondition == false)
            {
                soundPlayer.audioSource.PlayOneShot(soundPlayer.horn.file, soundPlayer.horn.volume);
                win.loseCondition = true;
            }
        }
        timerText.text = "" + ((int)timeLimit); // �e�L�X�g�I�u�W�F�N�g�ɐ������Ԃ�\������

        if((int)timeLimit <= 10 && (int)timeLimit > 0)
        {
            if(countDownCounter >= 1f)
            {
                soundPlayer.audioSource.PlayOneShot(soundPlayer.tick.file, soundPlayer.tick.volume);
                countDownCounter = 0f;
            }
            countDownCounter += Time.deltaTime;
        }
        else if((int)timeLimit <= 0 && endHorn == false)
        {
            soundPlayer.audioSource.PlayOneShot(soundPlayer.tick.file, soundPlayer.tick.volume);
            endHorn = true;
        }

        if(timeLimit < belowTime1)
        {
            if (timeLimit > belowTime2)
            {
                GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().pitch = 1.25f;
                timerText.color = belowTimeColor1;
            }
            else
            {
                GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().pitch = 1.35f;
                timerText.color = belowTimeColor2;
            }
        }
        else
        {
            GameObject.Find("BGM Audio Source").GetComponent<AudioSource>().pitch = 1f;
            timerText.color = originalColor;
        }
    }
}





