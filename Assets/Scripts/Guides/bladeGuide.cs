// using UnityEngine;
// using System.Collections;

// public class blade : MonoBehaviour
// {
//     public float forwardDistance = 0.005f; // Distance to move forward
//     public float downwardDistance = 0.01f; // Distance to move downward
//     public float speed = 0.01f; // Speed of movement

//     private Vector3 startPosition;
//     private Quaternion startRotation;
//     private bool isMoving = true;

//     void Start()
//     {
//         // Store the initial position and rotation
//         startPosition = transform.position;
//         startRotation = transform.rotation;

//         // Start the animation coroutine
//         StartCoroutine(AnimateBlade());
//     }

//     IEnumerator AnimateBlade()
//     {


//         while (true)
//         {
//             if (isMoving)
//             {
//                 // Step 1: Move forward for the specified distance
//                 float traveled = 0f;
//                 while (traveled < forwardDistance)
//                 {
//                     transform.position += transform.right * speed * Time.deltaTime;
//                     traveled += speed * Time.deltaTime;
//                     yield return null;
//                 }

//                 // // Step 2: Move downward for the specified distance
//                 // traveled = 0f;
//                 // Vector3 downwardDirection = transform.up;
//                 // while (traveled < downwardDistance)
//                 // {
//                 //     transform.position += downwardDirection * speed * 0.5f * Time.deltaTime;
//                 //     traveled += speed * Time.deltaTime;
//                 //     yield return null;
//                 // }

//                 // // Step 3: Move backward for the specified distance
//                 // traveled = 0f;
//                 // while (traveled < forwardDistance)
//                 // {
//                 //     transform.position += -transform.right * speed * Time.deltaTime;
//                 //     traveled += speed * Time.deltaTime;
//                 //     yield return null;
//                 // }

//                 // Reset position and rotation
//                 transform.position = startPosition;
//                 transform.rotation = startRotation;

//             }
//             yield return null; // Optional: short pause if needed
//         }


//     }
//     private void OnTriggerEnter(Collider other)
//     {
//         if (gameObject.tag == other.tag & other.gameObject != null)
//         {
//             Debug.Log($"{gameObject.name}collided with {other.name}");
//             GetComponent<MeshRenderer>().enabled = false;

//             StartCoroutine(ApplyAndFreeze(other.transform));
//             isMoving = false;

//         }

//     }
//     IEnumerator ApplyAndFreeze(Transform target)
//     {
//         // Cache the parent (in case it's parented and affected by it)
//         Transform originalParent = target.parent;

//         // Match world transform
//         target.position = transform.position;
//         // target.position = transform.position;
//         target.rotation = transform.rotation;

//         Rigidbody rb = target.GetComponent<Rigidbody>();
//         if (rb != null)
//         {
//             rb.isKinematic = true;
//         }

//         Vector3 frozenPos = target.position;
//         Quaternion frozenRot = target.rotation;

//         float timer = 2f;
//         float moveSpeed = 0.009f; // units per second on the x-axis
//         float elapsed = 0f;

//         smallCut detector = target.GetComponent<smallCut>();

//         while (true)
//         {
//             if (detector != null && detector.madeCut)
//             {
//                 Debug.Log("yes samll cut");
//                 break; // Stop moving if collision with "bone" occurred
//             }
//             float xOffset = elapsed * moveSpeed;
//             target.position = new Vector3(frozenPos.x - xOffset, frozenPos.y, frozenPos.z);
//             target.rotation = frozenRot;

//             elapsed += Time.deltaTime;
//             timer -= Time.deltaTime;
//             yield return null;
//         }

//         // float returnTimer = 2f;
//         // float returnElapsed = 0f;

//         // while (returnTimer > 0f)
//         // {
//         //     float xOffset = (1f - (returnElapsed / 2f)) * (elapsed * (moveSpeed * 2)); // Linearly interpolate back
//         //     target.position = new Vector3(frozenPos.x - xOffset, frozenPos.y, frozenPos.z);
//         //     target.rotation = frozenRot;

