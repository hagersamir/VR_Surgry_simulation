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
  public XRayExtraction xrayExtraction;

  private bool isRightHandGrasping = false;
  private bool isLeftHandGrasping = false;
  private bool rightWasGrasped = false;
  private bool leftWasGrasped = false;
  private bool isSliding = false;

  private XRGrabInteractable grabInteractable;
  private XRDirectInteractor rightInteractor;
  private XRDirectInteractor leftInteractor;
  private HandData rightOriginalHand;
  private HandData leftOriginalHand;

  private Vector3 rightHandStoredPosition;
  private Quaternion rightHandStoredRotation;
  private Vector3 leftHandStoredPosition;
  private Quaternion leftHandStoredRotation;

  private int alignmentStep = 0;
  private int totalSteps = 4;
  private Vector3 startPos;
  private Vector3 finalPos;
     //  fields for accuracy evaluation
    private Vector3 legPosition = new Vector3(0.324f, 0.993f, -0.781f);
    private float handExitThreshold = 0.25f;
    private bool alignmentEvaluationTriggered = false;

  void Start()
  {
    grabInteractable = GetComponent<XRGrabInteractable>();
    grabInteractable.selectEntered.AddListener(OnSelectEntered);
    grabInteractable.selectExited.AddListener(OnSelectExited);

    rightHandPose.gameObject.SetActive(false);
    leftHandPose.gameObject.SetActive(false);

    startPos = brokenBonePart.position;
    finalPos = new Vector3(0.187f, 1.0246f, -0.2841f) - new Vector3(0.297f, 0.014f, 0.143f);
  }

  void Update()
  {
    if (Keyboard.current != null)
    {
      if (Keyboard.current.gKey.wasPressedThisFrame)
      {
        if (isRightHandGrasping && !isSliding)
          StartCoroutine(SlideHand(rightHandPose.transform, -slideDistance, Vector3.up));
        else if (isLeftHandGrasping && !isSliding)
          StartCoroutine(SlideHand(leftHandPose.transform, slideDistance, Vector3.up));
      }

      if (Keyboard.current.uKey.wasPressedThisFrame)
      {
        RestoreVRHands();
      }
    }
    // Check if either hand moved away from leg position
        if (!alignmentEvaluationTriggered)
        {
            if (rightOriginalHand != null)
            {
                float dist = Vector3.Distance(rightOriginalHand.transform.position, legPosition);
                if (dist > handExitThreshold)
                {
                    alignmentEvaluationTriggered = true;
                    EvaluateAlignment();
                }
            }

            if (leftOriginalHand != null)
            {
                float dist = Vector3.Distance(leftOriginalHand.transform.position, legPosition);
                if (dist > handExitThreshold)
                {
                    alignmentEvaluationTriggered = true;
                    EvaluateAlignment();
                }
            }
        }
  }

  private void OnSelectEntered(SelectEnterEventArgs arg)
  {
    if (alignmentStep >= totalSteps) return;

    var interactor = arg.interactorObject as XRBaseInteractor;
    if (interactor == null) return;

    HandData handData = interactor.GetComponentInChildren<HandData>();
    if (handData == null) return;

    if (handData.handModelType == HandData.HandModelType.Right)
    {
      rightOriginalHand = handData;
      rightInteractor = interactor as XRDirectInteractor;
      isRightHandGrasping = true;
      rightWasGrasped = true;

      // Store position/rotation before hiding
      rightHandStoredPosition = handData.transform.position;
      rightHandStoredRotation = handData.transform.rotation;

      rightHandPose.gameObject.SetActive(true);
      StartCoroutine(SlideHand(rightHandPose.transform, -slideDistance, Vector3.up));
    }
    else if (handData.handModelType == HandData.HandModelType.Left)
    {
      leftOriginalHand = handData;
      leftInteractor = interactor as XRDirectInteractor;
      isLeftHandGrasping = true;
      leftWasGrasped = true;

      // Store position/rotation before hiding
      leftHandStoredPosition = handData.transform.position;
      leftHandStoredRotation = handData.transform.rotation;

      leftHandPose.gameObject.SetActive(true);
      StartCoroutine(SlideHand(leftHandPose.transform, slideDistance, Vector3.up));
    }

    handData.animator.enabled = false;
    handData.root.gameObject.SetActive(false);

    // Trigger alignment if second hand is grasped after first
    if ((handData.handModelType == HandData.HandModelType.Right && leftWasGrasped) ||
        (handData.handModelType == HandData.HandModelType.Left && rightWasGrasped))
    {
      StartCoroutine(AlignBrokenBoneStep());
    }
  }

  private void OnSelectExited(SelectExitEventArgs arg)
  {
    var interactor = arg.interactorObject as XRBaseInteractor;
    if (interactor == null) return;

    HandData handData = interactor.GetComponentInChildren<HandData>();
    if (handData == null) return;

    handData.animator.enabled = true;
    handData.root.gameObject.SetActive(true);

    if (handData.handModelType == HandData.HandModelType.Right)
    {
      isRightHandGrasping = false;
      rightHandPose.gameObject.SetActive(false);
      rightOriginalHand = null;
    }
    else
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

  private IEnumerator AlignBrokenBoneStep()
  {
    if (alignmentStep >= totalSteps) yield break;

    if (xrayExtraction != null && alignmentStep == 0)
    {
      xrayExtraction.SaveXrayImage("BEFORE REDUCTION");
    }

    alignmentStep++;

    float alignDuration = 0.4f;
    float timer = 0f;

    Vector3 initialPos = brokenBonePart.position;
    Vector3 targetPos = Vector3.Lerp(startPos, finalPos, (float)alignmentStep / totalSteps);

    while (timer < alignDuration)
    {
      brokenBonePart.position = Vector3.Lerp(initialPos, targetPos, timer / alignDuration);
      timer += Time.deltaTime;
      yield return null;
    }
    brokenBonePart.position = targetPos;
    yield return new WaitForSeconds(1f);

    RestoreVRHands();

    if (alignmentStep == totalSteps)
    {
      

      yield return new WaitForSeconds(2f);
      RestoreVRHands();

      if (stepManager != null && SceneManager.GetActiveScene().name == "TrainingScene")
      {
        stepManager.ReductionCompleted();
      }

      rightWasGrasped = false;
      leftWasGrasped = false;
    }
  }

  private void RestoreVRHands()
  {
    if (rightOriginalHand != null && rightInteractor != null)
    {
      rightOriginalHand.root.gameObject.SetActive(true);
      rightOriginalHand.animator.enabled = true;

      rightOriginalHand.transform.SetParent(null);
      rightOriginalHand.transform.position = rightHandStoredPosition;
      rightOriginalHand.transform.rotation = rightHandStoredRotation;

      rightOriginalHand.transform.SetParent(rightInteractor.transform);
      rightHandPose.gameObject.SetActive(false);
    }

    if (leftOriginalHand != null && leftInteractor != null)
    {
      leftOriginalHand.root.gameObject.SetActive(true);
      leftOriginalHand.animator.enabled = true;

      leftOriginalHand.transform.SetParent(null);
      leftOriginalHand.transform.position = leftHandStoredPosition;
      leftOriginalHand.transform.rotation = leftHandStoredRotation;

      leftOriginalHand.transform.SetParent(leftInteractor.transform);
      leftHandPose.gameObject.SetActive(false);
    }

    isRightHandGrasping = false;
    isLeftHandGrasping = false;
  }
    
     private void EvaluateAlignment()
    {
        if (xrayExtraction != null)
        xrayExtraction.SaveXrayImage("AFTER REDUCTION");
        float accuracy = GetAlignmentAccuracy(brokenBonePart.position);
        string grade = GetAccuracyGrade(accuracy);

        Debug.Log($"User moved hand away from leg area.");
        Debug.Log($"Final bone position: {brokenBonePart.position}");
        Debug.Log($"Alignment Accuracy: {(accuracy * 100f):F2}%");
        Debug.Log($"Grade: {grade}");
    }

    private float GetAlignmentAccuracy(Vector3 currentPos)
    {
        float totalDistance = Vector3.Distance(startPos, finalPos);
        float currentDistance = Vector3.Distance(currentPos, finalPos);

        if (totalDistance == 0f) return 1f;

        float accuracy = 1f - (currentDistance / totalDistance);
        return Mathf.Clamp01(accuracy);
    }

    private string GetAccuracyGrade(float accuracy)
    {
        if (accuracy >= 0.95f) return "Excellent";
        if (accuracy >= 0.80f) return "Good";
        if (accuracy >= 0.60f) return "Fair";
        return "Needs Improvement";
    }

}
