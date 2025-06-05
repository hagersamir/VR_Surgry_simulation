using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public float fadeDuration = 1.5f; // Duration of the fade effect (seconds)
    private Material bladeMaterial;
    private bool isFading = false;
    public GameObject THandle;
    public GameObject guideWire, guideWireRemovalDetector;

    private void Start()
    {
        bladeMaterial = GetComponent<Renderer>().material;
        guideWireRemovalDetector.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        // player has finished using the blade
        if (other.CompareTag("EntrySite") && !isFading)
        {
            isFading = true;
            // Start the fading animation
            StartCoroutine(FadeOut());
            //begin THandle Task
            eventManager.OnEventSkinCut();
        }
    }
    private IEnumerator FadeOut()
    {
        Color initialColor = bladeMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            bladeMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Ensure it's fully transparent at the end
        bladeMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        // Optionally disable the object after fading out
        gameObject.SetActive(false);
        THandle.SetActive(true);
        THandle.GetComponent<Animator>().enabled = false;
        guideWire.GetComponent<Animator>().enabled = false;
    }
}
