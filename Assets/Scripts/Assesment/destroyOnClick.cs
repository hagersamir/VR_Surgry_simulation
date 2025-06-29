using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Destroy : MonoBehaviour, IPointerClickHandler
{
    public GameObject ZoomBG; // Assign this in Inspector

    public void OnPointerClick(PointerEventData eventData)
    {
        ZoomBG.SetActive(false);
        Destroy(gameObject);

    }

}
