using UnityEngine;

public class ScrewAttachment : MonoBehaviour
{

    private bool isInHole = false;

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

            // Set the screw as a child of the screwdriver
            screw.SetParent(screwdriver);

            // Apply the specified world transformation
            screw.transform.localPosition = new Vector3(-0.23f, 0.28f, 0.71f);
            screw.transform.localScale = new Vector3(1.17f, 1.17f, 1.17f);
            screw.transform.localRotation = new Quaternion(-0.01696f, 0.70742f, 0.01890f, -0.70634f);

        }
        if (other.CompareTag("hole"))
        {
            isInHole = true;
            Debug.Log("brush");
            transform.SetParent(null);
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hole"))
            isInHole = false;
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

