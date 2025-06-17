using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.Controls;
public class MatchPositionOnTrigger : MonoBehaviour
{
    public bool isMatching = false;
    public bool isFree = true;
    private Coroutine matchCoroutine;
    public GameObject tool;
    public float xRoatiation;
    public float YRoatiation;
    public float zRoatiation;
    public string alighTag;
    public string freeTag;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name}collided with {other.name}");

        if (other.CompareTag(alighTag))
        {
            if (isFree)
            {

                // transform.position = other.transform.position;
                // transform.rotation = other.transform.rotation;
                isMatching = true;


                StartCoroutine(ApplyAndFreeze(other.transform));
            }
            isFree = false;



        }
        if (other.CompareTag(freeTag))
        {
            isFree = true;
            Debug.Log("fuckienFree");
            isMatching = false;

        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag(gameObject.tag))
    //     {
    //         isMatching = false;


    //     }
    //     // if (other.CompareTag("small cut"))
    //     // {
    //     //     isFree = false;
    //     // }
    // }

    IEnumerator ApplyAndFreeze(Transform target)
    {
        // if (isMatching)
        // {

        //     transform.position = target.position;
        //     transform.rotation = target.rotation;
        // }

        Rigidbody rb = tool.transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }


        // Quaternion frozenRot = tool.transform.localRotation;

        while (isMatching)
        {
            // if (isFree)
            // {
            //     Debug.Log("break");
            //     break;
            // }
            Vector3 currentRotation = tool.transform.eulerAngles;
            Vector3 childWorldPos = target.transform.position;

            // Get current Y of the tool
            float currentx = tool.transform.position.x;
            float currenty = tool.transform.position.y;
            float currentz = tool.transform.position.z;

            // Set the tool's position using X and Z from child, Y remains unchanged
            tool.transform.position = new Vector3(currentx, childWorldPos.y, childWorldPos.z);

            // Vector3 frozenLocalPos = tool.transform.localPosition;
            tool.transform.eulerAngles = new Vector3(xRoatiation, YRoatiation, zRoatiation);
            // tool.set

            // tool.transform.localPosition = new Vector3(
            //     tool.transform.localPosition.x,  // Allow X to change
            //     frozenLocalPos.y,        // Freeze Y
            //     frozenLocalPos.z         // Freeze Z
            // );

            // transform.localRotation = frozenRot;

            yield return null;
        }
    }

}
