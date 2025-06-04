using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awl : MonoBehaviour
{
    public EventManager eventManager;      // Assign in inspector
    public GameObject guideWire, THandle;  // Assign in inspector
    public float fadeDuration = 1.5f;      // Duration of the fade effect (seconds)

    private Material awlMaterial;
    private Material tHandleMaterial;
    private bool isFading = false;

    private void Start()
    {
        // Get the materials for both Awl and THandle
        awlMaterial = GetComponent<Renderer>().material;

        if (THandle != null)
        {
            tHandleMaterial = THandle.GetComponent<Renderer>().material;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Awl Limit") && !isFading)
        {
            isFading = true;

            // Prepare for the guide wire step
            guideWire.SetActive(true);
            guideWire.transform.position = new Vector3(-0.113f,1.244f,-0.257f);
            guideWire.transform.rotation = Quaternion.Euler(54.546f,182.119f,182.141f);

            // Begin guide wire insertion task after the awl is used
            eventManager.OnEventAwlUsed();

            // Start fading both objects
            StartCoroutine(FadeOut());
        }
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

            // Update materials with the new alpha value
            awlMaterial.color = new Color(initialAwlColor.r, initialAwlColor.g, initialAwlColor.b, alpha);
            if (tHandleMaterial != null)
            {
                tHandleMaterial.color = new Color(initialTHandleColor.r, initialTHandleColor.g, initialTHandleColor.b, alpha);
            }

            yield return null;
        }

        // Ensure they are fully transparent at the end
        awlMaterial.color = new Color(initialAwlColor.r, initialAwlColor.g, initialAwlColor.b, 0f);
        if (tHandleMaterial != null)
        {
            tHandleMaterial.color = new Color(initialTHandleColor.r, initialTHandleColor.g, initialTHandleColor.b, 0f);
        }

        // Deactivate the awl and THandle
        gameObject.SetActive(false);
        if (THandle != null)
        {
            THandle.SetActive(false);
        }
    }
}
