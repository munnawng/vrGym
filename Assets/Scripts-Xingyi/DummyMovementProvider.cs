using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class DummyMovementProvider : MonoBehaviour, IMovementProvider
{
    public IMovement CreateMovement()
    {
        Debug.Log("[DUMMY PROVIDER] CreateMovement called");
        return new SimpleFollowMovement();
    }

    private class SimpleFollowMovement : IMovement
    {
        private Pose _pose;

        public Pose Pose
        {
            get
            {
                Debug.Log("[DUMMY PROVIDER] Pose getter called — tracking movement.");
                return _pose;
            }
        }

        public bool Stopped => false;

        public void Initialize(Pose pose)
        {
            _pose = pose;
            Debug.Log("[DUMMY PROVIDER] Initialized with pose.");
        }

        public void UpdateTarget(Pose pose)
        {
            _pose = pose;
            Debug.Log("[DUMMY PROVIDER] Target updated.");
        }

        public void MoveTo(Pose pose)
        {
            _pose = pose;
            Debug.Log("[DUMMY PROVIDER] MoveTo called.");
        }

        public void StopAndSetPose(Pose pose)
        {
            _pose = pose;
            Debug.Log("[DUMMY PROVIDER] StopAndSetPose called.");
        }

        public void Tick()
        {
            // Optional: Add debug here if needed
        }
    }
}
