using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    bool Crush = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && GetComponentInParent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Crush")
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 originalSize = other.transform.localScale;
            originalSize.y = 0.25f;
            other.transform.localScale = originalSize;
            other.GetComponent<PlayerMovement>().normalJump = false;
            other.GetComponent<PlayerMovement>().playerInputActions.Player.Disable();
            if ((this.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "CrushWait" || this.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Crush") && Crush == false)
            {
                Vector3 newPos = other.transform.position;
                //newPos.y = this.gameObject.GetComponentInParent<Transform>().position.y - this.gameObject.GetComponentInParent<Transform>().localScale.y;
                //newPos.y = this.gameObject.GetComponentInChildren<Transform>().position.y;
                newPos.y = this.gameObject.GetComponentInParent<Transform>().GetComponentInParent<Transform>().position.y - 5f;
                Debug.Log(newPos.y);
                other.transform.position = newPos;
                Crush = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Vector3 originalPosition = other.transform.position;
            ////originalPosition.y -= 3.5f;
            //other.transform.position = originalPosition;
            other.GetComponent<PlayerMovement>().jumpReturnCounter = 0;
            other.GetComponent<PlayerMovement>().countDown = true;
            Crush = false;
        }
    }
}
