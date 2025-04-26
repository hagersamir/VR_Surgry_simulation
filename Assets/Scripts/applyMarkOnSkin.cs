using UnityEngine;
using DecalSystem;
public class SkinCollisionDecal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // remove the box collider of the skin and add the mesh to make the mark on thre right place
        BoxCollider boxCollider = other.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
            MeshCollider meshCollider = other.gameObject.AddComponent<MeshCollider>();

            Debug.Log("BoxCollider removed and MeshCollider added to: " + other.gameObject.name);
        }
        // Check if the collided object is tagged as "Skin"
        if (other.CompareTag("Skin"))
        {
            // Get the collision point in world space

            Transform hole = transform.Find("mark");
            // Duplicate the hole (Instantiate creates a copy)
            Transform decalInstance = Instantiate(hole, hole.position, hole.rotation);
            // Optionally, set the duplicated object as a child of the same parent
            //this 4 lines to make the duplicate have the same transformation of the orignal
            decalInstance.SetParent(transform);
            decalInstance.transform.SetPositionAndRotation(hole.transform.position, hole.transform.rotation);
            decalInstance.localScale = new Vector3(hole.lossyScale.x / transform.lossyScale.x, hole.lossyScale.y / transform.lossyScale.y, hole.lossyScale.z / transform.lossyScale.z);
            decalInstance.SetParent(null, true);

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
    }
}