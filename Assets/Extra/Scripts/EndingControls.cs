using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndingControls : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    float counter;
    bool end = false;
    private Transform lastCredit;
    int totalScore = 0;
    public GameObject bg2;
    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.started += Change;

        counter = 0f;

        lastCredit = GameObject.FindGameObjectWithTag("LastCredit").transform;
        end = false;
        if (GameObject.FindAnyObjectByType<Observer>() != null)
        {
            for (int i = 0; i < 12; i++)
            {
                totalScore += GameObject.FindAnyObjectByType<Observer>().GetComponent<Observer>().score[i];
            }
        }
        if (totalScore >= 600)
        {
            bg2.SetActive(true);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;

        Debug.Log(lastCredit.position.y);
        if(lastCredit.position.y > 70f)
        {
            end = true;
        }

        if(end == true)
        {
            end = false;
            playerInputActions.Player.Disable();
            FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("MainMenu");
        }
    }

    private void Change(InputAction.CallbackContext context)
    {
        if (context.started && counter > 2f)
        {
            playerInputActions.Player.Disable();
            FindObjectOfType<Fade>().gameObject.GetComponent<Fade>().ChangeScene("MainMenu");
        }
    }
}
