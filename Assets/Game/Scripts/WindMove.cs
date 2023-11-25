using UnityEngine;

public class WindMove : MonoBehaviour
{
    // x軸方向に加える風の力
    public float windX = 0f;
    // y軸方向に加える風の力
    public float windY = 0f;
    // z軸方向に加える風の力
    public float windZ = 0f;
    /// <summary>
    /// トリガーの範囲に入っている間ずっと実行される
    /// </summary>
    /// <param name="other"></param>
    /// 

    private void OnTriggerStay(Collider other)
    {
        // 当たった相手のrigidbodyコンポーネントを取得
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

        // rigidbodyがnullではない場合（相手のGameObjectにrigidbodyが付いている場合）
        if (otherRigidbody != null)
        {
            // 相手のrigidbodyに力を加える
            otherRigidbody.AddForce(windX, windY, windZ, ForceMode.Force);
        }
    }
}