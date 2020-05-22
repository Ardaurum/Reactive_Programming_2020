using UnityEngine;

namespace Utilities
{
    public sealed class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Transform _follow;
        [SerializeField] private float _distanceMax, _distanceMin;
        [SerializeField, Range(.0f, 1.0f)] private float _smoothing;

        public void LateUpdate()
        {
            var vector = _follow.position - transform.position;
            var distance = vector.magnitude;
            transform.forward = vector / distance;

            var targetPosition = transform.position;
            if (distance < _distanceMin)
            {
                targetPosition = _follow.position + _distanceMin * (-transform.forward);
            }
            else if (distance > _distanceMax)
            {
                targetPosition = _follow.position + _distanceMax * (-transform.forward);
            }

            targetPosition.y = _follow.position.y + 5.0f;

            transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothing);
        }
    }
}