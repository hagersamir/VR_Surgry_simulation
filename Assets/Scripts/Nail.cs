using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;

public class Nail : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public XRGrabInteractable grab;
    // Target snap position and rotation
    private Vector3 targetPosition = new Vector3(-0.00625181943f, 1.22146058f, -0.270748317f);
    private Quaternion targetRotation = Quaternion.Euler(60.5496979f,184.50705f,2.94030118f);
    public float neededNailDepth;
    public float actualNailDepth;
    public float nailPositionAccuracy;
    public XRayExtraction xrayExtraction;
    public string nailXrayImg;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractableTwoAttach>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("toolAlign") && !eventManager.IsTrainingMode)
        {
            grab.enabled = false;
            Animator animator = GetComponent<Animator>();
            animator.enabled = true;
            StartCoroutine(Animate(animator));
        }
        if (other.CompareTag("canalZone") && !eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.DelayCoroutine(4f, () => { eventManager.taskText.text = "<color=green>Inside canal. Proceed carefully.</color>"; });

            if (eventManager.alarmAudioSource && eventManager.alarmClip)
            {
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
            }
        }
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("wire"))
        {
            // Snap to the target position and rotation
            // transform.position = targetPosition; 
            // transform.rotation = targetRotation;
            if (!eventManager.IsTrainingMode)
            {
                eventManager.taskPanel.SetActive(true);
                eventManager.taskText.text = "<b><color=green>SUCCESS:</color></b> Correct nail depth. Ready to continue";
                if (eventManager.alarmAudioSource && eventManager.alarmClip)
                {
                    StartCoroutine(eventManager.StopAlarmAfterSeconds(5f));
                }
                Animator animator = GetComponent<Animator>();
                animator.enabled = false;
                grab.enabled = false;
                CalculateAccuracy();
                xrayExtraction.SaveXrayImage("Nail");
#if UNITY_EDITOR
                string directory = Application.dataPath + "/savedImages";
                Directory.CreateDirectory(directory);
                nailXrayImg = directory + "/Nail.png";
#else
                // In build, use persistent path
                string directory = Application.persistentDataPath + "/savedImages";
                Directory.CreateDirectory(directory);
                nailXrayImg = directory + "/Nail.png";
#endif
            }
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
        if (other.CompareTag("nailOverLimit") && !eventManager.IsTrainingMode)
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
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canalZone") && !eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.taskText.text = "<b><color=red>WARNING:</color></b> Nail touching bone walls! Realign to continue safely.";

            if (eventManager.alarmAudioSource && eventManager.alarmClip)
            {
                eventManager.alarmAudioSource.clip = eventManager.alarmClip;
                eventManager.alarmAudioSource.Play();
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
        nailPositionAccuracy = (positionAccuracy + rotationAccuracy) / 2f * 100f;

        Debug.Log($"Nail Position Error: {positionError:F4} m");
        Debug.Log($"Nail Rotation Error: {rotationError:F2}°");
        Debug.Log($"Nail Final Accuracy: {nailPositionAccuracy:F1}%");
    }
    IEnumerator Animate(Animator animator)
    {
        if (animator != null)
        {
            animator.Play("Nail"); // Play the insertion animation
            Debug.Log("Animation started");

            // Wait until the state actually switches to "Nail"
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Nail"));

            // Now wait for its length
            float duration = animator.GetCurrentAnimatorStateInfo(0).length;
            Debug.Log($"Animation duration: {duration:F2} seconds");
            yield return new WaitForSeconds(duration);

            grab.enabled = false;
        }
        else
        {
            Debug.LogWarning("No Animator found on tool.");
        }
    }

}
