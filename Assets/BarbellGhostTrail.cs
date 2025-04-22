using System.Collections;
using UnityEngine;

public class BarbellGhostTrail : MonoBehaviour
{
    public GameObject ghostPrefab;         // Assign faded barbell prefab
    public float spawnInterval = 0.001f;    // Time between ghost spawns
    public float movementThreshold = 0.001f; // How much movement is "fast"
    public float ghostLifetime = 0.1f;     // How long each ghost lasts

    private Vector3 lastPosition;
    private float spawnTimer = 0f;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // Check if the object moved
        float distanceMoved = (transform.position - lastPosition).magnitude;

        // If the movement exceeds the threshold, spawn a ghost
        if (distanceMoved > movementThreshold)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnGhost();
                spawnTimer = 0f;
            }
        }

        // Update the last position after each frame
        lastPosition = transform.position;
    }

    void SpawnGhost()
    {
        // Instantiate the ghost at the object's current position and rotation
        GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghost.transform.localScale = transform.localScale;

        // Start fading out the ghost over time
        StartCoroutine(FadeAndDestroy(ghost));
    }

    IEnumerator FadeAndDestroy(GameObject ghost)
    {
        Renderer rend = ghost.GetComponent<Renderer>();
        Material mat = rend.material;
        Color color = mat.color;

        // Start fading from the current alpha (should be full opacity initially)
        float alphaStart = color.a;
        float alphaEnd = 0f; // Fade to completely transparent

        float elapsedTime = 0f;

        // Fade out the ghost over its lifetime
        while (elapsedTime < ghostLifetime)
        {
            float lerpFactor = elapsedTime / ghostLifetime;
            color.a = Mathf.Lerp(alphaStart, alphaEnd, lerpFactor);
            mat.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully transparent at the end
        color.a = alphaEnd;
        mat.color = color;

        // Destroy the ghost object after its lifetime ends
        Destroy(ghost);
    }
}
