using System.Collections;
using UnityEngine;
using UnityEngine.Splines;


public class NailAndGuideWire : MonoBehaviour
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

            // Start the guide wire animation
            if (guideWireAnimator != null)
            {
                gameObject.transform.position = new Vector3(1000, 1000, 1000); // Move away
                gameObject.GetComponent<MeshRenderer>().enabled = false; // Hide visuals
                guideWire.SetActive(true);
                guideWireAnimator.Play();
            }

            if (nailAnimator != null)
            {
                StartCoroutine(StartNailAnimationAfterDelay(6f)); // Non-blocking delay
            }
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
