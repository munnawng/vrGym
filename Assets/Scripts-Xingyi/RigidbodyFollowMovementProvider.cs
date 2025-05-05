using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class RigidbodySpringFollowMovementProvider : MonoBehaviour, IMovementProvider
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [Header("Position Spring Settings")]
    public float stiffness = 10000f;
    public float damping = 800f;
    public float maxForce = 6000f;

    [Header("Rotation Spring Settings")]
    public float rotationStiffness = 1500f;
    public float rotationDamping = 300f;
    public float maxTorque = 2000f;

    private SpringFollowMovement _activeMovement;

    public IMovement CreateMovement()
    {
        _activeMovement = new SpringFollowMovement(_rigidbody, stiffness, damping, maxForce, rotationStiffness, rotationDamping, maxTorque);
        return _activeMovement;
    }

    // Lock the barbell to a hand transform
    public void LockBarbellToHand(Transform handTransform)
    {
        _activeMovement?.LockToHand(handTransform);
    }

    // Unlock the barbell from hand
    public void UnlockBarbell()
    {
        _activeMovement?.UnlockFromHand();
    }

    private class SpringFollowMovement : IMovement
    {
        private readonly Rigidbody _rigidbody;
        private readonly float _stiffness;
        private readonly float _damping;
        private readonly float _maxForce;
        private readonly float _rotStiffness;
        private readonly float _rotDamping;
        private readonly float _maxTorque;

        private Pose _pose;

        private bool _isLockedToHand = false;
        private Transform _handTransform;

        public SpringFollowMovement(
            Rigidbody rigidbody,
            float stiffness,
            float damping,
            float maxForce,
            float rotStiffness,
            float rotDamping,
            float maxTorque)
        {
            _rigidbody = rigidbody;
            _stiffness = stiffness;
            _damping = damping;
            _maxForce = maxForce;
            _rotStiffness = rotStiffness;
            _rotDamping = rotDamping;
            _maxTorque = maxTorque;
        }

        public Pose Pose => _pose;
        public bool Stopped => false;

        public void Initialize(Pose pose)
        {
            _pose = pose;
        }

        public void UpdateTarget(Pose pose)
        {
            _pose = pose;
        }

        public void MoveTo(Pose pose)
        {
            // If locked, directly follow the hand transform
            if (_isLockedToHand && _handTransform != null)
            {
                _rigidbody.MovePosition(_handTransform.position);
                _rigidbody.MoveRotation(_handTransform.rotation);
                return;
            }

            // Physics spring follow
            Vector3 pullIn = pose.rotation * Vector3.forward * 0.015f;
            _pose = new Pose(pose.position + pullIn, pose.rotation);

            Vector3 delta = _pose.position - _rigidbody.position;
            Vector3 velocity = _rigidbody.velocity;
            Vector3 force = (_stiffness * delta) - (_damping * velocity);
            _rigidbody.AddForce(Vector3.ClampMagnitude(force, _maxForce), ForceMode.Force);

            Quaternion deltaRot = _pose.rotation * Quaternion.Inverse(_rigidbody.rotation);
            deltaRot.ToAngleAxis(out float angleInDeg, out Vector3 rotationAxis);
            if (angleInDeg > 180f) angleInDeg -= 360f;
            Vector3 angularVelocity = _rigidbody.angularVelocity;
            Vector3 torque = (_rotStiffness * angleInDeg * Mathf.Deg2Rad * rotationAxis) - (_rotDamping * angularVelocity);
            _rigidbody.AddTorque(Vector3.ClampMagnitude(torque, _maxTorque), ForceMode.Force);
        }

        public void StopAndSetPose(Pose pose)
        {
            _pose = pose;
        }

        public void Tick() { }

        public void LockToHand(Transform handTransform)
        {
            _handTransform = handTransform;
            _isLockedToHand = true;

            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void UnlockFromHand()
        {
            _isLockedToHand = false;
            _rigidbody.isKinematic = false;
        }
    }
}
