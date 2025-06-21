using System.Collections;
using UnityEngine;
using TMPro;

public class NurseController : MonoBehaviour
{
    public Transform[] waypoints; // Points the nurse moves to
    public GameObject checklist; // Checklist object in hand
    public Animator animator; // Animator for controlling animations
    public float moveSpeed = 2.0f; // Movement speed
    public float stopThreshold = 0.2f; // Distance to stop moving
    public Transform handPosition; // Hand position for checklist
    public Transform parent;
    public TextMeshPro textMesh;
    public GameObject skipButton; // Reference to the skip button in the scene


    private bool rotates = false;
    private bool skipRequested = false;

    private Vector3 checklistInitialPosition;
    private Quaternion checklistInitialRotation;
    private Vector3 nurseInitialPosition;
    private Quaternion nurseInitialRotation;

    private Quaternion checklistRot = Quaternion.Euler(0, 273.589905f, 0);
    private Quaternion toolsRot = Quaternion.Euler(0.342127562f, 175.262482f, 0.533579767f);
    private Vector3 firstTarget = new Vector3(-3.07299995f, 0.0659999996f, -0.131999999f); // First table
    private Vector3 secondTarget = new Vector3(2.25f, 0, -2.6400001f); // Second table

  public void StartNurseActions()
{
    // yield return new WaitForSeconds(2f);
    StartCoroutine(PerformActions());
}

  void Start()
    {
        // StartCoroutine(PerformActions());
        checklistInitialPosition = checklist.transform.position;
        checklistInitialRotation = checklist.transform.rotation;
        nurseInitialPosition = transform.position;
        nurseInitialRotation = transform.rotation;
        textMesh.gameObject.SetActive(false); // Hide text at start
    }

    void LateUpdate()
    {
        textMesh.transform.LookAt(Camera.main.transform);
        textMesh.transform.rotation = Quaternion.Euler(
            textMesh.transform.rotation.eulerAngles.x,
            textMesh.transform.rotation.eulerAngles.y + 180f,
            textMesh.transform.rotation.eulerAngles.z
        );
    }

    public void SkipPreOperations()
{
    skipRequested = true;

    if (skipButton != null)
    {
        Destroy(skipButton); // Remove the button from the scene
    }
}


    IEnumerator PerformActions()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isIdle", false);
        if (skipRequested) { yield return SkipToEnd(); yield break; }

        animator.SetBool("isWalking", true);
        yield return MoveToPosition(firstTarget, checklistRot);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
        if (skipRequested) { yield return SkipToEnd(); yield break; }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isPickingUp", true);
        yield return new WaitForSeconds(2.0f);
        checklist.transform.position = handPosition.position;
        checklist.transform.SetParent(parent);
        animator.SetBool("isPickingUp", false);
        animator.SetBool("isIdle", true);
        if (skipRequested) { yield return SkipToEnd(); yield break; }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalkingWithChecklist", true);
        rotates = true;
        yield return MoveToPosition(secondTarget, toolsRot);
        animator.SetBool("isWalkingWithChecklist", false);
        yield return new WaitForSeconds(0.05f);
        animator.SetBool("isIdle", true);
        if (skipRequested) { yield return SkipToEnd(); yield break; }

        yield return new WaitForSeconds(0.7f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isLooking", true);
        yield return new WaitForSeconds(5f);
        animator.SetBool("isLooking", false);
        animator.SetBool("isChecking", true);
        yield return new WaitForSeconds(2f);
        yield return DisplayText();
        animator.SetBool("isChecking", false);
        animator.SetBool("isCompleteChecked", true);
        yield return new WaitForSeconds(0.2f);

        animator.SetBool("isIdle", true);
        animator.SetBool("isCompleteChecked", false);
        if (skipRequested) { yield return SkipToEnd(); yield break; }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        rotates = true;
        yield return MoveToPosition(firstTarget, checklistRot);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isThrow", true);
        yield return new WaitForSeconds(2.0f);
        checklist.transform.SetParent(null);
        checklist.transform.position = checklistInitialPosition;
        checklist.transform.rotation = checklistInitialRotation;
        animator.SetBool("isThrow", false);
        animator.SetBool("isIdle", true);

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        rotates = true;
        yield return MoveToPosition(nurseInitialPosition, nurseInitialRotation);
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(0.05f);
        animator.SetBool("isIdle", true);
    }

    IEnumerator MoveToPosition(Vector3 target, Quaternion rotate)
    {
        transform.LookAt(target);
        while (Vector3.Distance(transform.position, target) > stopThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
        if (rotates)
        {
            transform.rotation = rotate;
        }
    }

    IEnumerator DisplayText()
    {
        textMesh.gameObject.SetActive(true);
        SetDynamicText("All tools are checked well.");
        yield return new WaitForSeconds(2.0f);
        SetDynamicText("The diameter of the tibia of this patient is .. cm.");
        yield return new WaitForSeconds(2.0f);
        SetDynamicText("The length of the tibia is .. cm.");
        yield return new WaitForSeconds(2.0f);
        textMesh.gameObject.SetActive(false);
    }

    void SetDynamicText(string message)
    {
        textMesh.text = message;
        RectTransform rectTransform = textMesh.GetComponent<RectTransform>();
        float textWidth = textMesh.preferredWidth + 1.0f;
        float textHeight = textMesh.preferredHeight + 0.5f;
        rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
    }

    IEnumerator SkipToEnd()
    {
        // Drop checklist if picked
        checklist.transform.SetParent(null);
        checklist.transform.position = checklistInitialPosition;
        checklist.transform.rotation = checklistInitialRotation;

        // Return to original nurse position
        animator.SetBool("isWalking", true);
        rotates = true;
        yield return MoveToPosition(nurseInitialPosition, nurseInitialRotation);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
    }
}
