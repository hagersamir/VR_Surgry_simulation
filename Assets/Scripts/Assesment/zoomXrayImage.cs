using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DuplicateOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject targetTransform;
    public GameObject zoomBG;
    public Canvas parentCanvas;

    private bool hasClicked = false; // 👈 prevent multiple triggers

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasClicked) return;     // 👈 skip if already clicked
        hasClicked = true;          // 👈 lock the click

        zoomBG.SetActive(true);

        GameObject duplicate = Instantiate(gameObject);
        Destroy(duplicate.GetComponent<DuplicateOnClick>());

        duplicate.AddComponent<Destroy>().ZoomBG = zoomBG;

        if (parentCanvas != null)
        {
            duplicate.transform.SetParent(parentCanvas.transform, false);
        }
        else
        {
            duplicate.transform.SetParent(transform.parent, false);
        }

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

        // Optional: reset lock after short delay (in case you want to allow future clicks)
        Invoke(nameof(ResetClick), 0.3f); // adjust delay as needed
    }

    private void ResetClick()
    {
        hasClicked = false;
    }
}
