using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrab : MonoBehaviour
{
    public float fixedGrabDistance = 0.2f;
    private Camera cam;
    private GameObject grabbedObject;
    private float grabDistance = 2f;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Try to grab
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.CompareTag("Grabbable"))
                {
                    grabbedObject = hit.collider.gameObject;
                    grabDistance = fixedGrabDistance;
                }
            }
        }

        // Release
        if (Input.GetMouseButtonUp(0))
        {
            grabbedObject = null;
        }

        // Move while held
        if (grabbedObject != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPos = ray.origin + ray.direction * grabDistance;
            grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, targetPos, Time.deltaTime * 10f);
        }
    }
}
