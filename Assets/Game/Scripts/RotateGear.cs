using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour
{
    private Quaternion prevRot;     //前フレームのrotation値
    private float angle;             //回転角度
    [System.NonSerialized]
    public float AngularVelocity;  //角速度

    public bool IsRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        prevRot = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //親の現在フレームのRotation値を取得
        var ParentRot = this.transform.rotation;
        //前フレームからの回転量を求める
        var diffRot = Quaternion.Inverse(prevRot) * ParentRot;
        //回転した角度と回転軸（ローカル）を求める
        diffRot.ToAngleAxis(out var angle, out var axis);
        //角速度を計算
        AngularVelocity = angle / Time.deltaTime;

        if(AngularVelocity <= 0.1f)
        {
            IsRotate = false;
        }
        else
        {
            IsRotate = true;
        }

        prevRot = ParentRot;    //前フレームの値を現フレームの値に上書き
    }
}
