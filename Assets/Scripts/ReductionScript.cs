
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
    private bool rightHandGraspedBefore = false;
    private bool reductionCompleted = false;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(StartGrip);
        grabInteractable.selectExited.AddListener(EndGrip);

        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);
    }

    private void StartGrip(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            if (handData == null) return;

            handData.animator.enabled = false;

            if (handData.handModelType == HandData.HandModelType.Right)
            {
                isRightHandGrasping = true;
                rightHandGraspedBefore = true;
                StartCoroutine(SlideHand(handData, -slideDistance));
            }
            else if (handData.handModelType == HandData.HandModelType.Left)
            {
                isLeftHandGrasping = true;
                StartCoroutine(SlideHand(handData, slideDistance));

                if (isRightHandGrasping || rightHandGraspedBefore)
                {
                    StartCoroutine(AlignBrokenBone());
                }
            }

            if (isRightHandGrasping && isLeftHandGrasping)
            {
                StartCoroutine(AlignBrokenBone());
            }
        }
    }

    private void EndGrip(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
            if (handData == null) return;

            handData.animator.enabled = true;

            if (handData.handModelType == HandData.HandModelType.Right)
                isRightHandGrasping = false;
            else if (handData.handModelType == HandData.HandModelType.Left)
                isLeftHandGrasping = false;
        }
    }

    private IEnumerator SlideHand(HandData handData, float direction)
    {
        Vector3 startPos = handData.root.position;
        Vector3 targetPos = startPos + transform.up * direction;

        float timer = 0;
        while (timer < slideDuration)
        {
            handData.root.position = Vector3.Lerp(startPos, targetPos, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        timer = 0;
        while (timer < slideDuration)
        {
            handData.root.position = Vector3.Lerp(targetPos, startPos, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        handData.root.position = startPos;
    }

    private IEnumerator AlignBrokenBone()
    {
        if (reductionCompleted) yield break; 
        
        reductionCompleted = true;

        float alignDuration = 0.5f;
        float timer = 0;

        Vector3 initialPos = brokenBonePart.position;
        Vector3 finalPos = new Vector3(initialPos.x, 1.009f, initialPos.z);

        while (timer < alignDuration)
        {
            brokenBonePart.position = Vector3.Lerp(initialPos, finalPos, timer / alignDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        brokenBonePart.position = finalPos;

        yield return new WaitForSeconds(2f);
        
        if (stepManager != null)
        {
            stepManager.ReductionCompleted();
        }
    }
}