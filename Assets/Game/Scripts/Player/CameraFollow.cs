using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Field Of View")]
    public float waitingTime = 2f;
    public float speedCurve = 0.97f;
    public float MaxFOV = 100f;
    public float MinFOV = 60.0f;
    public float SpeedFOV = 1f;
    float speed = 1f;
    float counter = 0f;
    bool FOV = false;

    [Header("Follow")]
    public float followSpeed = 2f;
    public float xOffset = 0f;
    public float yOffset = 1f;
    public float zOffset = 0f;
    private GameObject target;
    public float maxFarOff = -12f;

    private void Awake()
    {
        target = GameObject.FindAnyObjectByType<PlayerMovement>().gameObject;
        speed = SpeedFOV;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target.GetComponent<WinCondition>().winCondition == false && target.GetComponent<WinCondition>().loseCondition == false)
        {
            Vector3 newPos = new Vector3(target.transform.position.x + xOffset, target.transform.position.y + yOffset, target.transform.position.z - 10f + zOffset);
            transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);

            transform.LookAt(target.transform);

            Vector3 dir = target.GetComponent<PlayerMovement>().direction;
            if (Mathf.Abs(dir.x) <= 0.2f && Mathf.Abs(dir.y) <= 0.2f)
            {
                counter += Time.deltaTime;
                if (counter >= waitingTime && FOV == false)
                {
                    if (GameObject.FindAnyObjectByType<StringAbilityActivator>() == null)
                    {
                        FOV = true;
                        SpeedFOV = speed;
                    }
                    else
                    {
                        if(GameObject.FindAnyObjectByType<StringAbilityActivator>().GetComponent<StringAbilityActivator>().collect == false)
                        {
                            FOV = true;
                            SpeedFOV = speed;
                        }
                    }
                }
            }
            else
            {
                if (FOV == true)
                {
                    counter = 0f;
                    FOV = false;
                    SpeedFOV = speed;
                }
            }

            //if (FOV == true)
            //{
            //    if (GetComponent<Camera>().fieldOfView < MaxFOV)
            //    {
            //        GetComponent<Camera>().fieldOfView += SpeedFOV;
            //        SpeedFOV *= speedCurve;
            //    }
            //    else
            //    {
            //        GetComponent<Camera>().fieldOfView = MaxFOV;
            //    }
            //}
            //else
            //{
            //    if (GetComponent<Camera>().fieldOfView > MinFOV)
            //    {
            //        GetComponent<Camera>().fieldOfView -= SpeedFOV;
            //        SpeedFOV *= speedCurve;
            //    }
            //    else
            //    {
            //        GetComponent<Camera>().fieldOfView = MinFOV;
            //    }
            //}
            if (FOV == true)
            {
                if (zOffset > maxFarOff)
                {
                    zOffset -= SpeedFOV;
                    SpeedFOV *= speedCurve;
                }
                else
                {
                    zOffset = maxFarOff;
                }
            }
            else
            {
                if (zOffset < 0f)
                {
                    zOffset += SpeedFOV;
                    SpeedFOV *= speedCurve;
                }
                else
                {
                    zOffset = 0f;
                }
            }
        }
    }
}
