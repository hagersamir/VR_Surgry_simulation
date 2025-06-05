using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Awl : MonoBehaviour
{
    public EventManager eventManager;      // Assign in inspector
    public GameObject guideWire, THandle;  // Assign in inspector
    public float fadeDuration = 1.5f;      // Duration of the fade effect (seconds)
    // public string animationName = "Awl"; // Name of forward animation

    private Material awlMaterial;
    private Material tHandleMaterial;
    private Animator awlAnimator;
    private bool isFading = false;

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

            eventManager.OnEventAwlUsed();

            StartCoroutine(PlayReverseAnimationAndFadeOut());
        }
    }

    private IEnumerator PlayReverseAnimationAndFadeOut()
    {
        if (awlAnimator != null)
        {
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

        gameObject.SetActive(false);
        if (THandle != null)
            THandle.SetActive(false);

        guideWire.transform.SetParent(null);
        guideWire.GetComponent<XRGrabInteractableTwoAttach>().enabled = true;
        guideWire.GetComponent<GuideWireConstraint>().enabled = true;
        // guideWire.SetActive(true);
        // guideWire.transform.position = new Vector3(-0.113f, 1.244f, -0.257f);
        // guideWire.transform.rotation = Quaternion.Euler(54.546f, 182.119f, 182.141f);
    }
}

