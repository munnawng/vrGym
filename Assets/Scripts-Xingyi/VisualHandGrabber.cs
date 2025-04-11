using UnityEngine;

public class VisualHandGrabber : MonoBehaviour
{
    public Transform realHand;  // Reference to the real (tracked) hand, e.g., LeftHandAnchor
    private GameObject heldObject = null;

    void Update()
    {
        // If currently holding an object, update its position and rotation to match the visual hand
        if (heldObject != null)
        {
            heldObject.transform.position = transform.position;
            heldObject.transform.rotation = transform.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // When a grabbable object enters the trigger zone and the real hand is grabbing
        if (other.CompareTag("Grabbable") && IsRealHandGrabbing())
        {
            heldObject = other.gameObject;
            var rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true; // Disable physics to avoid jittering
        }
    }

    void OnTriggerExit(Collider other)
    {
        // When the object leaves the trigger zone, release it
        if (other.gameObject == heldObject)
        {
            var rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;

            heldObject = null;
        }
    }

    // Temporary logic for testing: always returns true (pretending the hand is always grabbing)
    // Replace with actual input detection, e.g., OVRHand.IsPinching or HandGrabInteractor.HasInteractable
    private bool IsRealHandGrabbing()
    {
        return true; // Always grabbing — replace this for real use
    }
}
