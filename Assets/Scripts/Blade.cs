using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Blade : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public float fadeDuration = 4.0f; // Duration of the fade effect (seconds)
    private Material bladeMaterial;
    private bool isFading = false;
    public Camera screenshotCamera;
    public GameObject THandle;
    public GameObject guideWire, guideWireRemovalDetector;
    public string cuttingScreenshotImg;
    public float cuttingAccuracy;
    private Vector3 targetPosition = new Vector3(-0.118f, 1.272f, -0.238f);
    private Quaternion targetRotation = Quaternion.Euler(359.392242f, 16.1531677f, 5.01080561f);

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
            if (eventManager.IsTrainingMode)
            {
                isFading = true;
                // Start the fading animation
                StartCoroutine(FadeOut());
            }
            else
            {
                TakeScreenshotWithCamera();
                eventManager.taskPanel.SetActive(true);
                eventManager.taskText.text = "<b><color=green>SUCCESS:</color></b> Correct entry site!";
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
                Transform lastCut = cutUnparent.lastUnparentedCut;
                CalculateCutAccuracy(lastCut);
            }
            //begin THandle Task
            eventManager.OnEventSkinCut();
        }
        if (other.CompareTag("notEntry") && !eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.taskText.text = "<b><color=red>WARNING:</color></b> Incorrect entry site!";
            if (eventManager.alarmAudioSource && eventManager.alarmClip)
            {
                eventManager.alarmAudioSource.clip = eventManager.alarmClip;
                eventManager.alarmAudioSource.Play();
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
            }
        }
    }
    public void TakeScreenshotWithCamera()
    {
        int width = 1920;
        int height = 1080;

#if UNITY_EDITOR
        string directory = Application.dataPath + "/savedImages";
        Directory.CreateDirectory(directory);
        cuttingScreenshotImg = directory + "/EntrySiteCut.png";
#else
        // In build, use persistent path
        string directory = Application.persistentDataPath + "/savedImages";
        Directory.CreateDirectory(directory);
        cuttingScreenshotImg = directory + "/EntrySiteCut.png";
#endif

        // Create render texture and texture to hold the screenshot
        RenderTexture rt = new RenderTexture(width, height, 24);
        screenshotCamera.targetTexture = rt;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Render and read pixels
        screenshotCamera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Encode to PNG and save
        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(cuttingScreenshotImg, bytes);

        // Cleanup
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        Debug.Log("Screenshot saved to: " + cuttingScreenshotImg);
    }

    private void CalculateCutAccuracy(Transform actualCut)
    {
        // 1. Actual position & rotation
        Vector3 actualPosition = actualCut.position;
        Quaternion actualRotation = actualCut.rotation;

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
        cuttingAccuracy = (positionAccuracy + rotationAccuracy) / 2f * 100f;

        Debug.Log($"Position Error: {positionError:F4} m");
        Debug.Log($"Rotation Error: {rotationError:F2}°");
        Debug.Log($"Final Accuracy: {cuttingAccuracy:F1}%");
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
