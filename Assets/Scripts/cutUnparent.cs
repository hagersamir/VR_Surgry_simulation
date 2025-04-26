using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DecalSystem;


public class cutUnparent : MonoBehaviour
{
    private bool isCollidingWithSkin = false;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skin"))
        {
            isCollidingWithSkin = true;
        }
    }

    // Detect exit from a "Skin"-tagged object
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Skin"))
        {
            isCollidingWithSkin = false;
        }
    }
    private void Update()
    {
        if (!isCollidingWithSkin)
        {
            transform.SetParent(null); // Remove the child from the parent
            Decal decalComponent = transform.GetComponent<Decal>();

            decalComponent.BuildAndSetDirty();

            Debug.Log("Child has been unparented!");
        }

    }
}
