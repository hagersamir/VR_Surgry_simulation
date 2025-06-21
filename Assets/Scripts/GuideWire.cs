using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public TextMeshProUGUI cornerText;
    public float fadeDuration = 1.5f; // Duration of the fade effect (seconds)
    // private Vector3 snapPosition = new Vector3(-0.113f, 1.056f, -0.388f);
    private Vector3 snapPosition = new Vector3(-0.108999997f, 1.05799997f, -0.388000011f);
    // private Quaternion snapRotation = Quaternion.Euler(54.546f, 182.119f, 182.141f);
    private Quaternion snapRotation = Quaternion.Euler(0, 0, 0);
    private Material wireMaterial;
    private bool isFading = false;
    private bool isDone = false;
    public GameObject aimingGuide;
    public string guideWireXrayImg;
    public float neededWireDepth;
    public float actualWireDepth;
    public float wirePositionAccuracy;
    public float nailInsertionDuration;

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
            transform.position = snapPosition;
            transform.rotation = snapRotation;
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
            TimerManager.StopTimer();
            nailInsertionDuration = TimerManager.GetDuration();
            TimerManager.ResetTimer();
            TimerManager.StartTimer(); // SHOULD END WHEN DRILLING IS DONE
        }
        // player inserted the Guide wire too deep 
        if (other.CompareTag("nailOverLimit") && isDone && eventManager.IsTrainingMode)
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
