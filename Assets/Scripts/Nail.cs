using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Nail : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public XRGrabInteractable grab;

    // Target snap position and rotation
    private Vector3 snapPosition = new Vector3(-0.014f, 1.208f, -0.271f);
    // private Quaternion snapRotation = Quaternion.Euler(58.928f, 182.693f, 2.088f);
    private Quaternion snapRotation = Quaternion.Euler(57.6535378f,184.882233f,4.98095989f);
    public float neededNailDepth;
    public float actualNailDepth;
    public float nailPositionAccuracy;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractableTwoAttach>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("wire"))
        {
            // Snap to the target position and rotation
            transform.position = snapPosition;
            transform.rotation = snapRotation;
            //begin Guide wire removal task after nail is inserted 
            eventManager.OnEventNailUsed();
        }
        if (other.CompareTag("nail") && eventManager.IsTrainingMode)
        {
            Animator animator = GetComponent<Animator>();
            animator.enabled = true;
            StartCoroutine(Animate(animator));
            other.gameObject.SetActive(false);
            grab.enabled = false;
        }
        if (other.CompareTag("nailOverLimit") && eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.taskText.text = "<b><color=red>WARNING:</color></b> Nail inserted too deep! Pull back slightly.";
            if (eventManager.alarmAudioSource && eventManager.alarmClip)
            {
                eventManager.alarmAudioSource.clip = eventManager.alarmClip;
                eventManager.alarmAudioSource.Play();
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
            }
        }
    }
    IEnumerator Animate(Animator animator)
    {
        if (animator != null)
        {
            animator.Play("Nail"); // Play the insertion animation
            Debug.Log("Animation started");

            // Wait for animation to finish before proceeding
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.LogWarning("No Animator found on tool.");
        }
    }
}
