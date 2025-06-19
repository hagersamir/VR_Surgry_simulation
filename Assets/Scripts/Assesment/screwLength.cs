using UnityEngine.SceneManagement;
using UnityEngine;

using System.Collections;
using TMPro;
public class screwLength : MonoBehaviour
{


    private bool IsTrainingMode => SceneManager.GetActiveScene().name == "TrainingScene";
    public AudioSource alarmAudioSource;
    public AudioClip alarmClip;
    public TextMeshProUGUI taskText;
    public GameObject taskPanel;

    public string wrongLoking1;
    public string wrongLoking2;
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
        if (other.tag == wrongLoking1 || other.tag == wrongLoking2)
        {
            taskPanel.SetActive(true);
            taskText.text = "<b><color=red>WARNING:</color></b> Wrong Screw Length";
            if (alarmAudioSource && alarmClip)
            {
                alarmAudioSource.clip = alarmClip;
                alarmAudioSource.Play();
                StartCoroutine(StopAlarmAfterSeconds(3f));
            }
        }
        else if (other.CompareTag(transform.tag))
        {
            taskPanel.SetActive(true);
            taskText.text = "<b><color=green>Sucsess:</color></b> Right Screw Lenght";
            StartCoroutine(StopAlarmAfterSeconds(3f));

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
