using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LegTraction : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody tibiaRb;
    
    void Start()
    {
        tibiaRb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(ApplyTraction);
    }

    private void ApplyTraction(SelectEnterEventArgs args)
    {
        // Apply force to simulate traction
        tibiaRb.AddForce(Vector3.up * 50f + Vector3.forward * 20f, ForceMode.Force);
    }
}
