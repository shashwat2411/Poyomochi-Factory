using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gabage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�e�̔��ˏꏊ")]
    private GameObject launchPoint;

    [SerializeField]
    [Tooltip("�e")]
    private GameObject bullet;

    [SerializeField]
    [Tooltip("�e�̑���")]
    private float speed = 10.0f;

    [SerializeField]
    [Tooltip("�e�̔��ˊԊu(ms)")]
    private float waitingTime = 1.0f;

    private float bulletCount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bulletCount += Time.deltaTime;

        if(bulletCount >= waitingTime)
        {
            LauncherShot();
            bulletCount = 0.0f;
        }
    }

    private void LauncherShot()
    {
        //���˂���ꏊ���擾
        Vector3 bulletPosition = launchPoint.transform.position;
        //�擾�����ꏊ��"bullet"��prefab���o��������
        Quaternion angle = Quaternion.identity;
        angle.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.z, transform.rotation.eulerAngles.z - 90f);
        
        GameObject newBall = Instantiate(bullet, bulletPosition, angle);
        newBall.transform.localScale = this.transform.localScale / 2f;
        //�o���������{�[����forward(z������)
        Vector3 direction = -this.transform.right;
        //�{�[���̔��˕����ɗ͂�������
        newBall.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
        newBall.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 1f);
        //���O��bullet��prefab�Ɠ����ɕς���
        newBall.name = bullet.name;
        if (GetComponent<AudioSource>().isPlaying == false) { GetComponent<AudioSource>().Play(); }
        //�e����������
        //Destroy(newBall, 2.0f);
    }
}

//memo
/*==���j====================================
 * �����^�ł��p�^�ł����l�Ɏg����X�N���v�g
 *  �؂�ւ��^�ɂ��邩�ǂ��ɂ����ē����d�l�ɂ���
 *
 *==����==================================
 * �Z��̃I�u�W�F�N�g�Ŕ��˃|�C���g���w�肷��
 * �����˂̋�����ς�����悤��
 * �����˕���ς�����悤��
 * �������n�_�ɂ͉��ꂽ����z�u�i�v�Z���ďo�� or ����Ă݂Ď蓮�z�u�j
 * 
 *==���ɔ��˂���̂��I������烊�[�_�[�Ɋm�F���Ă݂�===============
 * �����˕����͑��������_���ɂ���H
 * �����˕��������ݒ肷��H
 */
