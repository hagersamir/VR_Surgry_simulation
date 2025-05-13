using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector

    // Target snap position and rotation
    private Vector3 snapPosition = new Vector3(-0.014f, 1.207f, -0.2775f);
    private Quaternion snapRotation = Quaternion.Euler(58.928f, 182.693f, 2.088f);
    private void OnTriggerEnter(Collider other)
    {
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("wire"))
        {
            // Snap to the target position and rotation
            transform.position = snapPosition;
            transform.rotation = snapRotation;
            //begin Guide wire removal task after nail is inserted 
            eventManager.OnEventNailUsed();
        }
    }
}
