using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class SimpleFollowProvider : MonoBehaviour, IMovementProvider
{
    private class SimpleFollowMovement : IMovement
    {
        private Transform _followTarget;
        public Pose Pose { get; private set; }
        public bool Stopped { get; private set; }

        public SimpleFollowMovement(Transform followTarget)
        {
            _followTarget = followTarget;
            Pose = new Pose(_followTarget.position, _followTarget.rotation);
        }

        public void MoveTo(Pose pose)
        {
            Pose = pose;
            Stopped = false;
        }

        public void StopAndSetPose(Pose pose)
        {
            Pose = pose;
            Stopped = true;
        }

        public void UpdateTarget(Pose pose)
        {
            Pose = pose;
        }

        public void Tick()
        {
            if (!Stopped)
            {
                Pose = new Pose(_followTarget.position, _followTarget.rotation);
            }
        }
    }

    public IMovement CreateMovement()
    {
        return new SimpleFollowMovement(this.transform);
    }
}
