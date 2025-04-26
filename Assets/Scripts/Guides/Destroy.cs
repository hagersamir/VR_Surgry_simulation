using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfSameTag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == other.tag)
        {
            Destroy(gameObject);
        }
    }
}
