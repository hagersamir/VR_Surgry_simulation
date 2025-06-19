using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ToolEntrySnapAndRestrict : MonoBehaviour
{
    public bool isMatching = false;
    public bool isFree = true;
    public GameObject tool;
    public Vector3 fixedRotationEuler;
    public Vector3 fixedPosition;
    public string alignTag;
    public string freeTag;

    private Rigidbody rb;

    private void Start()
    {
        if (tool == null)
        {
            Debug.LogError("Tool is not assigned!");
            return;
        }

        rb = tool.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Tool must have a Rigidbody component!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} collided with {other.name}");

        if (other.CompareTag(alignTag) && isFree)
        {
            isMatching = true;
            isFree = false;
            tool.transform.position = fixedPosition;
            tool.transform.rotation = Quaternion.Euler(fixedRotationEuler);

            // if (matchCoroutine != null)
            //     StopCoroutine(matchCoroutine);

            StartCoroutine(AlignAndLockMotion(other.transform));
        }

        if (other.CompareTag(freeTag))
        {
            isFree = true;
            isMatching = false;
            StopCoroutine(AlignAndLockMotion(other.transform));

            Debug.Log("Tool is now free.");
        }
    }

    IEnumerator AlignAndLockMotion(Transform target)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        while (isMatching)
        {
        // Snap tool to target YZ position (but keep current x and z)
        Vector3 currentPos = tool.transform.position;
        Vector3 targetPos = fixedPosition;
        Vector3 currentRotation = tool.transform.eulerAngles;
        tool.transform.position = new Vector3(targetPos.x, currentPos.y, targetPos.z);

        // Apply fixed rotation
        tool.transform.eulerAngles = new Vector3 (currentRotation.x, currentRotation.y, fixedRotationEuler.z);

        yield return null; // Wait one frame in case of physics lag
        }
    }
}
