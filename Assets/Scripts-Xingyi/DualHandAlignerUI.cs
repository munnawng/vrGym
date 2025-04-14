using UnityEngine;
using TMPro;

public class DualHandAlignerUI : MonoBehaviour
{
    [Header("Tracking References")]
    public GameObject barbell;
    public Transform leftHandPoint;
    public Transform rightHandPoint;
    public OVRHand leftHand;
    public OVRHand rightHand;

    [Header("UI")]
    public TextMeshProUGUI statusText;

    [Header("Config")]
    public float requiredHoldTime = 3.0f;
    public float minHandDistance = 0.3f;
    public float maxHandDistance = 1.2f;

    private float holdTimer = 0f;
    private bool aligned = false;

    void Start()
    {
        if (barbell != null)
        {
            barbell.SetActive(false);
        }

        if (statusText != null)
        {
            statusText.text = "Please hold the barbell with both hands for 5 seconds...";
        }
    }

    void Update()
    {
         Debug.Log("Aligner is running");
        if (aligned || leftHand == null || rightHand == null ||
            leftHandPoint == null || rightHandPoint == null || statusText == null)
        {
            return;
        }

        bool handsTracked = leftHand.IsTracked && rightHand.IsTracked;
        float distance = Vector3.Distance(leftHandPoint.position, rightHandPoint.position);
        bool handsInPosition = distance > minHandDistance && distance < maxHandDistance;

        Debug.Log("Tracking Status - Left: " + leftHand.IsTracked + ", Right: " + rightHand.IsTracked);
        Debug.Log("Hand Distance: " + distance.ToString("F2") + ", Hold Timer: " + holdTimer.ToString("F2"));

        if (handsTracked && handsInPosition)
        {
            holdTimer += Time.deltaTime;

            float remaining = Mathf.Max(0f, requiredHoldTime - holdTimer);
            statusText.text = "Hold this position: " + remaining.ToString("F1") + " seconds remaining";

            if (holdTimer >= requiredHoldTime)
            {
                AlignBarbell();
            }
        }
        else
        {
            holdTimer = 0f;
            statusText.text = "Please hold the barbell with both hands...";
        }
    }

    void AlignBarbell()
    {
        Vector3 center = (leftHandPoint.position + rightHandPoint.position) / 2f;
        Vector3 direction = (rightHandPoint.position - leftHandPoint.position).normalized;

        barbell.transform.position = center;
        barbell.transform.rotation = Quaternion.LookRotation(direction);
        barbell.SetActive(true);

        aligned = true;
        statusText.text = "Barbell aligned. You are ready to begin.";
        Debug.Log("Barbell aligned at position: " + center.ToString("F3"));
    }
}
