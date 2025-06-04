using UnityEngine;
using System.Collections;

public class drill : MonoBehaviour
{
    public float distance = 0.04f; // Distance to move before resetting
    public float speed = 0.01f; // Speed of movement

    private Vector3 startPosition;
    private bool isMoving = true;  // Flag to control movement
    private bool isDistal = false;




    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
        if (gameObject.name == "drill guide (2)")
        {
            isDistal = true;
        }
    }

    void Update()
    {
        if (!isMoving) return; // Skip movement if flagged

        // Move the object in its local left direction
        transform.position += transform.up * speed * Time.deltaTime;

        // Check if it moved beyond the distance
        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            // Reset position
            transform.position = startPosition;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == other.tag && other.gameObject != null)
        {
            Debug.Log($"{gameObject.name}collided with {other.name}");
            GetComponent<MeshRenderer>().enabled = false;
            // GetComponent<MeshCollider>().enabled = false;

            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = false;
            }
            StartCoroutine(ApplyAndFreeze(other.transform));
            isMoving = false; // Stop movement
        }
    }

    IEnumerator ApplyAndFreeze(Transform target)
    {
        Transform originalParent = target.parent;

        target.position = transform.position;
        target.rotation = transform.rotation;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Vector3 frozenPos = target.position;
        Quaternion frozenRot = target.rotation;

        float timer = 5f;
        float moveSpeed = 0.025f;
        float elapsed = 0f;

        // Get the child with tag "bit" and its collision detector
        Transform bitChild = null;
        foreach (Transform child in target.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("bit"))
            {
                bitChild = child;
                break;
            }
        }
        Drill detector = bitChild?.GetComponent<Drill>();

        while (true)
        {
            if (detector != null && detector.isCollidingWithBone)
            {
                Debug.Log("yes");
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

        gameObject.SetActive(false);
    }

}