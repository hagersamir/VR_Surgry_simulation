using UnityEngine;
using DecalSystem;
using JetBrains.Annotations;
public class SkinCollisionDecal : MonoBehaviour
{
    public EventManager eventManager; // Assign in inspector
    public bool isCollidingWithSkin = false;
    private Vector3 targetPosition = new Vector3(-0.0882999972f, 1.21169996f, -0.290100008f);
    public float moveSpeed = 2f; // Units per second

    private bool shouldMove = false;
    private bool move = false;
    private Vector3 targetPos;
    private float lastCutTime = -Mathf.Infinity;  // Initialized to allow the first cut

    private void OnTriggerEnter(Collider other)
    {
        // remove the box collider of the skin and add the mesh to make the mark on thre right place
        // BoxCollider boxCollider = other.GetComponent<BoxCollider>();
        // if (boxCollider != null)
        // {
        //     Destroy(boxCollider);
        //     MeshCollider meshCollider = other.gameObject.AddComponent<MeshCollider>();

        //     Debug.Log("BoxCollider removed and MeshCollider added to: " + other.gameObject.name);
        // }
        // Check if the collided object is tagged as "Skin"
        if (other.CompareTag("Skin"))
        {
            // Get the collision point in world space
            if (Time.time - lastCutTime < 1f)
                return;

            lastCutTime = Time.time;  // Update the time of the last cu

            Transform hole = transform.Find("mark");
            isCollidingWithSkin = true;
            Debug.Log("sleeve marking skin");

            // Duplicate the hole (Instantiate creates a copy)
            Transform decalInstance = Instantiate(hole, hole.position, hole.rotation);
            // Optionally, set the duplicated object as a child of the same parent
            //this 4 lines to make the duplicate have the same transformation of the orignal
            decalInstance.SetParent(transform);
            decalInstance.transform.SetPositionAndRotation(hole.transform.position, hole.transform.rotation);
            decalInstance.localScale = new Vector3(hole.lossyScale.x / transform.lossyScale.x, hole.lossyScale.y / transform.lossyScale.y, hole.lossyScale.z / transform.lossyScale.z);
            decalInstance.SetParent(null, true);
            decalInstance.SetParent(other.transform);


            // this is to call the build methond to display the texture
            Decal decalComponent = decalInstance.GetComponent<Decal>();
            if (decalComponent != null)
            {
                // Call the BuildAndSetDirty method
                decalComponent.BuildAndSetDirty();
                Debug.Log("Decal build triggered successfully!");
            }
            else
            {
                Debug.LogError("No Decal component found on the decalPrefab!");
            }


            // Remove the decal after a delay
            // Destroy(decalInstance, decalLifetime);
        }
        if (other.CompareTag("ProximalLock1"))
        {
            eventManager.OnEventProximalTrochar_1();

        }
        if (other.CompareTag("ProximalLock2"))
        {
            eventManager.OnEventProximalTrochar_2();

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            transform.GetComponent<MeshCollider>().enabled = false;

            targetPos = transform.position - new Vector3(0.01f, 0f, 0f);
            move = true;
        }

        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.05f * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                move = false;
            }
        }
    }


    // private void OnTriggerExit(Collider other)
    // {
    //     isCollidingWithSkin = false;
    //     Debug.Log("sleeve not touchung skin");
    // }
}