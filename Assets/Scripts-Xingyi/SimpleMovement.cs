using UnityEngine;
using Oculus.Interaction;

public class SimpleMovement : IMovement
{
    private Pose _pose;
    private bool _stopped;

    public Pose Pose => _pose;
    public bool Stopped => _stopped;

    public SimpleMovement(Transform followTarget)
    {
        _pose = new Pose(followTarget.position, followTarget.rotation);
        _stopped = false;
    }

    public void MoveTo(Pose pose)
    {
        _pose = pose;
        _stopped = false;
    }

    public void StopAndSetPose(Pose pose)
    {
        _pose = pose;
        _stopped = true;
    }

    public void UpdateTarget(Pose pose)
    {
        _pose = pose;
    }

    public void Tick()
    {
        // optional update logic
    }
}
