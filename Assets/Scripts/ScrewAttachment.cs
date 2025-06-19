using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using TMPro;
public class ScrewAttachment : MonoBehaviour
{

    public bool isInHole = false;
    public EventManager eventManager; // Assign in inspector
    private bool hasScrewChild = false;
    public bool ScrewPlaced = false;
    private MeshCollider mc;
    public StepManager stepManager; // Drag ObjectA (with ScriptA) here in the inspector
    public XRayExtraction xrayExtraction; // Drag ObjectA (with ScriptA) here in the inspector
    public string tagOfScrewDeattachment;
    public GameObject sendData;

    public float ScrewPositionAcc;


    private bool IsTrainingMode => SceneManager.GetActiveScene().name == "TrainingScene";
    public AudioSource alarmAudioSource;
    public AudioClip alarmClip;
    public TextMeshProUGUI taskText;
    public GameObject taskPanel;

    public string wrongLoking1;
    public string wrongLoking2;
    private void Start()
    {
        Debug.Log($"Local Position: {transform.localPosition}");
        Debug.Log($"Local Rotation: {transform.localRotation}");
        Debug.Log($"Local Scale: {transform.localScale}");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("saf");

        if (other.CompareTag("ScrewDriver") && !isInHole)

        {

            Transform screw = transform;
            Transform screwdriver = other.transform;
            mc = screwdriver.GetComponent<MeshCollider>();

            mc.enabled = false;
            // foreach (Transform child in other.transform)
            // {
            //     if (child.CompareTag("Screw"))
            //     {
            //         hasScrewChild = true;
            //         break;
            //     }
            // }

            // Set the screw as a child of the screwdriver
            // if (!hasScrewChild)
            // {
            screw.SetParent(screwdriver);

            // Apply the specified world transformation
            screw.transform.localPosition = new Vector3(-0.243f, 0.282f, 0.696f);
            // screw.transform.localScale = new Vector3(1.17f, 1.17f, 1.17f);
            screw.transform.localRotation = new Quaternion(-0.01696f, 0.70742f, 0.01890f, -0.70634f);
            // }

        }
        if (other.CompareTag(tagOfScrewDeattachment))
        {
            isInHole = true;
            Debug.Log("brush");
            transform.SetParent(other.transform);
            mc.enabled = true;

            // transform.tag = "Untagged";
            // hasScrewChild = false;

            transform.GetComponent<MeshCollider>().enabled = false;
            BoxCollider box = transform.GetComponent<BoxCollider>();
            if (box != null)
            {
                box.enabled = false;
            }

            ScrewPlaced = true;
            // eventManager.OnEventProximalScrew_1();
            if (eventManager.isDistalLocking && SceneManager.GetActiveScene().name == "TrainingScene" && stepManager != null)
            {

                stepManager.Locking_ClosureCompleted();
            }
            if (eventManager.isDistalLocking && SceneManager.GetActiveScene().name == "AssessmentScene")
            {

                sendData.SetActive(true);
            }
            if (xrayExtraction != null)
            {

                xrayExtraction.SaveXrayImage("Locking Screw", "locking screw other view");
            }

        }
        if (other.CompareTag("ProximalLock1"))
        {
            Debug.Log("proximal1");
            eventManager.OnEventProximalScrew_1();
            other.GetComponent<BoxCollider>().enabled = false;


        }
        else if (other.CompareTag("ProximalLock2"))
        {
            eventManager.OnEventProximalLockingDone();
            other.GetComponent<BoxCollider>().enabled = false;
            Debug.Log("proximal2");

        }

        if (SceneManager.GetActiveScene().name == "AssessmentScene")
        {

            if (other.tag == wrongLoking1 || other.tag == wrongLoking2)
            {
                taskPanel.SetActive(true);
                taskText.text = "<b><color=red>WARNING:</color></b> Wrong Screw Length";
                StartCoroutine(StopAlarmAfterSeconds(3f));
            }
            else if (other.CompareTag(transform.tag))
            {
                taskPanel.SetActive(true);
                taskText.text = "<b><color=green>Sucsess:</color></b> Right Screw Lenght";
                if (alarmAudioSource && alarmClip)
                {
                    alarmAudioSource.clip = alarmClip;
                    alarmAudioSource.Play();
                    StartCoroutine(StopAlarmAfterSeconds(3f));
                }
            }
        }
        // if (other.CompareTag("Bone") )// in here 
        // {
        //     Debug.Log("hole and bone");
        //     transform.SetParent(other.transform);
        // }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hole"))
            isInHole = false;
    }


    private IEnumerator StopAlarmAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (alarmAudioSource.isPlaying)
            alarmAudioSource.Stop();
        taskPanel.SetActive(false);
    }

}


// Local Position: (-0.23, 0.28, 0.71)
// UnityEngine.Debug:Log (object)
// ScrewAttachment:Start () (at Assets/Scripts/ScrewAttachment.cs:7)

// Local Rotation: (-0.01696, 0.70742, 0.01890, -0.70634)
// UnityEngine.Debug:Log (object)
// ScrewAttachment:Start () (at Assets/Scripts/ScrewAttachment.cs:8)

// Local Scale: (1.17, 1.17, 1.17)
// UnityEngine.Debug:Log (object)
// ScrewAttachment:Start () (at Assets/Scripts/ScrewAttachment.cs:9)