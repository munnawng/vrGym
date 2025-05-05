using UnityEngine;

public class DualHandZoneLocker : MonoBehaviour
{
    [Header("Lock Target")]
    public RigidbodySpringFollowMovementProvider followProvider;

    [Header("Real-World Barbell Ends (Proxy for Hands)")]
    public Transform leftGrabPoint;   // Assign LeftTriggerDetector.transform
    public Transform rightGrabPoint;  // Assign RightTriggerDetector.transform

    [Header("Trigger Colliders")]
    public Collider leftTrigger;
    public Collider rightTrigger;

    [Header("Visual Alignment Offsets (Tune in Inspector)")]
    public float offsetZ = 0.02f; // Forward/back
    public float offsetY = 0.01f; // Up/down

    private GameObject midpointObject;
    private bool isLocked = false;
    private bool leftInside = false;
    private bool rightInside = false;

    void Update()
    {
        if (!isLocked && leftInside && rightInside)
        {
            midpointObject = new GameObject("BarbellMidpoint");
            followProvider.LockBarbellToHand(midpointObject.transform);
            isLocked = true;
            Debug.Log("Barbell locked to midpoint.");
        }

        if (isLocked && midpointObject != null)
        {
            UpdateMidpointTransform();
        }
    }

    private void UpdateMidpointTransform()
    {
        Vector3 midPos = (leftGrabPoint.position + rightGrabPoint.position) / 2f;
        Quaternion midRot = Quaternion.LookRotation(
            rightGrabPoint.position - leftGrabPoint.position,
            Vector3.up
        );

        Vector3 offset = midRot * Vector3.forward * offsetZ + midRot * Vector3.up * offsetY;

        midpointObject.transform.position = midPos + offset;
        midpointObject.transform.rotation = midRot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if (leftTrigger.bounds.Contains(other.transform.position))
                leftInside = true;

            if (rightTrigger.bounds.Contains(other.transform.position))
                rightInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if (!leftTrigger.bounds.Contains(other.transform.position))
                leftInside = false;

            if (!rightTrigger.bounds.Contains(other.transform.position))
                rightInside = false;
        }
    }
}
