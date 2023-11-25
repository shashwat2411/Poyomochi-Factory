using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickMove : MonoBehaviour
{
    public GameObject GimmickParent = null;    //ギミックの親。こいつの運動を利用する
    public ParticleSystem steam;

    public float change = 0f;
    public float rotateAmount = 5f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //親が回ってたら
        if (GimmickParent.GetComponent<RotateGear>().IsRotate == true)
        {
            if (GimmickParent.GetComponent<AudioSource>().isPlaying == false)
            {
                GimmickParent.GetComponent<AudioSource>().Play();
            }
            //扉の高さが指定した限界より低かったら
            if (change <= rotateAmount+0.1f)
            {

                float difference = rotateAmount - change;
                change += ((GimmickParent.GetComponent<RotateGear>().AngularVelocity * 0.001f)) / 5f;

                this.transform.localScale = new Vector3(difference / rotateAmount, this.transform.localScale.y, this.transform.localScale.z);
                steam.startColor = new Color(steam.startColor.r, steam.startColor.g, steam.startColor.b, (difference / rotateAmount));
                GetComponent<AudioSource>().volume = difference;

                if(this.transform.localScale.x < 0.05f)
                {
                    GetComponent<BoxCollider>().enabled = false;
                }
            }


        }
    }
}
