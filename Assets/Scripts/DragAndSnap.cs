using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class DragAndSnap : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 1f;
    private float distancePercentage = 0f;
    private float splineLength;
    private bool isMoving = true;

    public float rotationAngle = 180f;
    public float rotationDuration = 0.5f;
    public float pauseDuration = 1f;
    private bool hasFinished = false;

    public SplineAnimate awlSplineAnimator; // Awl's animator
    public GameObject awl; // Awl object

    public SplineAnimate guideWireAnimator; // Guide wire's animator
    public GameObject guideWire; // Guide wire object

    public SplineAnimate nailAnimator; // Guide wire's animator
    public GameObject nail; // Guide wire object

    private void Start()
    {
        awl.SetActive(false);
        guideWire.SetActive(false);
        nail.SetActive(false);
        splineLength = spline.CalculateLength();
        StartCoroutine(MoveAndRotateAlongSpline());
    }

    private IEnumerator MoveAndRotateAlongSpline()
    {
        while (distancePercentage < 1f)
        {
            if (isMoving)
            {
                // Move along the spline
                distancePercentage += speed * Time.deltaTime / splineLength;
                Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
                transform.position = currentPosition;

                // Pause movement and rotation every interval
                isMoving = false; // Stop movement temporarily
                StartCoroutine(RotateYIncrementally());
            }
            yield return null;
        }
        hasFinished = true; // Mark as finished

        // Start the Awl animation
        if (awlSplineAnimator != null)
        {
            yield return new WaitForSeconds(1);
            awl.SetActive(true);
            awlSplineAnimator.Play();

            // Wait 5 seconds before reversing
            yield return new WaitForSeconds(5);
            StartCoroutine(ReverseAwlAnimation());
        }
    }

    private IEnumerator ReverseAwlAnimation()
    {
        if (awlSplineAnimator != null)
        {
            float duration = awlSplineAnimator.Duration; // Get the total animation time
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // Move the Awl backwards
                awlSplineAnimator.NormalizedTime = 1f - (elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure it's set to the start
            awlSplineAnimator.NormalizedTime = 0f;
            awl.SetActive(false); // Hide the Awl

            // yield return new WaitForSeconds(1); // Small delay before starting guide wire animation

            // Start the guide wire animation
            if (guideWireAnimator != null)
            {
                // Unparent Guide Wire so it remains visible
                // guideWire.transform.SetParent(null, true); // Keep its world position

                // Hide the parent object (this script's GameObject)
                // gameObject.SetActive(false);
                gameObject.transform.position = new Vector3(1000, 1000, 1000); // Move away
                gameObject.GetComponent<MeshRenderer>().enabled = false; // Hide visuals
                guideWire.SetActive(true);
                guideWireAnimator.Play();
            }
            // yield return new WaitForSeconds(4); // Wait before Nail animation

            if (nailAnimator != null)
            {
                // Debug.Log("Activating Nail"); // Debugging line
                // nail.SetActive(true); // Ensure Nail is active
                // // yield return new WaitForSeconds(0.1f); // Small delay to ensure activation
                // Debug.Log("Playing Nail Animation"); // Debugging line
                // nailAnimator.Play();
                StartCoroutine(StartNailAnimationAfterDelay(6f)); // Non-blocking delay
            }
            // Debug.Log("Didn't Activate Nail"); // Debugging line
        }
    }

private IEnumerator StartNailAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait before starting
        nail.SetActive(true); // Ensure Nail is active
        nailAnimator.Play();

        // Wait for Nail animation to finish
        yield return new WaitForSeconds(nailAnimator.Duration);

        // Now, reverse and hide the Guide Wire
        StartCoroutine(ReverseGuideWireAnimation());
    }

private IEnumerator ReverseGuideWireAnimation()
    {
        if (guideWireAnimator != null)
        {
            float duration = guideWireAnimator.Duration;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                guideWireAnimator.NormalizedTime = 1f - (elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure it's set to the start
            guideWireAnimator.NormalizedTime = 0f;
            guideWire.SetActive(false); // Hide the Guide Wire
        }
    }


    private IEnumerator RotateYIncrementally()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, rotationAngle, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure exact final rotation

        // Wait before continuing movement
        yield return new WaitForSeconds(pauseDuration);

        // Resume movement
        isMoving = true;
    }
}
// ---------------------------------------------Works-------------------------------------------------
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Splines;

// public class DragAndSnap : MonoBehaviour
// {
//     public SplineContainer spline;
//     public float speed = 1f;
//     private float distancePercentage = 0f;
//     private float splineLength;
//     private bool isMoving = true;

//     public float rotationAngle = 180f; // Angle per rotation step
//     public float rotationDuration = 0.5f; // Time to rotate
//     public float pauseDuration = 1f; // Pause between movements
//     private bool hasFinished = false;

//     public SplineAnimate secondSplineAnimator; // Reference to the second object's animator
//     public GameObject awl; // Reference to the second object

//     private void Start()
//     {
//         awl.SetActive(false);
//         splineLength = spline.CalculateLength();
//         StartCoroutine(MoveAndRotateAlongSpline());
//     }

//     private IEnumerator MoveAndRotateAlongSpline()
//     {
//         while (distancePercentage < 1f)
//         {
//             if (isMoving)
//             {
//                 // Move along the spline
//                 distancePercentage += speed * Time.deltaTime / splineLength;
//                 Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
//                 transform.position = currentPosition;

//                 // Face forward along the spline
//                 Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
//                 Vector3 direction = nextPosition - currentPosition;
//                 // transform.rotation = Quaternion.LookRotation(direction, transform.up);

