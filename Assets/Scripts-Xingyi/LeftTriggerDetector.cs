using UnityEngine;

public class LeftTriggerDetector : MonoBehaviour
{
    public TwoHandGrabToggle manager;

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Trigger enter detected: " + other.name);
    
    Transform t = other.transform;
    while (t != null)
    {
        Debug.Log("Checking: " + t.name + ", Tag: " + t.tag);
        if (t.CompareTag("Hand"))
        {
            Debug.Log("Hand entered LEFT trigger zone.");
            manager.SetLeftHandInside(true);
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
                manager.SetLeftHandInside(false);
                return;
            }
            t = t.parent;
        }
    }
}
