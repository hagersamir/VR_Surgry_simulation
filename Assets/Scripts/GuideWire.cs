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
    public GameObject aimingGuide;

    private void Start()
    {
        wireMaterial = GetComponent<Renderer>().material;
    }
    private void OnTriggerEnter(Collider other)
    {
        // player has finished inserting the Guide wire 
        if (other.CompareTag("wire"))
        {
            // Snap to the target position and rotation
            transform.position = snapPosition;
            transform.rotation = snapRotation;
            // aimingGuide.SetActive(true);
            // aimingGuide.GetComponent<Animator>().enabled = false;
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
            // Start the fading animation
            StartCoroutine(FadeOut());
        }
    }
    private IEnumerator FadeOut()
    {
        Debug.Log("FadeOut coroutine started");

        Color initialColor = wireMaterial.color;

        // // Ensure the material is using transparent rendering
        // wireMaterial.SetFloat("_Mode", 3);
        // wireMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        // wireMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // wireMaterial.SetInt("_ZWrite", 0);
        // wireMaterial.DisableKeyword("_ALPHATEST_ON");
        // wireMaterial.EnableKeyword("_ALPHABLEND_ON");
        // wireMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        // wireMaterial.renderQueue = 3000;

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
