using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GuideWireConstraint : MonoBehaviour
{
    public Animator animator;
    private XRGrabInteractable grab;

    private bool hasInserted = false;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractableTwoAttach>();
        grab.selectEntered.AddListener(OnGrabbed);

        // Optional: disable animator at start if needed
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (!hasInserted && animator != null)
        {
            animator.enabled = true;

            // Optional: start from 0.75 sec if your animation is 1 sec long
            float normalizedStartTime = 0.75f; // change if needed
            animator.Play("GuideWire", 0, normalizedStartTime); 

            hasInserted = true;

            // Optional: disable grab if you want to prevent moving afterward
            // grab.enabled = false;
        }
    }
}
