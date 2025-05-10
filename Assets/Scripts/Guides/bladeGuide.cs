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



    
}