//         //     returnElapsed += Time.deltaTime;
//         //     returnTimer -= Time.deltaTime;
//         //     yield return null;
//         // }
//         // if (rb != null)
//         // {
//         //     // rb.isKinematic = false;
//         // }
//         gameObject.SetActive(false);

//     }







// }
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class blade : MonoBehaviour
{
    public float forwardDistance = 0.005f; // Distance to move forward
    public float downwardDistance = 0.01f; // Distance to move downward
    public float speed = 0.01f; // Speed of movement

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isMoving = true;
    private bool isDistal = false;

    void Start()
    {
        // Store the initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Start the animation coroutine
        StartCoroutine(AnimateBlade());

        if (gameObject.name == "plade guide (2)")
        {
            isDistal = true;
        }
    }

    IEnumerator AnimateBlade()
    {


        while (true)
        {
            if (isMoving)
            {
                // Step 1: Move forward for the specified distance
                float traveled = 0f;
                while (traveled < forwardDistance)
                {
                    transform.position += transform.right * speed * Time.deltaTime;
                    traveled += speed * Time.deltaTime;
                    yield return null;
                }

                // // Step 2: Move downward for the specified distance
                // traveled = 0f;
                // Vector3 downwardDirection = transform.up;
                // while (traveled < downwardDistance)
                // {
                //     transform.position += downwardDirection * speed * 0.5f * Time.deltaTime;
                //     traveled += speed * Time.deltaTime;
                //     yield return null;
                // }

                // // Step 3: Move backward for the specified distance
                // traveled = 0f;
                // while (traveled < forwardDistance)
                // {
                //     transform.position += -transform.right * speed * Time.deltaTime;
                //     traveled += speed * Time.deltaTime;
                //     yield return null;
                // }

                // Reset position and rotation
                transform.position = startPosition;
                transform.rotation = startRotation;

            }
            yield return null; // Optional: short pause if needed
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == other.tag & other.gameObject != null)
        {
            Debug.Log($"{gameObject.name}collided with {other.name}");
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(ApplyAndFreeze(other.transform));
            isMoving = false;


        }

    }
    IEnumerator ApplyAndFreeze(Transform target)
    {
        // Cache the parent (in case it's parented and affected by it)
        Transform originalParent = target.parent;

        // Match world transform
        target.position = transform.position;
        // target.position = transform.position;
        target.rotation = transform.rotation;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Vector3 frozenPos = target.position;
        Quaternion frozenRot = target.rotation;

        float timer = 7f;
        float moveSpeed = 0.009f; // units per second on the x-axis
        float elapsed = 0f;
        smallCut detector = target.GetComponent<smallCut>();

        while (true)
        {
            if (detector != null && detector.madeCut)
            {
                Debug.Log("yes plade");
                break; // Stop moving if collision with "bone" occurred
            }
            float xOffset = elapsed * moveSpeed;
            if (isDistal)
            {

                target.position = new Vector3(frozenPos.x, frozenPos.y - xOffset, frozenPos.z);
            }
            else
            {

                // target.position = new Vector3(frozenPos.x - xOffset, frozenPos.y, frozenPos.z);
                target.position = frozenPos + target.right * xOffset;

            }
            target.rotation = frozenRot;

            elapsed += Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }

        // float returnTimer = 2f;
        // float returnElapsed = 0f;

        // while (returnTimer > 0f)
        // {
        //     float xOffset = (1f - (returnElapsed / 2f)) * (elapsed * (moveSpeed * 2)); // Linearly interpolate back
        //     target.position = new Vector3(frozenPos.x - xOffset, frozenPos.y, frozenPos.z);
        //     target.rotation = frozenRot;

        //     returnElapsed += Time.deltaTime;
        //     returnTimer -= Time.deltaTime;
        //     yield return null;
        // }
        // if (rb != null)
        // {
        //     // rb.isKinematic = false;
        // }
        gameObject.SetActive(false);
        detector.madeCut = false;

    }







}



