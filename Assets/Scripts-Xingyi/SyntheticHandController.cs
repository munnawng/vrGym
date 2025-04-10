using UnityEngine;

public class SyntheticHandController : MonoBehaviour
{
    public Transform realHand;
    public bool isResisting = true;
    [Range(0f, 0.5f)] public float resistanceOffset = 0.15f;
    public float followSpeed = 5f;

    public bool followRotation = true;
    public Vector3 rotationOffsetEuler = Vector3.zero; // Optional fix for flipped hands

    private Vector3 previousRealHandPos;
    private bool trackingInitialized = false;

    void Update()
    {
        if (realHand == null || realHand.position.magnitude < 0.01f)
        {
            trackingInitialized = false;
            return;
        }

        if (!trackingInitialized)
        {
            transform.position = realHand.position;
            previousRealHandPos = realHand.position;
            trackingInitialized = true;
            return;
        }

        // === POSITION UPDATE ===
        Vector3 targetPos = realHand.position;
        if (isResisting)
        {
            Vector3 handDelta = realHand.position - previousRealHandPos;
            if (handDelta.magnitude > 0.001f)
            {
                Vector3 resistanceDir = handDelta.normalized;
                targetPos -= resistanceDir * resistanceOffset;
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        previousRealHandPos = realHand.position;

        // === ROTATION UPDATE ===
        if (followRotation)
        {
            Quaternion targetRotation = realHand.rotation * Quaternion.Euler(rotationOffsetEuler);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * followSpeed);
        }

        Debug.DrawLine(transform.position, realHand.position, Color.red);
    }
}
