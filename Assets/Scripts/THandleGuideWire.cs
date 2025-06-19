using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class THandleGuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    private Vector3 snapPosition = new Vector3(-0.101f, 1.491f, -0.097f);
    private Quaternion snapRotation = Quaternion.Euler(0.837f, 275.577f, 326.616f);
    public GameObject Awl;
    public float neededThandleDepth;
    public float actualThandleDepth;
    public float tHandleAccuracy;
    public float entrySiteDuration;
    private bool IsTrainingMode => SceneManager.GetActiveScene().name == "TrainingScene";

    public AudioSource alarmAudioSource;
    public AudioClip alarmClip;
    public TextMeshProUGUI taskText;
    public GameObject taskPanel;
    private void OnTriggerEnter(Collider other)
    {
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("THandle Limit"))
        {
            // Snap to the target position and rotation
            if (!IsTrainingMode)
            {
                transform.position = snapPosition;
                transform.rotation = snapRotation;
                taskPanel.SetActive(true);
                taskText.text = "<b><color=green>SUCCESS:</color></b> Correct guide wire depth. Ready to continue";
                StartCoroutine(StopAlarmAfterSeconds(3f));
            }
            Awl.SetActive(true);
            Awl.GetComponent<Animator>().enabled = false;
            // begin awl task after T-Handle is used 
            eventManager.OnEventTHandleUsed();
        }
        if (other.CompareTag("THandle")&& !IsTrainingMode)
        {
            taskPanel.SetActive(true);
            taskText.text = "<b><color=red>WARNING:</color></b> Guide wire inserted too deep! Pull back slightly before you continue.";
            if (alarmAudioSource && alarmClip)
            {
                alarmAudioSource.clip = alarmClip;
                alarmAudioSource.Play();
                StartCoroutine(StopAlarmAfterSeconds(3f));
            }
        }
    }
    private IEnumerator StopAlarmAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (alarmAudioSource.isPlaying)
            alarmAudioSource.Stop();
        taskPanel.SetActive(false);
    }
}