//                 // Pause movement and rotation every interval
//                 isMoving = false; // Stop movement temporarily
//                 StartCoroutine(RotateYIncrementally());
//             }
//             yield return null;
//         }
//         hasFinished = true; // Mark as finished

//         // Start the second object's spline animation
//         if (secondSplineAnimator != null)
//         {
//             awl.SetActive(true);
//             secondSplineAnimator.Play(); 
//         }
//     }

//     private IEnumerator RotateYIncrementally()
//     {
//         Quaternion startRotation = transform.rotation;
//         Quaternion targetRotation = startRotation * Quaternion.Euler(0f, rotationAngle, 0f);
//         float elapsedTime = 0f;

//         while (elapsedTime < rotationDuration)
//         {
//             transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);
//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         transform.rotation = targetRotation; // Ensure exact final rotation

//         // Wait before continuing movement
//         yield return new WaitForSeconds(pauseDuration);

//         // Resume movement
//         isMoving = true;
//     }
// }

// ---------------------------------------------------------------------------------------------------------------------------------------------------------------
// using UnityEngine;
// using UnityEngine.EventSystems;

// public class DragAndSnap : MonoBehaviour
// {
//     private Vector3 originalPosition;
//     private Quaternion originalRotation;
//     private bool isInsideSnapZone = false;

//     // Define the fixed snap position and rotation
//     private static readonly Vector3 SNAP_POSITION = new Vector3(1.92f, 0.525f, -7.4094f);  // Adjust as needed
//     private static readonly Quaternion SNAP_ROTATION = Quaternion.Euler(0, -90, 321.716f); // Adjust as needed

//     void Start()
//     {
//         originalPosition = transform.position;
//         originalRotation = transform.rotation;
//     }

//     //public void OnBeginDrag(PointerEventData eventData)
//     //{
//     //    isInsideSnapZone = false;
//     //}

//     //public void OnDrag(PointerEventData eventData)
//     //{
//     //    // Convert mouse position to world position
//     //    Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
//     //    transform.position = new Vector3(newPosition.x, newPosition.y, originalPosition.z); // Maintain Z position
//     //}

//     //public void OnEndDrag(PointerEventData eventData)
//     //{
//     //    if (isInsideSnapZone)
//     //    {
//     //        transform.position = SNAP_POSITION; // Snap to fixed position
//     //        transform.rotation = SNAP_ROTATION; // Snap to fixed rotation
//     //    }
//     //    else
//     //    {
//     //        transform.position = originalPosition; // Reset position if not snapped
//     //        transform.rotation = originalRotation; // Reset rotation if not snapped
//     //    }
//     //}

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("SnapZone"))
//         {
//             isInsideSnapZone = true;
//             transform.position = SNAP_POSITION; // Snap to fixed position
//             transform.rotation = SNAP_ROTATION; // Snap to fixed rotation

//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("SnapZone"))
//         {
//             isInsideSnapZone = false;
//         }
//     }
// }

// using UnityEngine;
// using System.Collections;
// using UnityEngine.Splines;

// public class DragAndSnap : MonoBehaviour
// {
//     private Vector3 originalPosition;
//     private Quaternion originalRotation;
//     private bool isInsideSnapZone = false;
//     private SplineAnimate splineAnimate; // Reference to Spline Animate component

//     // Define the fixed snap position and rotation
//     private static readonly Vector3 SNAP_POSITION = new Vector3(1.92f, 0.525f, -7.4094f);
//     private static readonly Quaternion SNAP_ROTATION = Quaternion.Euler(0, -90, 321.716f);

//     public float rotationAngle = 130f; // Degrees to rotate each interval
//     public float rotationDuration = 0.5f; // Duration of each rotation in seconds
//     public float pauseDuration = 1f; // Pause duration between rotations
//     public int totalRotations = 6; // Total number of rotations

//     void Start()
//     {
//         originalPosition = transform.position;
//         originalRotation = transform.rotation;

//         // Get reference to SplineAnimate component (Make sure it's attached)
//         splineAnimate = GetComponent<SplineAnimate>();
//         // Disable the SplineAnimate component
//         splineAnimate.enabled = false;

        // StartCoroutine(RotateRoutine());

//         // if (splineAnimate != null)
//         // {
//         //     splineAnimate.Pause(); // Ensure the animation is paused at the start
//         //     splineAnimate.Restart(false); // Restart animation from the beginning and play
//         // }
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("SnapZone"))
//         {
//             isInsideSnapZone = true;
//             transform.position = SNAP_POSITION; // Snap to fixed position
//             transform.rotation = SNAP_ROTATION; // Snap to fixed rotation

//             // Start the Spline animation
//             if (splineAnimate != null)
//             {
//                 // splineAnimate.Restart(false); // Restart animation from the beginning and play
//                 // Disable the SplineAnimate component
//                 splineAnimate.enabled = true;
//                 splineAnimate.Play();
//             }
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag("SnapZone"))
//         {
//             isInsideSnapZone = false;
//             splineAnimate.Pause();
//         }
//     }

//     private IEnumerator RotateRoutine()
//     {
//         for (int i = 0; i < totalRotations; i++)
//         {
//             Quaternion startRotation = transform.rotation;
//             Quaternion endRotation = startRotation * Quaternion.Euler(0f, rotationAngle, 0f);
//             float elapsedTime = 0f;

//             while (elapsedTime < rotationDuration)
//             {
//                 transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);
//                 elapsedTime += Time.deltaTime;
//                 yield return null;
//             }

//             transform.rotation = endRotation; // Ensure final rotation is set
//             splineAnimate.Pause();
//             yield return new WaitForSeconds(pauseDuration);
//         }
//     }
// }



