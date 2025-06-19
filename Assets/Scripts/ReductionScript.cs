using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class ReductionScript : MonoBehaviour
{
    public float slideDistance = 0.1f;
    public float slideDuration = 0.3f;
    public HandData rightHandPose;
    public HandData leftHandPose;
    public Transform brokenBonePart;
    public StepManager stepManager;
    public XRayExtraction xrayExtraction;
    public AudioSource alarmAudioSource;
    public AudioClip alarmClip;
    public TextMeshProUGUI taskText;
    public GameObject taskPanel;

    private bool isRightHandGrasping = false;
    private bool isLeftHandGrasping = false;
    private bool rightWasGrasped = false;
    private bool leftWasGrasped = false;
    private bool isSliding = false;
    private bool reductionCompleted = false;
    private bool alignmentEvaluationTriggered = false;

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

    private Vector3 legPosition = new Vector3(0.324f, 0.993f, -0.781f);
    private float handExitThreshold = 1.5f;

    private bool IsTrainingMode => SceneManager.GetActiveScene().name == "TrainingScene";

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);
        taskPanel.SetActive(false);

        startPos = brokenBonePart.position;
        finalPos = new Vector3(0.187f, 1.0246f, -0.2841f) - new Vector3(0.297f, 0.014f, 0.143f);
    }

    void Update()
    {
        if (reductionCompleted) return;

        if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            if (isRightHandGrasping && !isSliding)
                StartCoroutine(SlideHand(rightHandPose.transform, -slideDistance, Vector3.up));
            if (isLeftHandGrasping && !isSliding)
                StartCoroutine(SlideHand(leftHandPose.transform, slideDistance, Vector3.up));
        }

        if (!alignmentEvaluationTriggered)
        {
            if (rightOriginalHand && Vector3.Distance(rightOriginalHand.transform.position, legPosition) > handExitThreshold)
            {
                alignmentEvaluationTriggered = true;
                EvaluateAlignment();
            }

            if (leftOriginalHand && Vector3.Distance(leftOriginalHand.transform.position, legPosition) > handExitThreshold)
            {
                alignmentEvaluationTriggered = true;
                EvaluateAlignment();
            }
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs arg)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
        if (reductionCompleted) return;

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

            rightHandStoredPosition = handData.transform.position;
            rightHandStoredRotation = handData.transform.rotation;
            rightHandPose.gameObject.SetActive(true);
            StartCoroutine(SlideHand(rightHandPose.transform, -slideDistance, Vector3.up));
        }
        if (handData.handModelType == HandData.HandModelType.Left)
        {
            leftOriginalHand = handData;
            leftInteractor = interactor as XRDirectInteractor;
            isLeftHandGrasping = true;
            leftWasGrasped = true;

            leftHandStoredPosition = handData.transform.position;
            leftHandStoredRotation = handData.transform.rotation;
            leftHandPose.gameObject.SetActive(true);
            StartCoroutine(SlideHand(leftHandPose.transform, slideDistance, Vector3.up));
        }

        handData.animator.enabled = false;
        handData.root.gameObject.SetActive(false);

        // Trigger reduction logic
        if (IsTrainingMode)
        {
            if (isRightHandGrasping && isLeftHandGrasping && !reductionCompleted)
                StartCoroutine(OneStepReduction());
        }
        else
        {
            if ((handData.handModelType == HandData.HandModelType.Right && leftWasGrasped) ||
                (handData.handModelType == HandData.HandModelType.Left && rightWasGrasped))
            {
                StartCoroutine(AlignBrokenBoneStep());
            }

            if (alignmentStep >= totalSteps)
            {
                taskPanel.SetActive(true);
                taskText.text = "<b><color=red>WARNING:</color></b> Bone already aligned!";
                if (alarmAudioSource && alarmClip)
                {
                    alarmAudioSource.clip = alarmClip;
                    alarmAudioSource.Play();
                    StartCoroutine(StopAlarmAfterSeconds(3f));
                }
            }
        }
    }

    private IEnumerator OneStepReduction()
    {
        reductionCompleted = true;

        Vector3 initialPos = brokenBonePart.position;
        Vector3 finalPos = new Vector3(0.187000006f, 1.02460003f, -0.284099996f) - new Vector3(0.296999991f, 0.014000058f, 0.143000007f);
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            brokenBonePart.position = Vector3.Lerp(initialPos, finalPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        brokenBonePart.position = finalPos;
        yield return new WaitForSeconds(2f);
        RestoreVRHands();

        if (stepManager != null)
            stepManager.ReductionCompleted();
    }

    private IEnumerator AlignBrokenBoneStep()
    {
        if (alignmentStep >= totalSteps) yield break;

        if (alignmentStep == 0 && xrayExtraction != null)
            xrayExtraction.SaveXrayImage("BEFORE REDUCTION");

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
            reductionCompleted = true;
            yield return new WaitForSeconds(2f);
            RestoreVRHands();

            if (stepManager != null && !IsTrainingMode)
                stepManager.ReductionCompleted();

            rightWasGrasped = false;
            leftWasGrasped = false;
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
        Vector3 slideDir = direction.normalized;
        Vector3 start = handTransform.position;
        Vector3 end = start + slideDir * distance;

        float timer = 0f;
        while (timer < slideDuration)
        {
            handTransform.position = Vector3.Lerp(start, end, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        handTransform.position = end;
        yield return new WaitForSeconds(0.2f);

        timer = 0f;
        while (timer < slideDuration)
        {
            handTransform.position = Vector3.Lerp(end, start, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        handTransform.position = start;
        isSliding = false;
    }

    private void RestoreVRHands()
    {
        if (rightOriginalHand != null && rightInteractor != null)
        {
            rightOriginalHand.root.gameObject.SetActive(true);
            rightOriginalHand.animator.enabled = true;
            rightOriginalHand.transform.SetParent(rightInteractor.transform);
            rightOriginalHand.transform.localPosition = Vector3.zero;
            rightOriginalHand.transform.localRotation = Quaternion.identity;
            rightHandPose.gameObject.SetActive(false);
        }

        if (leftOriginalHand != null && leftInteractor != null)
        {
            leftOriginalHand.root.gameObject.SetActive(true);
            leftOriginalHand.animator.enabled = true;
            leftOriginalHand.transform.SetParent(leftInteractor.transform);
            leftOriginalHand.transform.localPosition = Vector3.zero;
            leftOriginalHand.transform.localRotation = Quaternion.identity;
            leftHandPose.gameObject.SetActive(false);
        }

        isRightHandGrasping = false;
        isLeftHandGrasping = false;
    }

    private void EvaluateAlignment()
    {
        reductionCompleted = true;
        if (xrayExtraction != null)
            xrayExtraction.SaveXrayImage("AFTER REDUCTION");

        float accuracy = GetAlignmentAccuracy(brokenBonePart.position);
        Debug.Log($"User moved hand away. Accuracy: {(accuracy * 100f):F2}%");
        RestoreVRHands();
    }

    private float GetAlignmentAccuracy(Vector3 currentPos)
    {
        float totalDist = Vector3.Distance(startPos, finalPos);
        float currentDist = Vector3.Distance(currentPos, finalPos);
        return totalDist == 0f ? 1f : Mathf.Clamp01(1f - (currentDist / totalDist));
    }

    private IEnumerator StopAlarmAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (alarmAudioSource.isPlaying)
            alarmAudioSource.Stop();
        taskPanel.SetActive(false);
    }

    public float reductionErrorLength => Vector3.Distance(brokenBonePart.position, finalPos);
    public float AlignmentAccuracy => GetAlignmentAccuracy(brokenBonePart.position);
}
