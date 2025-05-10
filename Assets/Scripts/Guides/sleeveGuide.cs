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

   
}