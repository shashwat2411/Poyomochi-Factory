using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConveyorSamples
{
    public class Conveyor : MonoBehaviour
    {
        /// �x���g�R���x�A�̉ғ���
        public bool IsOn = false;

        /// �x���g�R���x�A�̐ݒ葬�x
        public float TargetDriveSpeed = 3.0f;

        /// ���݂̃x���g�R���x�A�̑��x
        public float CurrentSpeed { get { return _currentSpeed; } }

        /// �x���g�R���x�A�����̂𓮂�������
        public Vector3 DriveDirection = Vector3.forward;

        /// �R���x�A�����̂������́i�����́j
        [SerializeField] private float _forcePower = 50f;

        private float _currentSpeed = 0;
        private List<Rigidbody> _rigidbodies = new List<Rigidbody>();

        void Start()
        {
            //�����͐��K�����Ă���
            DriveDirection = DriveDirection.normalized;
        }

        void FixedUpdate()
        {
            _currentSpeed = IsOn ? TargetDriveSpeed : 0;

            //���ł����I�u�W�F�N�g�͏�������
            _rigidbodies.RemoveAll(r => r == null);

            foreach (var r in _rigidbodies)
            {
                //���̂̈ړ����x�̃x���g�R���x�A�����̐������������o��
                var objectSpeed = Vector3.Dot(r.velocity, DriveDirection);

                //�ڕW�l�ȉ��Ȃ��������
                if (objectSpeed < Mathf.Abs(TargetDriveSpeed))
                {
                    r.AddForce(DriveDirection * _forcePower, ForceMode.Acceleration);
                }
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            var rigidBody = collision.gameObject.GetComponent<Rigidbody>();
            _rigidbodies.Add(rigidBody);
        }

        void OnCollisionExit(Collision collision)
        {
            var rigidBody = collision.gameObject.GetComponent<Rigidbody>();
            _rigidbodies.Remove(rigidBody);
        }
    }
}

