// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UIElements;


// public class NurseController : MonoBehaviour
// {
//     public Transform[] waypoints; // Points the nurse moves to
//     public GameObject checklist; // Checklist object in hand
//     public Animator animator; // Animator for controlling animations
//     public float moveSpeed = 2.0f; // Movement speed
//     public float stopThreshold = 0.2f; // Distance to stop moving
//     public Transform handPosition; // Hand position for checklist
//     public Transform parent;
//     public TextMeshPro textMesh; 
//     private bool rotates = false;
//     private Vector3 checklistInitialPosition;
//     private Quaternion checklistInitialRotation;
//     private Vector3 nurseInitialPosition;
//     private Quaternion nurseInitialRotation;
//     private Quaternion checklistRot = Quaternion.Euler(0,273.589905f,0);
//     private Quaternion toolsRot = Quaternion.Euler(0.342127562f, 175.262482f, 0.533579767f);
//     private Vector3 firstTarget = new Vector3(-3.07299995f, 0.0659999996f, -0.131999999f); // First table
//     private Vector3 secondTarget = new Vector3(2.25f,0,-2.6400001f); // Second table
    

//     void Start()
//     {
//         StartCoroutine(PerformActions());
//         // Store the initial position and rotation of the checklist
//         checklistInitialPosition = checklist.transform.position;
//         checklistInitialRotation = checklist.transform.rotation;
//         // Store the initial position and rotation of the nurse
//         nurseInitialPosition = transform.position;
//         nurseInitialRotation = transform.rotation;
//         textMesh.gameObject.SetActive(false); // Hide text at start
//     }

//     void LateUpdate()
//     {
//     // Ensure the text always faces the camera
//     textMesh.transform.LookAt(Camera.main.transform);
//     // Correct the mirroring effect by flipping the text
//     textMesh.transform.rotation = Quaternion.Euler(
//         textMesh.transform.rotation.eulerAngles.x,
//         textMesh.transform.rotation.eulerAngles.y + 180f, // Flip by 180 degrees
//         textMesh.transform.rotation.eulerAngles.z
//     );
//     }

//     IEnumerator PerformActions()
//     {
//         // Step 1: Idle for 0.5 seconds
//         animator.SetBool("isIdle", true);
//         yield return new WaitForSeconds(0.5f);
//         animator.SetBool("isIdle", false);

//         // Step 2: Walk to the first table
//         animator.SetBool("isWalking", true);
//         yield return MoveToPosition(firstTarget,checklistRot);
//         animator.SetBool("isWalking", false);
//         animator.SetBool("isIdle", true);

//         // Step 3: Pick up checklist
//         yield return new WaitForSeconds(0.5f); // Short delay before picking up
//         animator.SetBool("isIdle", false);
//         animator.SetBool("isPickingUp", true);
//         yield return new WaitForSeconds(2.0f); 
//         // Attach checklist to hand
//         checklist.transform.position = handPosition.position;
//         checklist.transform.SetParent(parent);
//         animator.SetBool("isPickingUp", false);
//         animator.SetBool("isIdle", true);

//         // Step 4: Walk to the second table
//         yield return new WaitForSeconds(0.5f);
//         animator.SetBool("isIdle", false);
//         animator.SetBool("isWalkingWithChecklist", true);
//         rotates=true;
//         yield return MoveToPosition(secondTarget ,toolsRot); 
//         animator.SetBool("isWalkingWithChecklist", false);
//         yield return new WaitForSeconds(0.05f);
//         animator.SetBool("isIdle", true);
        
//         // Step 5: Checking tools ... and Displaying text
//         yield return new WaitForSeconds(0.7f);
//         animator.SetBool("isIdle", false);
//         animator.SetBool("isLooking", true);
//         yield return new WaitForSeconds(5f);
//         animator.SetBool("isLooking", false);
//         animator.SetBool("isChecking", true);
//         yield return new WaitForSeconds(2f);
//         yield return DisplayText();
//         animator.SetBool("isChecking", false);
//         animator.SetBool("isCompleteChecked", true);
//         yield return new WaitForSeconds(0.2f);

//         // Step 6: Displaying text
//         animator.SetBool("isIdle", true);
//         animator.SetBool("isCompleteChecked", false);

//         // Step 7: Return back to checklist table 
//         yield return new WaitForSeconds(0.5f);
//         animator.SetBool("isIdle", false);
//         animator.SetBool("isWalking", true);
//         rotates=true;
//         yield return MoveToPosition(firstTarget,checklistRot);
//         animator.SetBool("isWalking", false);
//         animator.SetBool("isIdle", true); 

//         // Step 8: Drop down the checklist
//         yield return new WaitForSeconds(0.5f); // Short delay before drop down
//         animator.SetBool("isIdle", false);
//         animator.SetBool("isThrow", true);
//         yield return new WaitForSeconds(2.0f); 
//         // Restore checklist to its initial position
//         checklist.transform.SetParent(null); // Detach from the hand
//         checklist.transform.position = checklistInitialPosition;
//         checklist.transform.rotation = checklistInitialRotation;
//         animator.SetBool("isThrow", false);
//         animator.SetBool("isIdle", true);

//         // Step 9: return back to intial position 
//         yield return new WaitForSeconds(0.5f);
//         animator.SetBool("isIdle", false);
//         animator.SetBool("isWalking", true);
//         rotates=true;
//         yield return MoveToPosition(nurseInitialPosition,nurseInitialRotation); 
//         animator.SetBool("isWalking", false);
//         yield return new WaitForSeconds(0.05f);
//         animator.SetBool("isIdle", true);
// }

//     IEnumerator MoveToPosition(Vector3 target,Quaternion rotate)
//     {
//         transform.LookAt(target);
//         while (Vector3.Distance(transform.position, target) > stopThreshold)
//         {
//             transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
//             yield return null; // Wait for the next frame
//         }
//         transform.position = target;
//         if (rotates)
//         {
//         transform.rotation = rotate;
//         }
//     }

//     IEnumerator DisplayText()
//     {
//         textMesh.gameObject.SetActive(true);
//         SetDynamicText("All tools are checked well.");
//         yield return new WaitForSeconds(2.0f);
//         SetDynamicText("The diameter of the tibia of this patient is .. cm.");
//         yield return new WaitForSeconds(2.0f);
//         SetDynamicText("The length of the tibia is .. cm.");
//         yield return new WaitForSeconds(2.0f);
//         textMesh.gameObject.SetActive(false); // Hide text after speaking
//     }

//     void SetDynamicText(string message)
//     {
//         textMesh.text = message;
//         // Adjust text box size 
//         RectTransform rectTransform = textMesh.GetComponent<RectTransform>();
//         float textWidth = textMesh.preferredWidth + 1.0f;  //  padding
//         float textHeight = textMesh.preferredHeight + 0.5f; 
//         rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
//     }
// }
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

    void Start()
    {
        StartCoroutine(PerformActions());
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
