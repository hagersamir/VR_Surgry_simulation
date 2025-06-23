using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class Awl : MonoBehaviour
{
    public EventManager eventManager;      // Assign in inspector
    public TextMeshProUGUI cornerText;
    public XRGrabInteractable grab;
    public GameObject guideWire, THandle;  // Assign in inspector
    public float fadeDuration = 1.5f;      // Duration of the fade effect (seconds)
    private Material awlMaterial;
    private Material tHandleMaterial;
    private Animator awlAnimator;
    private bool isFading = false;
    public float entrySiteDuration;
    void Awake()
    {
        grab = GetComponent<XRGrabInteractableTwoAttach>();
    }
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
        if (other.CompareTag("toolAlign") && !eventManager.IsTrainingMode)
        {
            Animator animator = GetComponent<Animator>();
            animator.enabled = true;
            StartCoroutine(Animate(animator));
            // grab.enabled = false;
        }
        if (other.CompareTag("Awl Limit") && !isFading)
        {
            isFading = true;

            if (!eventManager.IsTrainingMode)
            {
                eventManager.taskPanel.SetActive(true);
                eventManager.taskText.text = "<b><color=green>SUCCESS:</color></b> Entry canal successfully opened, you can proceed";
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
                TimerManager.StopTimer();
                entrySiteDuration = TimerManager.GetDuration();
                TimerManager.ResetTimer();
                TimerManager.StartTimer();
            }
            eventManager.OnEventAwlUsed();
            StartCoroutine(PlayReverseAnimationAndFadeOut());
        }
        if (other.CompareTag("awlOverLimit") && !eventManager.IsTrainingMode)
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
    IEnumerator Animate(Animator animator)
    {
        if (animator != null)
        {
            animator.Play("Awl"); // Play the insertion animation
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


