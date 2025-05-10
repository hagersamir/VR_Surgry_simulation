using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideWire : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    
    private void OnTriggerEnter(Collider other)
    {
        // player has finished inserting the Guide wire 
        if (other.CompareTag("wire"))
        {
            //begin nail task after Guide wire is inserted 
            eventManager.OnEventGuideWireUsed();
        }
    }
}
