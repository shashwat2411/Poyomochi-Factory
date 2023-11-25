using UnityEngine;

public class WindMove : MonoBehaviour
{
    // x�������ɉ����镗�̗�
    public float windX = 0f;
    // y�������ɉ����镗�̗�
    public float windY = 0f;
    // z�������ɉ����镗�̗�
    public float windZ = 0f;
    /// <summary>
    /// �g���K�[�͈̔͂ɓ����Ă���Ԃ����Ǝ��s�����
    /// </summary>
    /// <param name="other"></param>
    /// 

    private void OnTriggerStay(Collider other)
    {
        // �������������rigidbody�R���|�[�l���g���擾
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

        // rigidbody��null�ł͂Ȃ��ꍇ�i�����GameObject��rigidbody���t���Ă���ꍇ�j
        if (otherRigidbody != null)
        {
            // �����rigidbody�ɗ͂�������
            otherRigidbody.AddForce(windX, windY, windZ, ForceMode.Force);
        }
    }
}