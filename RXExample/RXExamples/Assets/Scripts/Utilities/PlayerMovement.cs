using UnityEngine;

namespace Utilities
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private Transform _forwardArrow;
        private Vector3 _forward;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _forward = Vector3.forward;
            _rigidbody = GetComponentInChildren<Rigidbody>();
        }
        
        private void Update()
        {
            
            if (Input.GetKey(KeyCode.W))
            {
                _rigidbody.AddForce(_forward * _speed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                _rigidbody.AddForce(-_forward * _speed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                _forward = Quaternion.Euler(.0f, _rotateSpeed, .0f) * _forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                _forward = Quaternion.Euler(.0f, -_rotateSpeed, .0f) * _forward;
            }

            _forwardArrow.forward = _forward;
            _forwardArrow.position = transform.position + _forward * 1.5f;
        }
    }
}