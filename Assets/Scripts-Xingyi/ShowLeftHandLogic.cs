using UnityEngine;

public class LeftHandVisualController : MonoBehaviour
{
    public OVRHand leftHand;
    public GameObject leftHandVisual;

    private bool wasTracking = false;

    void Start()
    {
        if (leftHandVisual != null)
            leftHandVisual.SetActive(false);     }

    void Update()
    {
        if (leftHand == null || leftHandVisual == null)
            return;

        if (leftHand.IsTracked)
        {
            if (!wasTracking)
            {
                leftHandVisual.SetActive(true);
                Debug.Log("Left hand tracking restored, visual enabled.");
                wasTracking = true;
            }
        }
        else
        {
            if (wasTracking)
            {
                leftHandVisual.SetActive(false);
                Debug.Log("Left hand tracking lost, visual hidden.");
                wasTracking = false;
            }
        }
    }
}
