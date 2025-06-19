using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;

public class LockingCutsLocations : MonoBehaviour
{
    // Start is called before the first frame update

    public bool rightCutLocation = false;
    public AudioSource alarmAudioSource;
    public AudioClip alarmClip;
    public TextMeshProUGUI taskText;
    public GameObject taskPanel;

    public string wrongLoking1;
    public string wrongLoking2;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("First Proximal Locking Hole") || other.CompareTag("Second Proximal Locking Hole") || other.CompareTag("Distal Locking Hole"))
        {
            rightCutLocation = true;
        }

        if (other.CompareTag("Skin"))
        {
            if (rightCutLocation)
            {
                taskPanel.SetActive(true);
                taskText.text = "<b><color=green>Sucsess:</color></b> Right Cutting Location";
                StartCoroutine(StopAlarmAfterSeconds(3f));
            }
            else
            {
                taskPanel.SetActive(true);
                taskText.text = "<b><color=red>WARNING:</color></b> Wrong Cutting Location";
                if (alarmAudioSource && alarmClip)
                {
                    alarmAudioSource.clip = alarmClip;
                    alarmAudioSource.Play();
                    StartCoroutine(StopAlarmAfterSeconds(3f));
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("First Proximal Locking Hole") || other.CompareTag("Second Proximal Locking Hole") || other.CompareTag("Distal Locking Hole"))
        {
            rightCutLocation = false;
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
