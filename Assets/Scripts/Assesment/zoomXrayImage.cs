using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DuplicateOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject targetTransform; // Assign this in Inspector
    public GameObject zoomBG; // Assign this in Inspector
    public Canvas parentCanvas;        // Optional, assign if needed for hierarchy

    public void OnPointerClick(PointerEventData eventData)
    {
        // Duplicate the current image
        zoomBG.SetActive(true);
        GameObject duplicate = Instantiate(gameObject);
        Destroy(duplicate.GetComponent<DuplicateOnClick>());

        // Add DestroyOnClick script to the duplicate
        duplicate.AddComponent<Destroy>();
        duplicate.GetComponent<Destroy>().ZoomBG = zoomBG;

        // Make sure the duplicate is parented correctly
        if (parentCanvas != null)
        {
            duplicate.transform.SetParent(parentCanvas.transform, false);
        }
        else
        {
            duplicate.transform.SetParent(transform.parent, false);
        }

        // Set position and scale to match the target GameObject
        if (targetTransform != null)
        {
            RectTransform dupRect = duplicate.GetComponent<RectTransform>();
            RectTransform targetRect = targetTransform.GetComponent<RectTransform>();

            if (dupRect != null && targetRect != null)
            {
                dupRect.position = targetRect.position;
                dupRect.localScale = targetRect.localScale;
            }
        }
    }
}
