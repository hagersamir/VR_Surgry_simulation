using UnityEngine;

public class THandleGuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    private Vector3 snapPosition = new Vector3(-0.109999999f, 1.51999998f, -0.125f);
    private Quaternion snapRotation = Quaternion.Euler(0.837f, 275.577f, 326.616f);
    public GameObject Awl;
    public float neededThandleDepth;
    public float actualThandleDepth;
    public float tHandleAccuracy;

    private void OnTriggerEnter(Collider other)
    {
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("THandle Limit"))
        {
            // Snap to the target position and rotation
            if (!eventManager.IsTrainingMode)
            {
                transform.position = snapPosition;
                transform.rotation = snapRotation;
                eventManager.taskPanel.SetActive(true);
                eventManager.taskText.text = "<b><color=green>SUCCESS:</color></b> Correct guide wire depth. Ready to continue";
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
            }
            Awl.SetActive(true);
            Awl.GetComponent<Animator>().enabled = false;
            // begin awl task after T-Handle is used 
            eventManager.OnEventTHandleUsed();
        }
        if (other.CompareTag("THandle") && !eventManager.IsTrainingMode)
        {
            eventManager.taskPanel.SetActive(true);
            eventManager.taskText.text = "<b><color=red>WARNING:</color></b> Guide wire inserted too deep! Pull back slightly before you continue.";
            if (eventManager.alarmAudioSource && eventManager.alarmClip)
            {
                eventManager.alarmAudioSource.clip = eventManager.alarmClip;
                eventManager.alarmAudioSource.Play();
                StartCoroutine(eventManager.StopAlarmAfterSeconds(3f));
            }
        }
    }
}
