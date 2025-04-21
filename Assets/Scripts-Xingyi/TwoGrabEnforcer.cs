using UnityEngine;
using Oculus.Interaction.HandGrab;

public class TwoHandGrabToggle : MonoBehaviour
{
    public HandGrabInteractable leftGrabPoint;
    public HandGrabInteractable rightGrabPoint;

    private bool leftHandInside = false;
    private bool rightHandInside = false;

    private void Update()
    {
        Debug.Log($"[DEBUG] leftGrab: {leftGrabPoint?.name}, rightGrab: {rightGrabPoint?.name}, leftInside: {leftHandInside}, rightInside: {rightHandInside}");

        if (leftHandInside && rightHandInside)
        {
            leftGrabPoint.enabled = true;
            rightGrabPoint.enabled = true;
        }
        else
        {
            leftGrabPoint.enabled = false;
            rightGrabPoint.enabled = false;
        }
    }


    public void SetLeftHandInside(bool inside)
    {
        leftHandInside = inside;
    }

    public void SetRightHandInside(bool inside)
    {
        rightHandInside = inside;
    }
}
