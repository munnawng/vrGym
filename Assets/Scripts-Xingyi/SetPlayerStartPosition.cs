using UnityEngine;
using System.Collections;

public class AlignPlayerToGymScene : MonoBehaviour
{
    public Transform trackingSpace;        // XR Rig's TrackingSpace
    public Transform headset;              // CenterEyeAnchor
    public Transform barbellAnchor;        // Anchor in front of the barbell

    public float distanceFromBarbell = 1.0f;   // Distance between chest and barbell
    public float waistOffset = 0.5f;           // Distance from headset to chest
    public float targetFloorY = 0.0f;          // Ground height

    void Start()
    {
        StartCoroutine(AlignRig());
    }

    IEnumerator AlignRig()
    {
        yield return null; // Wait for one frame for XR to initialize

        if (trackingSpace == null || headset == null || barbellAnchor == null)
        {
            Debug.LogError("AlignPlayerToGymScene: Missing references.");
            yield break;
        }

        // Headset offset relative to rig
        Vector3 headsetOffset = headset.position - trackingSpace.position;

        // Compute the direction the headset is facing (flattened on XZ)
        Vector3 headsetForward = headset.forward;
        headsetForward.y = 0;
        headsetForward.Normalize();

        // Adjust chest position: slightly behind the headset
        Vector3 chestWorldPos = headset.position - headsetForward * waistOffset;

        // Determine where the chest should be in world space (in front of barbell)
        Vector3 barbellBack = -barbellAnchor.forward;
        Vector3 targetChestPosition = barbellAnchor.position + barbellBack * distanceFromBarbell;

        // Move rig so the chest aligns with the desired chest position
        Vector3 rigOffset = chestWorldPos - trackingSpace.position;
        Vector3 newRigPosition = targetChestPosition - rigOffset;

        // Adjust height (optional)
        float headsetHeight = headset.position.y - trackingSpace.position.y;
        newRigPosition.y = targetFloorY - headsetHeight;

        // Apply new rig position
        trackingSpace.position = newRigPosition;

        // Face the barbell
        Vector3 lookDirection = barbellAnchor.position - headset.position;
        lookDirection.y = 0;
        trackingSpace.rotation = Quaternion.LookRotation(lookDirection);

        Debug.Log("AlignPlayerToGymScene: Chest aligned correctly in front of barbell.");
    }
}
