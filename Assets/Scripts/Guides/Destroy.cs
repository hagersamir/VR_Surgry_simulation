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

    IEnumerator ApplyAndFreeze(Transform target)
    {
        // Cache the parent (in case it's parented and affected by it)
        Transform originalParent = target.parent;

        // Match world transform
        target.position = transform.position;
        target.rotation = transform.rotation;

        // Freeze: temporarily disable movement
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // prevents physics movement
        }

        Vector3 frozenPos = target.position;
        Quaternion frozenRot = target.rotation;

        float timer = 1f;
        while (timer > 0f)
        {
            target.position = frozenPos;
            target.rotation = frozenRot;
            timer -= Time.deltaTime;
            yield return null;
        }

        // Unfreeze
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
