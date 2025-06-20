using UnityEngine;

public class BoneSelectorUI : MonoBehaviour
{
    public GameObject popUpWindow;      
    public GameObject[] bonesToShow;    

    void Start()
    {
        popUpWindow.SetActive(true);

        // Hide all bones initially
        foreach (var bone in bonesToShow)
        {
            bone.SetActive(false);
        }
    }

    public void OnCaseSelected(int caseIndex)
    {
        popUpWindow.SetActive(false);

        // Show only selected bone
        for (int i = 0; i < bonesToShow.Length; i++)
        {
            bonesToShow[i].SetActive(i == caseIndex);
        }
    }
}
