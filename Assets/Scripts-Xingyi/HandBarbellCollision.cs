using UnityEngine;

public class HandBarbellCollisionController : MonoBehaviour
{
    void Start()
    {
        // Get the Layer numbers from their names
        int leftHandLayer = LayerMask.NameToLayer("LeftHand");
        int rightHandLayer = LayerMask.NameToLayer("RightHand");
        int barbellLayer = LayerMask.NameToLayer("Grabbable");

        // Check if the layers are valid
        if (leftHandLayer == -1 || rightHandLayer == -1 || barbellLayer == -1)
        {
            Debug.LogError("Please make sure 'LeftHand', 'RightHand', and 'Barbell' layers exist!");
            return;
        }

        // Disable collisions between hands and barbell
        Physics.IgnoreLayerCollision(leftHandLayer, barbellLayer, true);
        Physics.IgnoreLayerCollision(rightHandLayer, barbellLayer, true);

        Debug.Log("Collision between hands and barbell has been disabled.");
    }
}
