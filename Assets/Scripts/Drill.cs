using UnityEngine;
using DecalSystem;

public class Drill : MonoBehaviour
{
public EventManager eventManager; // Assign in inspector

    private void Start()
    {
        // transform.Find("hole")?.gameObject.AddComponent<BoxCollider>();
    }
    //when you hit the bone with the drill bit duplicate the hole paint
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bone"))
        {
            DuplicateObject();
        }
    }

    void DuplicateObject()
    {
        Transform hole = transform.Find("hole");
        // Duplicate the hole (Instantiate creates a copy)
        Transform newHole = Instantiate(hole, hole.position, hole.rotation);
        // Optionally, set the duplicated object as a child of the same parent
        //this 4 lines to make the duplicate have the same transformation of the orignal
        newHole.SetParent(transform);
        newHole.transform.SetPositionAndRotation(hole.transform.position, hole.transform.rotation);
        newHole.localScale = new Vector3(hole.lossyScale.x / transform.lossyScale.x, hole.lossyScale.y / transform.lossyScale.y, hole.lossyScale.z / transform.lossyScale.z);
        newHole.SetParent(null, true);


        // Copy BoxCollider هنا انا كنت فاكرة ان البوكس كوليدر بيروح لوحده لما ارن بس طلع السليف بن الصرمة هو السبب
        // BoxCollider originalCollider = hole.GetComponent<BoxCollider>();
        // if (originalCollider != null)
        // {
        //     BoxCollider newCollider = newHole.gameObject.AddComponent<BoxCollider>();
        //     newCollider.center = originalCollider.center;
        //     newCollider.size = originalCollider.size;
        //     newCollider.isTrigger = originalCollider.isTrigger;
        // }
        // else
        // {
        //     Debug.LogWarning("No BoxCollider found on the original hole!");
        // }
        // this is to call the build methond to display the texture
        Decal decalComponent = newHole.GetComponent<Decal>();
        if (decalComponent != null)
        {
            decalComponent.BuildAndSetDirty();
        }
        else
        {
            Debug.LogError("No Decal component found on the decalPrefab!");
        }
        Debug.Log("Duplicate created at: " + newHole.transform.position);


eventManager.OnEventProximalDrill_1();

    }


}