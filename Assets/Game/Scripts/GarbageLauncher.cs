using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gabage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("弾の発射場所")]
    private GameObject launchPoint;

    [SerializeField]
    [Tooltip("弾")]
    private GameObject bullet;

    [SerializeField]
    [Tooltip("弾の速さ")]
    private float speed = 10.0f;

    [SerializeField]
    [Tooltip("弾の発射間隔(ms)")]
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
        //発射する場所を取得
        Vector3 bulletPosition = launchPoint.transform.position;
        //取得した場所に"bullet"のprefabを出現させる
        Quaternion angle = Quaternion.identity;
        angle.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.z, transform.rotation.eulerAngles.z - 90f);
        
        GameObject newBall = Instantiate(bullet, bulletPosition, angle);
        newBall.transform.localScale = this.transform.localScale / 2f;
        //出現させたボールのforward(z軸方向)
        Vector3 direction = -this.transform.right;
        //ボールの発射方向に力を加える
        newBall.GetComponent<Rigidbody>().AddForce(direction * speed, ForceMode.Impulse);
        newBall.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 1f);
        //名前をbulletのprefabと同じに変える
        newBall.name = bullet.name;
        if (GetComponent<AudioSource>().isPlaying == false) { GetComponent<AudioSource>().Play(); }
        //弾を消去する
        //Destroy(newBall, 2.0f);
    }
}

//memo
/*==方針====================================
 * 直線型でも角型でも同様に使えるスクリプト
 *  切り替え型にするかどうにかして同じ仕様にする
 *
 *==作り方==================================
 * 〇空のオブジェクトで発射ポイントを指定する
 * ◎発射の強さを変えられるように
 * ◎発射物を変えられるように
 * ◎落下地点には汚れた床を配置（計算して出す or やってみて手動配置）
 * 
 *==一定に発射するのが終わったらリーダーに確認してみる===============
 * ○発射方向は多少ランダムにする？
 * ○発射物も複数設定する？
 */
