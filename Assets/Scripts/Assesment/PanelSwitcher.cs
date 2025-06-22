using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    [Header("Step Panels")]
    public GameObject ReductionPanel;
    public GameObject CuttingPanel;
    public GameObject InsertionPanel;
    public GameObject LockingPanel;

    [Header("Step Buttons")]
    public Button ReductionBtn;
    public Button CuttingBtn;
    public Button InsertionBtn;
    public Button LockingBtn;

    void Start()
    {
        // Assign button click handlers
        ReductionBtn.onClick.AddListener(() => ShowOnly(ReductionPanel));
        CuttingBtn.onClick.AddListener(() => ShowOnly(CuttingPanel));
        InsertionBtn.onClick.AddListener(() => ShowOnly(InsertionPanel));
        LockingBtn.onClick.AddListener(() => ShowOnly(LockingPanel));

        // Show first panel by default
        ShowOnly(ReductionPanel);
    }

    void ShowOnly(GameObject activePanel)
    {
        ReductionPanel.SetActive(activePanel == ReductionPanel);
        CuttingPanel.SetActive(activePanel == CuttingPanel);
        InsertionPanel.SetActive(activePanel == InsertionPanel);
        LockingPanel.SetActive(activePanel == LockingPanel);
    }
}
