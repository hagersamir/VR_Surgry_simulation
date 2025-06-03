using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public TextMeshProUGUI cornerText;
    public float fadeDuration = 1.5f; // Duration of the fade effect (seconds)
    private Vector3 snapPosition = new Vector3(-0.113f,1.056f,-0.388f);
    private Quaternion snapRotation = Quaternion.Euler(54.546f,182.119f,182.141f);
    private Material wireMaterial;
    private bool isFading = false;
    public GameObject aimingGuide;

    private void Start()
    {
        wireMaterial = GetComponent<Renderer>().material;
    }
    private void OnTriggerEnter(Collider other)
    {
        // player has finished inserting the Guide wire 
        if (other.CompareTag("wire") && !aimingGuide.activeSelf)
        {
            // Snap to the target position and rotation
            transform.position = snapPosition;
            transform.rotation = snapRotation;
            aimingGuide.SetActive(true);
            //begin nail task after Guide wire is inserted 
            eventManager.OnEventGuideWireUsed();
        }

        // player has finished REMOVING the Guide wire 
        if (other.CompareTag("GuideWire") && !isFading)
        {
            isFading = true;
            // Start the fading animation
            StartCoroutine(FadeOut());
            // cornerText.text = "Step 2 is Done!";
            // yield return new WaitForSeconds(2f);
            // cornerText.text = "";

            // Begin DRILLING TASKs after Guide wire is removed 
            // HERE
        }
    }
    private IEnumerator FadeOut()
    {
        Color initialColor = wireMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            wireMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Ensure it's fully transparent at the end
        wireMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        // Optionally disable the object after fading out
        gameObject.SetActive(false);

        cornerText.text = "Task 2 is Done!";
        yield return new WaitForSeconds(2f);
        cornerText.text = "Task 3: Nail Locking";
    }
}
