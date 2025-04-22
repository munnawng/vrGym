using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

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
    public float readyMessageDuration = 8f;
    public float countdownDuration = 5f;
    public float alignedMessageDuration = 18f; // 3s + 15s
    public float minGripDistance = 0.3f;
    public float maxGripDistance = 1.0f;
    public float resetCooldown = 1f;

    private float timer = 0f;
    private float resetCooldownTimer = 0f;

    private float palmTextTimer = 0f;
    private float idlePalmTextDuration = 15f;
    private float palmOkTextDuration = 1f;

    private enum State
    {
        Idle,
        PalmOk,
        GripOk,
        Countdown,
        Aligned
    }

    private State currentState = State.Idle;

    void Start()
    {
        if (barbell != null) barbell.SetActive(false);
        statusText.text = "Please keep both palms facing up and shoulder-width apart in front of your belly. Hold this position.";
        palmTextTimer = idlePalmTextDuration;
    }

    void Update()
    {
        if (resetCooldownTimer > 0f)
            resetCooldownTimer -= Time.deltaTime;

        if (!IsValid()) return;

        bool palmsUp = IsPalmUp(leftHand) && IsPalmUp(rightHand);
        float gripDistance = Vector3.Distance(leftHandPoint.position, rightHandPoint.position);
        bool gripOk = gripDistance >= minGripDistance && gripDistance <= maxGripDistance;

        switch (currentState)
        {
            case State.Idle:
                if (palmsUp)
                {
                    TransitionTo(State.PalmOk, "Palm orientation is correct.");
                }
                else
                {
                    if (palmTextTimer <= 0f)
                        Show("");
                    else
                        palmTextTimer -= Time.deltaTime;
                }
                break;

            case State.PalmOk:
                if (!palmsUp)
                {
                    SafeReset("Please keep both palms facing up and shoulder-width apart in front of your belly. Hold this position.");
                    palmTextTimer = idlePalmTextDuration;
                }
                else if (palmTextTimer > 0f)
                {
                    Show("Palm orientation is correct, please wait...");
                    palmTextTimer -= Time.deltaTime;
                }
                else
                {
                    if (!gripOk)
                    {
                        Show(gripDistance < minGripDistance ? "Please widen your grip." : "Please narrow your grip.");
                    }
                    else
                    {
                        TransitionTo(State.GripOk, "Good grip width. Hold this position. Countdown starts now...");
                    }
                }
                break;

            case State.GripOk:
                if (palmTextTimer > 0f)
                {
                    Show("Good grip width. Hold this position. Countdown will start...");
                    palmTextTimer -= Time.deltaTime;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (!palmsUp || !gripOk)
                    {
                        SafeReset(!palmsUp ? "Please keep both palms facing up and shoulder-width apart in front of your belly. Hold this position."
                            : gripDistance < minGripDistance ? "Please widen your grip." : "Please narrow your grip.");
                        palmTextTimer = idlePalmTextDuration;
                    }
                    else if (timer >= readyMessageDuration)
                    {
                        TransitionTo(State.Countdown, "");
                    }
                }
                break;

            case State.Countdown:
                timer += Time.deltaTime;
                if (!palmsUp || !gripOk)
                {
                    SafeReset(!palmsUp ? "Please keep both palms facing up and shoulder-width apart in front of your belly. Hold this position."
                        : gripDistance < minGripDistance ? "Please widen your grip." : "Please narrow your grip.");
                    palmTextTimer = idlePalmTextDuration;
                }
                else
                {
                    float remaining = countdownDuration - timer;
                    Show("Hold this position: " + remaining.ToString("F1") + " seconds remaining");

                    if (remaining <= 0f)
                    {
                        AlignBarbell();
                        TransitionTo(State.Aligned, "");
                    }
                }
                break;

            case State.Aligned:
                timer += Time.deltaTime;
                // FollowHands();

                if (timer < 3f)
                {
                    Show("Barbell is now visible. Initial alignment completed.");
                }
                else if (timer < 18f)
                {
                    Show("Gently move your fingers to check if the barbell fits your palms.");
                }
                else
                {
                    statusText.text = "";
                }
                break;
        }
    }

    void SafeReset(string message)
    {
        if (resetCooldownTimer <= 0f)
        {
            ResetAll(message);
            resetCooldownTimer = resetCooldown;
        }
    }

    void TransitionTo(State next, string message)
    {
        currentState = next;
        timer = 0f;

        if (!string.IsNullOrEmpty(message))
        {
            Show(message);

            if (next == State.PalmOk)
            {
                palmTextTimer = palmOkTextDuration;
            }
            else if (next == State.GripOk)
            {
                palmTextTimer = readyMessageDuration;
            }
        }
    }

    void Show(string msg)
    {
        if (statusText != null)
        {
            statusText.text = msg;
        }
    }

    void ResetAll(string message)
    {
        currentState = State.Idle;
        timer = 0f;
        if (barbell != null) barbell.SetActive(false);
        Show(message);
        palmTextTimer = idlePalmTextDuration;
    }

    void AlignBarbell()
    {
        Vector3 leftPalm = leftHandPoint.position;
        Vector3 rightPalm = rightHandPoint.position;

        Vector3 leftPalmNormal = leftHandPoint.up;
        Vector3 rightPalmNormal = rightHandPoint.up;

        Vector3 center = (leftPalm + rightPalm) / 2f;
        Vector3 forward = (rightPalm - leftPalm).normalized;
        Vector3 palmNormal = ((leftPalmNormal + rightPalmNormal) / 2f).normalized;

        float offsetDistance = 0.025f;
        Vector3 finalPosition = center + palmNormal * offsetDistance;

        barbell.transform.position = finalPosition;
        barbell.transform.rotation = Quaternion.LookRotation(forward, palmNormal);
        barbell.SetActive(true);
    }

    void FollowHands()
    {
        if (!IsValid()) return;

        Vector3 center = (leftHandPoint.position + rightHandPoint.position) / 2f;
        Vector3 direction = (rightHandPoint.position - leftHandPoint.position).normalized;

        Vector3 offset = Vector3.Cross(direction, Vector3.up) * 0.05f;
        Vector3 targetPos = center + offset;

        barbell.transform.position = Vector3.Lerp(barbell.transform.position, targetPos, Time.deltaTime * 20f);
        Quaternion targetRot = Quaternion.LookRotation(direction);
        barbell.transform.rotation = Quaternion.Slerp(barbell.transform.rotation, targetRot, Time.deltaTime * 20f);
    }

    bool IsValid()
    {
        return leftHand != null && rightHand != null &&
               leftHandPoint != null && rightHandPoint != null &&
               leftHand.IsTracked && rightHand.IsTracked;
    }

    bool IsPalmUp(OVRHand hand)
    {
        var skeleton = hand.GetComponent<OVRSkeleton>();
        if (skeleton == null || !skeleton.IsDataValid || !skeleton.IsDataHighConfidence) return false;

        var bones = skeleton.Bones;
        var wrist = bones.FirstOrDefault(b => b.Id == OVRSkeleton.BoneId.Hand_WristRoot);
        var palm = bones.FirstOrDefault(b => b.Id == OVRSkeleton.BoneId.Hand_Middle3);

        if (wrist == null || palm == null) return false;

        Vector3 palmNormal = (palm.Transform.position - wrist.Transform.position).normalized;
        return Vector3.Dot(palmNormal, Vector3.up) > 0.3f;
    }
}