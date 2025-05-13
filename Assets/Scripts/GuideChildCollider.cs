using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideChildCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject guideParent;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} ColliderDistance2D with {other.name}");

    }
    private void OnTriggerExit(Collider other)
    {
        
        guideParent.GetComponent<MeshCollider>().enabled = true;
    }
}