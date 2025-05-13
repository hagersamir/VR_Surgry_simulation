// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DestroyIfSameTag : MonoBehaviour
// {
//     private bool isColliderLocked = false;

//     private void OnTriggerEnter(Collider other)
//     {
//         if (gameObject.tag == other.tag & other.gameObject != null)
//         {
//             Debug.Log($"{gameObject.name}collided with {other.name}");
//             GetComponent<MeshCollider>().enabled = false;

//             StartCoroutine(ApplyAndFreeze(other.transform));
//             // Destroy(gameObject);
//             // GetComponent<MeshCollider>().enabled = false;
//             // StartCoroutine(TemporarilyDisableCollider(5f));

//             // StartCoroutine(DisableColliderAfterDelay(0.5f)); // Small delay

//             // foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
//             // {
//             //     mr.enabled = false;
//             // }
//             // Rigidbody rb = GetComponent<Rigidbody>();
//             // if (rb != null)
//             // {
//             //     rb.velocity = Vector3.zero;
//             //     rb.angularVelocity = Vector3.zero;
//             //     rb.constraints = RigidbodyConstraints.FreezeAll;
//             // }

//         }

//     }


//     private IEnumerator TemporarilyDisableCollider(float duration)
//     {
//         float timer = duration;
//         while (timer > 0f)
//         {
//             GetComponent<MeshCollider>().enabled = false;
//             timer -= Time.deltaTime;
//             yield return null;
//         }
//     }

//     // private IEnumerator DisableColliderAfterDelay(float delay)
//     // {
//     //     yield return new WaitForSeconds(delay);
//     //     GetComponent<MeshCollider>().enabled = false;
//     // }
//     IEnumerator ApplyAndFreeze(Transform target)
//     {
//         // Cache the parent (in case it's parented and affected by it)
//         Transform originalParent = target.parent;

//         // Match world transform
//         target.position = transform.position;
//         // target.position = transform.position;
//         target.rotation = transform.rotation;

//         Rigidbody rb = target.GetComponent<Rigidbody>();
//         if (rb != null)
//         {
//             rb.isKinematic = true;
//         }

//         Vector3 frozenPos = target.position;
//         Quaternion frozenRot = target.rotation;

//         float timer = 2f;
//         float moveSpeed = 0.02f; // units per second on the x-axis
//         float elapsed = 0f;

//         while (timer > 0f)
//         {
//             float xOffset = elapsed * moveSpeed;
//             target.position = new Vector3(frozenPos.x - xOffset, frozenPos.y, frozenPos.z);
//             target.rotation = frozenRot;

//             elapsed += Time.deltaTime;
//             timer -= Time.deltaTime;
//             yield return null;
//         }

//         if (rb != null)
//         {
//             // rb.isKinematic = false;
//         }
//         gameObject.SetActive(false);

//     }
// }