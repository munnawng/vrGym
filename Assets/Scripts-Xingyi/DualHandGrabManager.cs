using UnityEngine;

public class DualHandGrabManager : MonoBehaviour
{
    public Transform leftGrabPoint;
    public Transform rightGrabPoint;
    public Rigidbody barbellRigidbody;

    private bool leftGrabbed = false;
    private bool rightGrabbed = false;

    void Update()
    {
        if (leftGrabbed && rightGrabbed)
        {
            Vector3 center = (leftGrabPoint.position + rightGrabPoint.position) / 2f;
            barbellRigidbody.MovePosition(center);

            Vector3 forward = (rightGrabPoint.position - leftGrabPoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
            barbellRigidbody.MoveRotation(rotation);
        }
    }

    public void SetLeftGrabbed(bool grabbed)
    {
        leftGrabbed = grabbed;
    }

    public void SetRightGrabbed(bool grabbed)
    {
        rightGrabbed = grabbed;
    }
}
