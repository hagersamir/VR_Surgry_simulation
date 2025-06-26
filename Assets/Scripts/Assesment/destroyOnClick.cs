using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Destroy : MonoBehaviour, IPointerClickHandler
{
    public GameObject ZoomBG; // Assign this in Inspector
    public Canvas parentCanvas;        // Optional, assign if needed for hierarchy

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
        ZoomBG.SetActive(false);

    }

}
