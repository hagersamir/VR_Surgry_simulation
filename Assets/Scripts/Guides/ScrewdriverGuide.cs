using UnityEngine;
using System.Collections;

public class screw : MonoBehaviour
{
    public float distance = 0.04f; // Distance to move before resetting
    public float speed = 0.01f; // Speed of movement

    private Vector3 startPosition;
    private Quaternion startRotation;


    void Start()
    {
        // Store the initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
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


    
}