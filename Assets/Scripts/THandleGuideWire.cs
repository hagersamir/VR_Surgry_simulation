using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THandleGuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    private Vector3 snapPosition = new Vector3(-0.101f,1.491f,-0.097f);
    private Quaternion snapRotation = Quaternion.Euler(0.837f,275.577f,326.616f);
    public GameObject Awl;
    private void OnTriggerEnter(Collider other)
    {
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("THandle Limit"))
        {
            // Snap to the target position and rotation
            transform.position = snapPosition;
            transform.rotation = snapRotation;
            Awl.SetActive(true);
            // begin awl task after T-Handle is used 
            eventManager.OnEventTHandleUsed();
        }
    }
}
