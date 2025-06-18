using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Blade : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public float fadeDuration = 4.0f; // Duration of the fade effect (seconds)
    private Material bladeMaterial;
    private bool isFading = false;
    public GameObject THandle;
    public GameObject guideWire, guideWireRemovalDetector;
    public string cuttingScreenshotImg;
    public float cuttingAccuracy;
    private bool IsTrainingMode => SceneManager.GetActiveScene().name == "TrainingScene";
    public AudioSource alarmAudioSource;
    public AudioClip alarmClip;
    public TextMeshProUGUI taskText;
    public GameObject taskPanel;


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
            if (IsTrainingMode)
            {
                // Start the fading animation
                StartCoroutine(FadeOut());
            }
            else
            {
                TakeScreenshot();
                taskPanel.SetActive(true);
                taskText.text = "<b><color=green>SUCCESS:</color></b> Correct entry site!";
                StartCoroutine(StopAlarmAfterSeconds(3f));
            }
            //begin THandle Task
            eventManager.OnEventSkinCut();
        }
        if (other.CompareTag("notEntry") && !IsTrainingMode)
        {
            taskPanel.SetActive(true);
            taskText.text = "<b><color=red>WARNING:</color></b> Incorrect entry site!";
            if (alarmAudioSource && alarmClip)
            {
                alarmAudioSource.clip = alarmClip;
                alarmAudioSource.Play();
                StartCoroutine(StopAlarmAfterSeconds(3f));
            }
        }
    }
    void TakeScreenshot()
    {
        cuttingScreenshotImg = Application.dataPath + "/savedImages/EntrySiteCut.png";
        ScreenCapture.CaptureScreenshot(cuttingScreenshotImg);
        Debug.Log("Screenshot saved to: " + cuttingScreenshotImg);
    }

    private IEnumerator StopAlarmAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (alarmAudioSource.isPlaying)
            alarmAudioSource.Stop();
        taskPanel.SetActive(false);
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
