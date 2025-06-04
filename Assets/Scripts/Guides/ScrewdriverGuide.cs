using UnityEngine;
using System.Collections;

public class screw : MonoBehaviour
{
    public float distance = 0.04f; // Distance to move before resetting
    public float speed = 0.01f; // Speed of movement

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isMoving = true;  // Flag to control movement

    private bool isDistal = false;

    void Start()
    {
        // Store the initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;
        if (gameObject.name == "Screwdriver guide (2)")
        {
            isDistal = true;
        }
    }

    void Update()
    {
        if (!isMoving) return; // Skip movement if flagged

        // Move the object in its local forward direction
        transform.position += -transform.forward * speed * Time.deltaTime;

        // Rotate around its own local X axis
        transform.Rotate(Vector3.forward * 360f * Time.deltaTime);

        // Check if it moved beyond the distance
        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            // Reset position and rotation
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == other.tag & other.gameObject != null)
        {
            isMoving = false; // Stop movement

            Debug.Log($"{gameObject.name}collided with {other.name}");
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = false;
            }
            GetComponent<MeshCollider>().enabled = false;

            StartCoroutine(ApplyAndFreeze(other.transform));
            // Destroy(gameObject);
            // GetComponent<MeshCollider>().enabled = false;
            // StartCoroutine(TemporarilyDisableCollider(5f));

            // StartCoroutine(DisableColliderAfterDelay(0.5f)); // Small delay

            // foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            // {
            //     mr.enabled = false;
            // }
            // Rigidbody rb = GetComponent<Rigidbody>();
            // if (rb != null)
            // {
            //     rb.velocity = Vector3.zero;
            //     rb.angularVelocity = Vector3.zero;
            //     rb.constraints = RigidbodyConstraints.FreezeAll;
            // }

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
        float moveSpeed = 0.04f; // units per second on the x-axis
        float elapsed = 0f;



        Transform screwChild = null;
        foreach (Transform child in target.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Screw"))
            {
                screwChild = child;
                break;
            }
        }
        ScrewAttachment detector = screwChild?.GetComponent<ScrewAttachment>();

        // ScrewAttachment detector = target.GetComponent<ScrewAttachment>();

        while (true)
        {
            if (detector != null && detector.ScrewPlaced)
            {
                Debug.Log("yes screw");
                break; // Stop moving if collision with "bone" occurred
            }
            float xOffset = elapsed * moveSpeed;
            if (isDistal)
            {

                target.position = new Vector3(frozenPos.x, frozenPos.y - xOffset, frozenPos.z);
            }
            else
            {

                target.position = new Vector3(frozenPos.x - xOffset, frozenPos.y, frozenPos.z);
            }
            target.rotation = frozenRot;

            elapsed += Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }

        if (rb != null)
        {
            // rb.isKinematic = false;
        }
        gameObject.SetActive(false);
        detector.ScrewPlaced = false;
    }


}