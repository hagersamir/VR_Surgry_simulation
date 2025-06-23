using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class GuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public TextMeshProUGUI cornerText;
    public float fadeDuration = 1.5f; // Duration of the fade effect (seconds)
    private Vector3 targetPosition = new Vector3(-0.118000001f, 1.03499997f, -0.409000009f);
    private Quaternion targetRotation = Quaternion.Euler(0.0994562954f, 4.67367411f, 2.12592959f);
    private Material wireMaterial;
    private bool isFading = false;
    private bool isDone = false;
    public GameObject aimingGuide;
    public string guideWireXrayImg;
    public float neededWireDepth;
    public float actualWireDepth;
    public float wirePositionAccuracy;
    public float nailInsertionDuration;
    private XRayExtraction xrayExtraction;

    private void Start()
    {
        wireMaterial = GetComponent<Renderer>().material;
    }
    private void OnTriggerEnter(Collider other)
    {
        // player has finished inserting the Guide wire 
        if (other.CompareTag("wire") && !isDone)
        {
            isDone = true;
            // Snap to the target position and rotation
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            GetComponent<Animator>().enabled = false;
            //begin nail task after Guide wire is inserted 
            eventManager.OnEventGuideWireUsed();
        }

        // player has finished REMOVING the Guide wire 
        if (other.CompareTag("GuideWire") && !isFading)
        {
            isFading = true;
            // Begin DRILLING TASKs after Guide wire is removed 
            eventManager.onEventGuideWire();
            aimingGuide.GetComponent<Nail>().grab.enabled = false;
            // Start the fading animation
            StartCoroutine(FadeOut());
            if (!eventManager.IsTrainingMode)
            {
                eventManager.taskPanel.SetActive(true);
                eventManager.taskText.text = "<b><color=green>SUCCESS:</color></b> Correct guide wire depth. Ready to continue";
                if (eventManager.alarmAudioSource && eventManager.alarmClip)
                {
                    StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
                }
                TimerManager.StopTimer();
                nailInsertionDuration = TimerManager.GetDuration();
                TimerManager.ResetTimer();
                TimerManager.StartTimer(); // SHOULD END WHEN DRILLING IS DONE
                CalculateAccuracy();
#if UNITY_EDITOR
                string directory = Application.dataPath + "/savedImages";
                Directory.CreateDirectory(directory);
                guideWireXrayImg = directory + "/GuideWire.png";
#else
                // In build, use persistent path
                string directory = Application.persistentDataPath + "/savedImages";
                Directory.CreateDirectory(directory);
                guideWireXrayImg = directory + "/GuideWire.png";
#endif
                xrayExtraction.SaveXrayImage("GuideWire");
            }
        }
        // player inserted the Guide wire too deep 
        if (other.CompareTag("nailOverLimit") && isDone && !eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.taskText.text = "<b><color=red>WARNING:</color></b> Guide wire inserted too deep! Pull back slightly.";
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
        wirePositionAccuracy = (positionAccuracy + rotationAccuracy) / 2f * 100f;

        Debug.Log($"Guide Wire Position Error: {positionError:F4} m");
        Debug.Log($"Guide Wire Rotation Error: {rotationError:F2}°");
        Debug.Log($"Guide Wire Final Accuracy: {wirePositionAccuracy:F1}%");
    }
    private IEnumerator FadeOut()
    {
        Debug.Log("FadeOut coroutine started");

        Color initialColor = wireMaterial.color;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            wireMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        wireMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        cornerText.text = "Task 3 Is Done!";
        yield return new WaitForSeconds(3f);
        cornerText.text = "Task 4: Nail Locking";

        gameObject.SetActive(false); // Disable only after text is shown
    }

}
