using UnityEngine;

public class RightTriggerDetector : MonoBehaviour
{
    public TwoHandGrabToggle manager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[RIGHT TRIGGER] Detected: {other.name}, Tag: {other.tag}, Layer: {LayerMask.LayerToName(other.gameObject.layer)}");

        Transform t = other.transform;
        while (t != null)
        {
            Debug.Log("Right checking: " + t.name + ", Tag: " + t.tag);
            if (t.CompareTag("Hand"))
            {
                Debug.Log("Hand entered RIGHT trigger zone.");
                manager.SetRightHandInside(true);
                return;
            }
            t = t.parent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        while (t != null)
        {
            if (t.CompareTag("Hand"))
            {
                Debug.Log("Hand exited RIGHT trigger zone.");
                manager.SetRightHandInside(false);
                return;
            }
            t = t.parent;
        }
    }
}
