using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awl : MonoBehaviour
{
    public EventManager eventManager;  // Assign in inspector
    public GameObject guideWire, THandle; // Assign in inspector
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Awl"))
        {
            // prepare for the guide wire step
            THandle.SetActive(false);
            guideWire.SetActive(true);
            
            //begin guide wire insertion task after the awl is used
            eventManager.OnEventAwlUsed();
            
            // Deactivate the awl itself
            gameObject.SetActive(false);
        }
    }
}
