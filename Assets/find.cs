using UnityEngine;

public class FindObjectsByTag : MonoBehaviour
{
    public string targetTag = "Enemy"; // Set your tag in the Inspector

    void Start()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        if (taggedObjects.Length > 0)
        {
            Debug.Log($"Found {taggedObjects.Length} GameObjects with tag '{targetTag}':");
            foreach (GameObject obj in taggedObjects)
            {
                Debug.Log(obj.name, obj);
            }
        }
        else
        {
            Debug.Log($"No GameObjects found with tag '{targetTag}'.");
        }
    }
}
