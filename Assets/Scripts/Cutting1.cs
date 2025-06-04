// using UnityEngine;
// using DecalSystem;
// using UnityEditor;

// public class smallCut : MonoBehaviour
// {
//     public Transform child;  // Assign the child object in the Inspector
//     private float lastParentY;

//     private Vector3 childLocalPosition;
//     private Quaternion childLocalRotation;
//     private Vector3 childLocalScale;

//     public GameObject cutTexturePrefab;

//     public EventManager eventManager; // Assign in inspector
//     private bool proxmial_1 = false;
//     public bool madeCut = false;

//     void Start()
//     {

//         //save the orignal transformation of the texture to assign it to the pref after
//         // Find "SkinCut" child and store its relative transform
//         Transform skinCut = transform.Find("SkinCutForDrill");
//         if (skinCut != null)
//         {
//             child = skinCut;
//             childLocalPosition = child.localPosition;
//             childLocalRotation = child.localRotation;
//             childLocalScale = child.localScale;
//         }

//         lastParentY = transform.position.y;
//     }


//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject.CompareTag("Skin"))
//         {

//             child.gameObject.SetActive(true);
//             // eventManager.OnEventProximalCut_1();
//         }
//         if (other.CompareTag("ProximalLock1"))
//         {
//             proxmial_1 = true;
//             eventManager.OnEventProximalCut_1();

//         }
//         else if (other.CompareTag("ProximalLock2") && proxmial_1)// this is because if you accedently hit the other cutPlace it dose not shoe the guide of the other drill(only call the event if the first proximal is done)
//         {
//             eventManager.OnEventProximalCut_2();

//         }
//         if (eventManager.isDistalLocking)
//         {
//             eventManager.OnEventDistalCut();
//         }
//     }
//     private void OnTriggerExit(Collider other)
//     {
//         if (other.gameObject.CompareTag("Skin"))
//         {
//             // if (transform.Find("SkinCutForDrill"))
//             // {
//             madeCut = true;

//                 // Instantiate new object with the same relative transform
//                 GameObject instance = Instantiate(cutTexturePrefab, transform);
//                 instance.transform.localPosition = childLocalPosition;
//                 instance.transform.localRotation = childLocalRotation;
//                 instance.transform.localScale = childLocalScale;
//                 child = instance.transform;
//                 child.gameObject.SetActive(false);
//             // }
//         }
//     }
// }





using UnityEngine;
using DecalSystem;
using UnityEditor;
using Unity.VisualScripting;

public class smallCut : MonoBehaviour
{
    public Transform child;  // Assign the child object in the Inspector
    private float lastParentY;

    private Vector3 childLocalPosition;
    private Quaternion childLocalRotation;
    private Vector3 childLocalScale;

    public GameObject cutTexturePrefab;

    public EventManager eventManager; // Assign in inspector
    private bool proxmial_1 = false;
    private bool proxmial_2 = false;
    public bool madeCut = false;


    void Start()
    {

        //save the orignal transformation of the texture to assign it to the pref after
        // Find "SkinCut" child and store its relative transform
        Transform skinCut = transform.Find("SkinCutForDrill");
        if (skinCut != null)
        {
            child = skinCut;
            childLocalPosition = child.localPosition;
            childLocalRotation = child.localRotation;
            childLocalScale = child.localScale;
        }

        lastParentY = transform.position.y;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Skin"))
        {

            madeCut = true;

            child.gameObject.SetActive(true);
            // eventManager.OnEventProximalCut_1();
        }
        if (other.CompareTag("ProximalLock1"))
        {
            proxmial_1 = true;
            eventManager.OnEventProximalCut_1();

        }
        else if (other.CompareTag("ProximalLock2") && proxmial_1)
        {
            proxmial_2 = true;
            eventManager.OnEventProximalCut_2();

        }
        // if (eventManager.isDistalLocking && proxmial_2)
        // {
        //     eventManager.OnEventDistalCut();
        // }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Skin"))
        {
            // if (transform.Find("SkinCutForDrill"))
            // {


            // Instantiate new object with the same relative transform
            GameObject instance = Instantiate(cutTexturePrefab, transform);
            instance.transform.localPosition = childLocalPosition;
            instance.transform.localRotation = childLocalRotation;
            instance.transform.localScale = childLocalScale;
            child = instance.transform;
            child.gameObject.SetActive(false);
            // }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            eventManager.OnEventDistalCut();
        }
    }
}
