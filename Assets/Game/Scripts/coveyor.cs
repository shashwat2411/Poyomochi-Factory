using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conveyor
{
    public class Conveyor : MonoBehaviour
    {
        private bool IsOn = false;
        private float TargetDriveSpeed = 10.0f;

        private Rigidbody player;

        void Start()
        {
            player = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<Rigidbody>();
            IsOn = false;
        }

        void FixedUpdate()
        {

            //if (objectSpeed < Mathf.Abs(TargetDriveSpeed))
            if (IsOn == true && player.velocity.x < 5.0f)
            {
                player.AddForce(Vector3.right * TargetDriveSpeed, ForceMode.Acceleration);
                Debug.Log(player.velocity);
            }

        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                IsOn = true;
                collision.gameObject.GetComponent<PlayerMovement>().playerInputActions.Disable();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                IsOn = false;
                collision.gameObject.GetComponent<PlayerMovement>().playerInputActions.Enable();
            }
        }
    }
}