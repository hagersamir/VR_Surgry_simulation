using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Awl : MonoBehaviour
{
    public EventManager eventManager;      // Assign in inspector
    public TextMeshProUGUI cornerText;
    public GameObject guideWire, THandle;  // Assign in inspector
    public float fadeDuration = 1.5f;      // Duration of the fade effect (seconds)
    private Material awlMaterial;
    private Material tHandleMaterial;
    private Animator awlAnimator;
    private bool isFading = false;
    public float entrySiteDuration;

    private void Start()
    {
        guideWire.GetComponent<XRGrabInteractableTwoAttach>().enabled = false;
        awlMaterial = GetComponent<Renderer>().material;

        if (THandle != null)
            tHandleMaterial = THandle.GetComponent<Renderer>().material;

        awlAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Awl Limit") && !isFading)
        {
            isFading = true;

            if (!eventManager.IsTrainingMode)
            {
                // }
                // else
                // {
                eventManager.taskPanel.SetActive(true);
                eventManager.taskText.text = "<b><color=green>SUCCESS:</color></b> Entry canal successfully opened, you can proceed";
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
                TimerManager.StopTimer();
                entrySiteDuration = TimerManager.GetDuration();
                TimerManager.ResetTimer();
                TimerManager.StartTimer();
                // eventManager.taskPanel.SetActive(true);
                // eventManager.taskText.text = "<b><color=blue>Hint:</color></b> You can remove the Awl and the T-Handle";
                // StartCoroutine(eventManager.StopAlarmAfterSeconds(5f));
            }
            eventManager.OnEventAwlUsed();
            StartCoroutine(PlayReverseAnimationAndFadeOut());
        }
        if (other.CompareTag("AwlOverLimit") && !eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.taskText.text = "<b><color=red>WARNING:</color></b> Be carefull not to damage internal structures or the far cortex";
            if (eventManager.alarmAudioSource && eventManager.alarmClip)
            {
                eventManager.alarmAudioSource.clip = eventManager.alarmClip;
                eventManager.alarmAudioSource.Play();
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
            }
        }
    }

    private IEnumerator PlayReverseAnimationAndFadeOut()
    {
        if (awlAnimator != null)
        {
            awlAnimator.enabled = true;
            awlAnimator.Play("Awl_Reverse");
            float reverseClipLength = awlAnimator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == "Awl_Reverse")?.length ?? 1f;
            yield return new WaitForSeconds(reverseClipLength);
        }

        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color initialAwlColor = awlMaterial.color;
        Color initialTHandleColor = tHandleMaterial != null ? tHandleMaterial.color : Color.clear;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            awlMaterial.color = new Color(initialAwlColor.r, initialAwlColor.g, initialAwlColor.b, alpha);
            if (tHandleMaterial != null)
            {
                tHandleMaterial.color = new Color(initialTHandleColor.r, initialTHandleColor.g, initialTHandleColor.b, alpha);
            }

            yield return null;
        }

        awlMaterial.color = new Color(initialAwlColor.r, initialAwlColor.g, initialAwlColor.b, 0f);
        if (tHandleMaterial != null)
        {
            tHandleMaterial.color = new Color(initialTHandleColor.r, initialTHandleColor.g, initialTHandleColor.b, 0f);
        }
        cornerText.text = "Task 2 Is Done!";
        yield return new WaitForSeconds(2f);
        cornerText.text = "Task 3: Guide Wire And Nail Insertion";

        gameObject.SetActive(false);
        guideWire.transform.SetParent(null);
        if (THandle != null)
            THandle.SetActive(false);

        guideWire.GetComponent<XRGrabInteractableTwoAttach>().enabled = true;
        if (eventManager.IsTrainingMode)
        {
            guideWire.GetComponent<GuideWireConstraint>().enabled = true;
        }
        // guideWire.SetActive(true);
        // guideWire.transform.position = new Vector3(-0.113f, 1.244f, -0.257f);
        // guideWire.transform.rotation = Quaternion.Euler(54.546f, 182.119f, 182.141f);
    }
}

