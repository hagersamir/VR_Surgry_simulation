using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    
    private void OnTriggerEnter(Collider other)
    {
        // player has finished using the T-Handle Guide wire 
        if (other.CompareTag("wire"))
        {
            //begin Guide wire removal task after nail is inserted 
            eventManager.OnEventNailUsed();
        }
    }
}
