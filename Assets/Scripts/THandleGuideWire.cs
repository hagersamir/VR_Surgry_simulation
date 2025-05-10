using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THandleGuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    
    private void OnTriggerEnter(Collider other)
    {
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("THandle"))
        {
            //begin awl task after T-Handle is used 
            eventManager.OnEventTHandleUsed();
        }
    }
}
