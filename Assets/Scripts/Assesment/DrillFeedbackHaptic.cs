using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrillFeedbackHaptic : MonoBehaviour
{
    public AudioSource alarmAudioSource;
    public AudioClip alarmClip;

    public TextMeshProUGUI taskText;
    public GameObject taskPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bone"))
        {
            taskPanel.SetActive(true);
            taskText.text = "Drill Collide The Bone";
            StartCoroutine(StopAlarmAfterSeconds(3f));

        }
        if (other.CompareTag("Drill Limit"))
        {
            taskPanel.SetActive(true);
            taskText.text = "<b><color=red>WARNING:</color></b> Drill inserted too deep! ";
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
        taskPanel.SetActive(false);
    }
}
