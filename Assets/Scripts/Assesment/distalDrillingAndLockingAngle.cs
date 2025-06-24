using UnityEngine;
using TMPro;
public class RotationCheck : MonoBehaviour
{
    public float angle;
    public GameObject tool;
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            float xRotation = tool.transform.localEulerAngles.z;

            // Normalize rotation to 0–360
            xRotation = NormalizeAngle(xRotation);

            if (Mathf.Abs(xRotation - angle) > 8f)
            {
                Debug.Log("no");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("distalAngleDetection"))
        {
            float xRotation = tool.transform.localEulerAngles.z;

            // Normalize rotation to 0–360
            xRotation = NormalizeAngle(xRotation);

            if (Mathf.Abs(xRotation - angle) > 8f)
            {
                taskPanel.SetActive(true);
                taskText.text = "<b><color=red>WARNING:</color></b>Wrong locking angle";
                Debug.Log("no");
            }
            else
            {
                taskText.text = "<b><color=green>WARNING:</color></b>right locking angle";

            }
        }
    }

    // Helper to wrap angles like -180 to 180 or >360 to 0-360
    float NormalizeAngle(float angle)
    {
        return (angle + 360f) % 360f;
    }
}
