using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour
{
    private Quaternion prevRot;     //�O�t���[����rotation�l
    private float angle;             //��]�p�x
    [System.NonSerialized]
    public float AngularVelocity;  //�p���x

    public bool IsRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        prevRot = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //�e�̌��݃t���[����Rotation�l���擾
        var ParentRot = this.transform.rotation;
        //�O�t���[������̉�]�ʂ����߂�
        var diffRot = Quaternion.Inverse(prevRot) * ParentRot;
        //��]�����p�x�Ɖ�]���i���[�J���j�����߂�
        diffRot.ToAngleAxis(out var angle, out var axis);
        //�p���x���v�Z
        AngularVelocity = angle / Time.deltaTime;

        if(AngularVelocity <= 0.1f)
        {
            IsRotate = false;
        }
        else
        {
            IsRotate = true;
        }

        prevRot = ParentRot;    //�O�t���[���̒l�����t���[���̒l�ɏ㏑��
    }
}
