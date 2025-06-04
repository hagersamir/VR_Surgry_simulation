using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGuides : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the same tag
        if (other.CompareTag(gameObject.tag))
        {
            Debug.Log($"{gameObject.name} vanished due to collision with {other.name}");

            // Hide all renderers
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = false;
            }

            // Optionally disable this object's collider to avoid repeat triggers
            GetComponent<Collider>().enabled = false;
        }
    }


}
