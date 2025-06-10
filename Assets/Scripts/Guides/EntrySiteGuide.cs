using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySiteGuide : MonoBehaviour
{
private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the same tag
        if (other.CompareTag("EntrySite"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;

            StartCoroutine(ApplyAndFreeze(other.transform));
        }
    }

    IEnumerator ApplyAndFreeze(Transform target)
    {
        // Match world transform
        target.position = transform.position;
        // target.position = transform.position;
        target.rotation = transform.rotation;
        target.GetComponent<MeshCollider>().enabled = true;
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Vector3 frozenPos = target.position;
        Quaternion frozenRot = target.rotation;

        target.position = new Vector3(frozenPos.x, frozenPos.y, frozenPos.z);
        target.rotation = frozenRot;

        Animator animator = target.GetComponent<Animator>();
        animator.enabled = true;
        if (animator != null)
        {
            animator.Play("Blade"); // Play the insertion animation
            Debug.Log("Animation started");

            // Wait for animation to finish before proceeding
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        }
        else
        {
            Debug.LogWarning("No Animator found on tool.");
        }

        animator.enabled = false;
        gameObject.SetActive(false);
    }
}
