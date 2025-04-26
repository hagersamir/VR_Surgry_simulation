using UnityEngine;
using DecalSystem;
using UnityEditor;

public class ChildMoveWithParent : MonoBehaviour
{
    public Transform child;  // Assign the child object in the Inspector
    private float lastParentY;

    private Vector3 childLocalPosition;
    private Quaternion childLocalRotation;
    private Vector3 childLocalScale;

    public GameObject cutTexturePrefab;

    void Start()
    {

        //save the orignal transformation of the texture to assign it to the pref after
        // Find "SkinCut" child and store its relative transform
        Transform skinCut = transform.Find("SkinCut");
        if (skinCut != null)
        {
            child = skinCut;
            childLocalPosition = child.localPosition;
            childLocalRotation = child.localRotation;
            childLocalScale = child.localScale;
        }

        lastParentY = transform.position.y;
    }

    void Update()
    {
        if (child != null && child.parent == transform) // Ensure the child is still parented
        {
            float parentDeltaY = transform.position.y - lastParentY;

            if (parentDeltaY < 0) // Only move child when parent moves downward
            {
                // Calculate the movement direction in parent's local space (0, -1, -1)
                Vector3 localDirection = new Vector3(-1, -1, 0).normalized;

                // Convert the local direction to world space
                Vector3 worldDirection = transform.TransformDirection(localDirection);

                // Apply the full movement amount in this direction
                child.position += worldDirection * -parentDeltaY;

                // Update Decal (if exists)
                Decal decalComponent = child.GetComponent<Decal>();
                if (decalComponent != null)
                {
                    decalComponent.BuildAndSetDirty();
                }
            }

            lastParentY = transform.position.y;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        child.gameObject.SetActive(true);
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
