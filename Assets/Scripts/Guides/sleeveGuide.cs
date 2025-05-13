using UnityEngine;
using System.Collections;

public class MoveForwardAndReset : MonoBehaviour
{
    public float distance = 0.04f; // Distance to move before resetting
    public float speed = 0.01f;    // Speed of movement

    private Vector3 startPosition;
    private bool isMoving = true;  // Flag to control movement

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!isMoving) return; // Skip movement if flagged

        // Move the object forward on the X axis
        transform.position += -transform.forward * speed * Time.deltaTime;

        // Reset if it exceeds distance
        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            transform.position = startPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == other.tag && other.gameObject != null)
        {
            isMoving = false; // Stop movement
            Debug.Log($"{gameObject.name}collided with {other.name}");
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;

            StartCoroutine(ApplyAndFreeze(other.transform));
        }
    }

    IEnumerator ApplyAndFreeze(Transform target)
    {
        // Cache the parent (in case it's parented and affected by it)
        Transform originalParent = target.parent;

        // Match world transform
        target.position = transform.position;
        // target.position = transform.position;
        target.rotation = transform.rotation;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Vector3 frozenPos = target.position;
        Quaternion frozenRot = target.rotation;

        float timer = 2f;
        float moveSpeed = 0.013f; // units per second on the x-axis
        float elapsed = 0f;
        SkinCollisionDecal detector = target.GetComponent<SkinCollisionDecal>();

        while (true)
        {
             if (detector != null && detector.isCollidingWithSkin)
            {
                Debug.Log("yes seleve");
                break; // Stop moving if collision with "bone" occurred
            }
            float xOffset = elapsed * moveSpeed;
            target.position = new Vector3(frozenPos.x - xOffset, frozenPos.y, frozenPos.z);
            target.rotation = frozenRot;

            elapsed += Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        float returnTimer = 5f;
        float returnElapsed = 0f;


        while (returnTimer > 0f)
        {
            float xOffset = (1f - (returnElapsed / 2f)) * (elapsed * (moveSpeed*2)); // Linearly interpolate back
            target.position = new Vector3(frozenPos.x - xOffset, frozenPos.y, frozenPos.z);
            target.rotation = frozenRot;

            returnElapsed += Time.deltaTime;
            returnTimer -= Time.deltaTime;
            yield return null;
        }

        if (rb != null)
        {
            // rb.isKinematic = false;
        }
        gameObject.SetActive(false);

    }
}
