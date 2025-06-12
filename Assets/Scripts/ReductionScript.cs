
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class ReductionScript : MonoBehaviour
{
    public float slideDistance = 0.1f;
    public float slideDuration = 0.3f;
    public HandData rightHandPose;
    public HandData leftHandPose;
    public Transform brokenBonePart;
    public StepManager stepManager;

    private bool isRightHandGrasping = false;
    private bool isLeftHandGrasping = false;
    private bool reductionCompleted = false;
    private bool isSliding = false;

    private XRGrabInteractable grabInteractable;
    private XRDirectInteractor rightInteractor;
    private XRDirectInteractor leftInteractor;
    private HandData rightOriginalHand;
    private HandData leftOriginalHand;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);
    }

    void Update()
    {
      // Prevent any hand movement if reduction is completed
    if (reductionCompleted) return;
        // Check for keyboard G press (grip button simulation)
        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            if (isRightHandGrasping && !isSliding)
            {
                StartCoroutine(SlideHand(rightHandPose.transform, -slideDistance, Vector3.up));
            }
            if (isLeftHandGrasping && !isSliding)
            {
                StartCoroutine(SlideHand(leftHandPose.transform, slideDistance, Vector3.up));
            }
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs arg)
    {
      if (reductionCompleted) return; // Prevent further interaction after completion
        var interactor = arg.interactorObject as XRBaseInteractor;
        if (interactor == null) return;

        HandData handData = interactor.GetComponentInChildren<HandData>();
        if (handData == null) return;

        // Store original hand references
        if (handData.handModelType == HandData.HandModelType.Right)
        {
            rightOriginalHand = handData;
            rightInteractor = interactor as XRDirectInteractor;
        }
        else if (handData.handModelType == HandData.HandModelType.Left)
        {
            leftOriginalHand = handData;
            leftInteractor = interactor as XRDirectInteractor;
        }

        handData.animator.enabled = false;
        handData.root.gameObject.SetActive(false); // Hide original VR hand

        if (handData.handModelType == HandData.HandModelType.Right)
        {
            isRightHandGrasping = true;
            rightHandPose.gameObject.SetActive(true);
            StartCoroutine(SlideHand(rightHandPose.transform, -slideDistance, Vector3.up));
        }
        else if (handData.handModelType == HandData.HandModelType.Left)
        {
            isLeftHandGrasping = true;
            leftHandPose.gameObject.SetActive(true);
            StartCoroutine(SlideHand(leftHandPose.transform, slideDistance, Vector3.up));
        }

        // Check for both hands grasping
        if (isRightHandGrasping && isLeftHandGrasping && !reductionCompleted)
        {
            StartCoroutine(AlignBrokenBone());
        }
    }

    private void OnSelectExited(SelectExitEventArgs arg)
    {
        var interactor = arg.interactorObject as XRBaseInteractor;
        if (interactor == null) return;

        HandData handData = interactor.GetComponentInChildren<HandData>();
        if (handData == null) return;

        handData.animator.enabled = true;
        handData.root.gameObject.SetActive(true); // Show original VR hand

        if (handData.handModelType == HandData.HandModelType.Right)
        {
            isRightHandGrasping = false;
            rightHandPose.gameObject.SetActive(false);
            rightOriginalHand = null;
        }
        else if (handData.handModelType == HandData.HandModelType.Left)
        {
            isLeftHandGrasping = false;
            leftHandPose.gameObject.SetActive(false);
            leftOriginalHand = null;
        }
    }

    private IEnumerator SlideHand(Transform handTransform, float distance, Vector3 direction)
    {
        isSliding = true;
        Vector3 slideDirection = direction.normalized;
        Vector3 startPos = handTransform.position;
        Vector3 targetPos = startPos + slideDirection * distance;

        float timer = 0f;
        while (timer < slideDuration)
        {
            handTransform.position = Vector3.Lerp(startPos, targetPos, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        handTransform.position = targetPos;

        yield return new WaitForSeconds(0.2f);

        timer = 0f;
        while (timer < slideDuration)
        {
            handTransform.position = Vector3.Lerp(targetPos, startPos, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        handTransform.position = startPos;
        isSliding = false;
    }

    private IEnumerator AlignBrokenBone()
    {
        reductionCompleted = true;

        float alignDuration = 0.5f;
        float timer = 0f;

        Vector3 initialPos = brokenBonePart.position;
        // Vector3 finalPos = new Vector3(0.1788f, 1.0248f, -0.2833f) + new Vector3(-0.297f, -0.014f, -0.143f);
        Vector3 finalPos = new Vector3(0.187000006f, 1.02460003f, -0.284099996f)- new Vector3(0.296999991f, 0.014000058f,0.143000007f);

        while (timer < alignDuration)
        {
            brokenBonePart.position = Vector3.Lerp(initialPos, finalPos, timer / alignDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        brokenBonePart.position = finalPos;

        yield return new WaitForSeconds(2f);

      
    // Restore both hands in one step
    RestoreVRHands();
        if (stepManager != null && SceneManager.GetActiveScene().name == "TrainingScene")
{
    stepManager.ReductionCompleted();
}

    }
    private void RestoreVRHands()
{
    // Restore Right Hand
    if (rightOriginalHand != null && rightInteractor != null)
    {
        rightOriginalHand.root.gameObject.SetActive(true);
        rightOriginalHand.animator.enabled = true;
        
        // Reset parent & transform to follow the controller
        rightOriginalHand.transform.SetParent(rightInteractor.transform);
        rightOriginalHand.transform.localPosition = Vector3.zero;
        rightOriginalHand.transform.localRotation = Quaternion.identity;
        
        rightHandPose.gameObject.SetActive(false);
    }
    else
    {
        Debug.LogError("Right hand or interactor missing!");
    }

    // Restore Left Hand
    if (leftOriginalHand != null && leftInteractor != null)
    {
        leftOriginalHand.root.gameObject.SetActive(true);
        leftOriginalHand.animator.enabled = true;
        
        // Reset parent & transform to follow the controller
        leftOriginalHand.transform.SetParent(leftInteractor.transform);
        leftOriginalHand.transform.localPosition = Vector3.zero;
        leftOriginalHand.transform.localRotation = Quaternion.identity;
        
        leftHandPose.gameObject.SetActive(false);
    }
    else
    {
        Debug.LogError("Left hand or interactor missing!");
    }
}
}