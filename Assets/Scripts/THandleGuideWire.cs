using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class THandleGuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public XRGrabInteractable grab;
    private Vector3 targetPosition = new Vector3(-0.116999999f, 1.51300001f, -0.128000006f);
    private Quaternion targetRotation = Quaternion.Euler(2.511993f, 267.749f, 334.316f);
    public GameObject Awl;
    public float neededThandleDepth;
    public float actualThandleDepth;
    public float tHandleAccuracy;
    void Awake()
    {
        grab = GetComponent<XRGrabInteractableTwoAttach>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("toolAlign") && !eventManager.IsTrainingMode)
        {
            Animator animator = GetComponent<Animator>();
            animator.enabled = true;
            StartCoroutine(Animate(animator));
        }
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("THandle Limit"))
        {
            // Snap to the target position and rotation
            if (!eventManager.IsTrainingMode)
            {
                // transform.position = snapPosition;
                // transform.rotation = snapRotation;
                eventManager.taskPanel.SetActive(true);
                eventManager.taskText.text = "<b><color=green>SUCCESS:</color></b> Correct guide wire depth. Ready to continue";
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
                CalculateAccuracy();
            }
            Awl.SetActive(true);
            Awl.GetComponent<Animator>().enabled = false;
            // begin awl task after T-Handle is used 
            eventManager.OnEventTHandleUsed();
        }
        if (other.CompareTag("THandle") && !eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.taskText.text = "<b><color=red>WARNING:</color></b> Guide wire inserted too deep! Pull back slightly before you continue.";
            if (eventManager.alarmAudioSource && eventManager.alarmClip)
            {
                eventManager.alarmAudioSource.clip = eventManager.alarmClip;
                eventManager.alarmAudioSource.Play();
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
            }
        }
        if (other.CompareTag("Bone") && !eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.taskText.text = "You collided with the bone";
            if (eventManager.alarmAudioSource && eventManager.alarmClip)
            {
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
            }
        }
    }
    private void CalculateAccuracy()
    {
        // 1. Actual position & rotation
        Vector3 actualPosition = transform.position;
        Quaternion actualRotation = transform.rotation;

        // 2. Position error
        float positionError = Vector3.Distance(actualPosition, targetPosition); // in meters

        // 3. Rotation error
        float rotationError = Quaternion.Angle(actualRotation, targetRotation); // in degrees

        // 4. Accuracy calculation (example: normalized to 0–1)
        float maxPositionError = 0.05f;  // tolerance (5 cm)
        float maxRotationError = 30f;    // tolerance (30 degrees)

        float positionAccuracy = Mathf.Clamp01(1f - (positionError / maxPositionError));
        float rotationAccuracy = Mathf.Clamp01(1f - (rotationError / maxRotationError));

        // 5. Average both:
        tHandleAccuracy = (positionAccuracy + rotationAccuracy) / 2f * 100f;

        Debug.Log($"THandle Position Error: {positionError:F4} m");
        Debug.Log($"THandle Rotation Error: {rotationError:F2}°");
        Debug.Log($"THandle Final Accuracy: {tHandleAccuracy:F1}%");
    }
    IEnumerator Animate(Animator animator)
    {
        if (animator != null)
        {
            animator.Play("THandle"); // Play the insertion animation
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
