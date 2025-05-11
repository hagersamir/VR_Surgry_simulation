// using UnityEngine;
// using DecalSystem;

// public class Drill : MonoBehaviour
// {
//     public EventManager eventManager; // Assign in inspector

//     private void Start()
//     {
//         // transform.Find("hole")?.gameObject.AddComponent<BoxCollider>();
//     }
//     //when you hit the bone with the drill bit duplicate the hole paint
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Bone"))
//         {
//             DuplicateObject();
//         }
//     }

//     void DuplicateObject()
//     {
//         Transform hole = transform.Find("hole");
//         // Duplicate the hole (Instantiate creates a copy)
//         Transform newHole = Instantiate(hole, hole.position, hole.rotation);
//         // Optionally, set the duplicated object as a child of the same parent
//         //this 4 lines to make the duplicate have the same transformation of the orignal
//         newHole.SetParent(transform);
//         newHole.transform.SetPositionAndRotation(hole.transform.position, hole.transform.rotation);
//         newHole.localScale = new Vector3(hole.lossyScale.x / transform.lossyScale.x, hole.lossyScale.y / transform.lossyScale.y, hole.lossyScale.z / transform.lossyScale.z);
//         newHole.SetParent(null, true);


//         // Copy BoxCollider هنا انا كنت فاكرة ان البوكس كوليدر بيروح لوحده لما ارن بس طلع السليف بن الصرمة هو السبب
//         // BoxCollider originalCollider = hole.GetComponent<BoxCollider>();
//         // if (originalCollider != null)
//         // {
//         //     BoxCollider newCollider = newHole.gameObject.AddComponent<BoxCollider>();
//         //     newCollider.center = originalCollider.center;
//         //     newCollider.size = originalCollider.size;
//         //     newCollider.isTrigger = originalCollider.isTrigger;
//         // }
//         // else
//         // {
//         //     Debug.LogWarning("No BoxCollider found on the original hole!");
//         // }
//         // this is to call the build methond to display the texture
//         Decal decalComponent = newHole.GetComponent<Decal>();
//         if (decalComponent != null)
//         {
//             decalComponent.BuildAndSetDirty();
//         }
//         else
//         {
//             Debug.LogError("No Decal component found on the decalPrefab!");
//         }
//         Debug.Log("Duplicate created at: " + newHole.transform.position);


//         eventManager.OnEventProximalDrill_1();

//     }


// }

using UnityEngine;
using DecalSystem;

public class Drill : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation
    public EventManager eventManager;  // Assign in inspector
    public GameObject drillAssemply;   // The part that rotates
    public AudioClip rotationSound;    // Assign in Inspector

    private bool isCollidingWithBone = false;
    private bool hasDuplicated = false;
    private AudioSource audioSource;

    private bool proximal_1;// this is only to know which proximal locking  i am at
    private bool proximal_2;

    private GameObject boneParentOfScrew;

    private void Start()
    {
        // Add or get AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = rotationSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            drillAssemply.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (!hasDuplicated && isCollidingWithBone)
            {
                DuplicateObject();
                hasDuplicated = true;
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            hasDuplicated = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bone"))
        {
            isCollidingWithBone = true;
            boneParentOfScrew = other.gameObject;
        }
        if (other.CompareTag("ProximalLock1"))
        {
            proximal_1 = true;
        }
        if (other.CompareTag("ProximalLock2"))
        {
            proximal_2 = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bone"))
        {
            isCollidingWithBone = false;
        }
        if (other.CompareTag("ProximalLock1"))
        {
            proximal_1 = false;
        }
        if (other.CompareTag("ProximalLock2"))
        {
            proximal_2 = false;
        }
        if (eventManager.isDistalLocking)
        {
            eventManager.OnEventDistalDrilling();
        }
    }

    void DuplicateObject()
    {
        Transform hole = transform.Find("hole");
        if (hole == null)
        {
            Debug.LogWarning("No 'hole' child found!");
            return;
        }

        Transform newHole = Instantiate(hole, hole.position, hole.rotation);

        newHole.SetParent(transform);
        newHole.transform.SetPositionAndRotation(hole.transform.position, hole.transform.rotation);
        newHole.localScale = new Vector3(
            hole.lossyScale.x / transform.lossyScale.x,
            hole.lossyScale.y / transform.lossyScale.y,
            hole.lossyScale.z / transform.lossyScale.z
        );
        newHole.SetParent(null, true);

        Decal decalComponent = newHole.GetComponent<Decal>();
        if (decalComponent != null)
        {
            decalComponent.BuildAndSetDirty();
            newHole.transform.SetParent(boneParentOfScrew.transform);
        }
        else
        {
            Debug.LogError("No Decal component found on the decalPrefab!");
        }

        Debug.Log("Duplicate created at: " + newHole.transform.position);

        if (proximal_1)
        {
            eventManager?.OnEventProximalDrill_1();
        }
        if (proximal_2)
        {
            eventManager?.OnEventProximalDrill_2();
        }
    }
}
