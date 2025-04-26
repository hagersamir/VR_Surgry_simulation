using UnityEngine;
using System.Collections;

public class blade : MonoBehaviour
{
    public float forwardDistance = 0.005f; // Distance to move forward
    public float downwardDistance = 0.01f; // Distance to move downward
    public float speed = 0.01f; // Speed of movement

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        // Store the initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Start the animation coroutine
        StartCoroutine(AnimateBlade());
    }

    IEnumerator AnimateBlade()
    {
        while (true)
        {
            // Step 1: Move forward for the specified distance
            float traveled = 0f;
            while (traveled < forwardDistance)
            {
                transform.position += transform.right * speed * Time.deltaTime;
                traveled += speed * Time.deltaTime;
                yield return null;
            }

            // Step 2: Move downward for the specified distance
            traveled = 0f;
            Vector3 downwardDirection = transform.up;
            while (traveled < downwardDistance)
            {
                transform.position += downwardDirection * speed * 0.5f * Time.deltaTime;
                traveled += speed * Time.deltaTime;
                yield return null;
            }

            // Step 3: Move backward for the specified distance
            traveled = 0f;
            while (traveled < forwardDistance)
            {
                transform.position += -transform.right * speed * Time.deltaTime;
                traveled += speed * Time.deltaTime;
                yield return null;
            }

            // Reset position and rotation
            transform.position = startPosition;
            transform.rotation = startRotation;

            yield return null; // Optional: short pause if needed
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("small cut"))
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
