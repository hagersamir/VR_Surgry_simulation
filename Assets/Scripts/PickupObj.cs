using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    public GameObject pickUpCecklist;
    public GameObject handCarryObj;
    public Transform parent;
    private Vector3 checklistPosition;
    private Quaternion checklistRotation;
    // Start is called before the first frame update
    void Start()
    {
        if (pickUpCecklist != null)
        {
          checklistPosition = pickUpCecklist.transform.position;
          checklistRotation = pickUpCecklist.transform.rotation;
        }
    }

    public void OnPickUp(){
      if (pickUpCecklist != null)
      {
        Vector3 distance = pickUpCecklist.transform.position - handCarryObj.transform.position ;
        float magnitude = distance.magnitude;
        if (magnitude <= 0.2f)
        {
          pickUpCecklist.transform.SetParent(parent);
          pickUpCecklist.transform.localPosition = Vector3.zero;
        }
      }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
