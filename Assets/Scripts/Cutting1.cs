using UnityEngine;
using DecalSystem;
using UnityEditor;

public class smallCut : MonoBehaviour
{
    public Transform child;  // Assign the child object in the Inspector
    private float lastParentY;

    private Vector3 childLocalPosition;
    private Quaternion childLocalRotation;
    private Vector3 childLocalScale;

    public GameObject cutTexturePrefab;

public EventManager eventManager; // Assign in inspector

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

            child.gameObject.SetActive(true);
            eventManager.OnEventProximalCut_1();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Skin"))
        {
            if (transform.childCount == 0)
            {


                // Instantiate new object with the same relative transform
                GameObject instance = Instantiate(cutTexturePrefab, transform);
                instance.transform.localPosition = childLocalPosition;
                instance.transform.localRotation = childLocalRotation;
                instance.transform.localScale = childLocalScale;
                child = instance.transform;
                child.gameObject.SetActive(false);
            }
        }
    }
}
