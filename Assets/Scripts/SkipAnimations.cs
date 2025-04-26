using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipAnimations : MonoBehaviour
{
    public GameObject nurse; // Drag the Nurse GameObject here in Inspector
    public GameObject checklist; // Drag the checklist object
    public GameObject textObject; // Drag the text (TextMeshPro) object
    public Transform checklistRestPosition; // Empty GameObject where checklist should return to
    public Vector3 finalPosition; // Final nurse position
    public Vector3 finalRotationEuler; // Final rotation in Euler angles

    private Animator animator;

    void Start()
    {
        if (nurse != null)
        {
            animator = nurse.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("Nurse GameObject is not assigned!");
        }
    }

    public void SkipPreOperations()
    {
        if (animator != null)
        {
            // Stop all animations
            animator.enabled = false;
        }

        if (nurse != null)
        {
            // Move nurse to final position
            nurse.transform.position = finalPosition;
            nurse.transform.rotation = Quaternion.Euler(finalRotationEuler);
        }

        if (checklist != null && checklistRestPosition != null)
        {
            checklist.transform.SetParent(null); // Ensure it's not attached to the hand
            checklist.transform.position = checklistRestPosition.position;
            checklist.transform.rotation = checklistRestPosition.rotation;
        }

        if (textObject != null)
        {
            textObject.SetActive(false); // Hide the floating text
        }
    }
}
