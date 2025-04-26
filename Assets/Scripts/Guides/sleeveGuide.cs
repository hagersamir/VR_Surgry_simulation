using UnityEngine;
using System.Collections;

public class MoveForwardAndReset : MonoBehaviour
{
    public float distance = 0.04f; // Distance to move before resetting
    public float speed = 0.01f; // Speed of movement

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the object forward on the X axis
        transform.position += -transform.forward * speed * Time.deltaTime;

        // Check if it moved beyond the distance
        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            // Reset position
            transform.position = startPosition;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sleeve"))
        {
            StartCoroutine(ApplyAndFreeze(other.transform));
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