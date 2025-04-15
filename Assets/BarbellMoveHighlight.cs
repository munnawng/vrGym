using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BarbellMoveHighlight : MonoBehaviour
{
    public GameObject blurObject;
    public float blurSpeedThreshold = 0.01f;
    public float fadeSpeed = 5f;

    private Material blurMat;
    private Color baseColor;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;

        if (blurObject != null)
        {
            blurMat = blurObject.GetComponent<Renderer>().material;
            baseColor = blurMat.color;
        }
    }

    void Update()
    {
        if (blurObject == null || blurMat == null) return;

        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        float alphaTarget = speed > blurSpeedThreshold ? 0.6f : 0f;

        Color c = blurMat.color;
        c.a = Mathf.Lerp(c.a, alphaTarget, Time.deltaTime * fadeSpeed);
        blurMat.color = c;

        //Debug.Log("Rigidbody velocity: " + rb.velocity);

        // TEMPORARY DEBUG VERSION
        //float alphaTarget = 0.3f; // Always partially visible for testing

        //Color c = blurMat.color;
        //c.a = alphaTarget;
        //blurMat.color = c;
    }
}

