
using System.Collections;
using UnityEngine;
using TMPro;

public class StepManager : MonoBehaviour
{
    public TextMeshProUGUI cornerText;
    public TextMeshProUGUI taskText;
    public GameObject taskPanel;
    private int currentStep = 1;
    private bool isTransitioning = false;

    private string[] stepNames =
    {
        "Step 1: Reduction",
        "Step 2: Entry Site",
        "Step 3: Nail Insertion",
        "Step 4: Locking and Closure"
    };

    private string[][] stepNotes =
    {
        new string[] { "TASK : Go and Grasp ankle with both hand", "TASK : Apply traction gently like you see" },
        new string[] { "TASK : Locate the correct entry site", "TASK : Ensure proper alignment" },
        new string[] { "TASK : Insert the nail carefully", "TASK : Confirm depth and position" },
        new string[] { "TASK : Lock screws in place", "TASK : Perform final inspection", "TASK : Close the wound" }
    };

    void Start()
    {
        taskPanel.SetActive(false);
        UpdateStepUI();
    }

    public void ReductionCompleted()
    {
        if (!isTransitioning && currentStep == 1)
        {
            StartCoroutine(CompleteReductionStep());
        }
    }

    private IEnumerator CompleteReductionStep()
    {
        isTransitioning = true;

        cornerText.text = "Step 1 is finished well!";
        yield return new WaitForSeconds(2f);

        // Move to next step
        currentStep++;
        UpdateStepUI();
        isTransitioning = false;
    }

    public void CompleteStep()
    {
        if (currentStep < stepNames.Length && !isTransitioning && currentStep != 1)
        {
            StartCoroutine(TransitionToNextStep());
        }
        else if (currentStep == stepNames.Length)
        {
            cornerText.text = "All steps are done now!";
            ShowTask("Congratulations! You have completed the surgery.");
        }
    }

    public void ShowTractionTask()
    {
        ShowTask("<b><color=#2A7FFF>TASK : </color></b>Apply traction gently like you see");
    }

    private IEnumerator TransitionToNextStep()
    {
        isTransitioning = true;

        cornerText.text = $"Step {currentStep} is complete!";
        ShowTask("Step completed successfully!");
        yield return new WaitForSeconds(15f);
        HideTask();

        currentStep++;
        cornerText.text = $"Starting {stepNames[currentStep - 1]}...";
        ShowTask("Preparing next step...");
        yield return new WaitForSeconds(15f);
        HideTask();

        UpdateStepUI();
        isTransitioning = false;
    }

    private void UpdateStepUI()
    {
        cornerText.text = stepNames[currentStep - 1];
        StartCoroutine(ShowTaskNotes());
    }

    private IEnumerator ShowTaskNotes()
    {
        if (currentStep == 1)
        {
            // First task
            yield return new WaitForSeconds(2f);
            ShowTask("<b><color=#2A7FFF>TASK : </color></b>Go and Grasp ankle with both hand");
            yield return new WaitForSeconds(3f);
            HideTask();
        }
        else
        {
            foreach (string note in stepNotes[currentStep - 1])
            {
                yield return new WaitForSeconds(5f);
                ShowTask(note.Replace("TASK :", "<b><color=#2A7FFF>TASK : </color></b>"));
                yield return new WaitForSeconds(45f);
                HideTask();
            }
        }
    }

    public void ShowTask(string message)
    {
        taskPanel.SetActive(true);
        taskText.text = message;
    }

    public void HideTask()
    {
        taskPanel.SetActive(false);
    }
}