
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class StepManager : MonoBehaviour
// {
//   public TextMeshProUGUI cornerText;
//   public TextMeshProUGUI taskText;
//   public EventManager eventManager;      // Assign in inspector
//   public GameObject taskPanel;
//   private int currentStep = 1;
//   private bool isTransitioning = false;
//   private Dictionary<int, float> stepStartTimes = new Dictionary<int, float>();
//   public static float reductionDuration;
//   public static float entrySiteDuration;
//   public static float nailInsertionDuration;
//   public static float lockingClosureDuration;

//   private string[] stepNames =
//   {
//         "Step 1: Reduction",
//         "Step 2: Entry Site",
//         "Step 3: Nail Insertion",
//         "Step 4: Locking and Closure"
//     };

//   void Start()
//   {
//     taskPanel.SetActive(false);
//     RecordStepStartTime();
//     UpdateStepUI();

//   }

//   public void ReductionCompleted() { currentStep = 1; CompleteStep(); }
//   public void EntrySiteCompleted() { currentStep = 2; CompleteStep(); }
//   public void NailInsertionCompleted() { currentStep = 3; CompleteStep(); }
//   public void Locking_ClosureCompleted() { currentStep = 4; CompleteStep(); }


//   public void CompleteStep()
//   {
//     if (currentStep < stepNames.Length && !isTransitioning)
//     {
//       RecordStepDuration();  // Record how long the step took
//       StartCoroutine(TransitionToNextStep());
//     }
//     else if (currentStep == stepNames.Length)
//     {
//       cornerText.text = "All steps are done now!";
//       ShowTask("Congratulations! You have completed the surgery.");
//     }
//   }

//   public void ShowTractionTask()
//   {
//     ShowTask("<b><color=#2A7FFF>TASK : </color></b>Apply traction gently like you see");
//   }

//   private IEnumerator TransitionToNextStep()
//   {
//     isTransitioning = true;

//     cornerText.text = $"Step {currentStep} is complete!";
//     ShowTask($"Step {currentStep} completed successfully!");
//     yield return new WaitForSeconds(5f);
//     HideTask();

//     if (currentStep == 1)
//     {
//       eventManager.OnEventReductionDone();  // Ensures event still fires
//     }
//     // move to next step 
//     currentStep++;
//     RecordStepStartTime();  // Start tracking next step
//     cornerText.text = $"Starting {stepNames[currentStep - 1]}...";
//     ShowTask("Preparing next step...");
//     yield return new WaitForSeconds(2f);
//     HideTask();

//     UpdateStepUI();
//     isTransitioning = false;
//   }


//   private void UpdateStepUI()
//   {
//     cornerText.text = stepNames[currentStep - 1];
//     StartCoroutine(ShowTaskNotes());
//   }

//   private IEnumerator ShowTaskNotes()
//   {
//     if (currentStep == 1)
//     {
//       // First task
//       yield return new WaitForSeconds(2f);
//       ShowTask("<b><color=#2A7FFF>TASK : </color></b>Go and Grasp ankle with both hand");
//       yield return new WaitForSeconds(3f);
//       HideTask();
//     }
//     else if (currentStep == 2)
//     {
//       ShowTask("<b><color=#2A7FFF>Task 1: </color></b>Use the scalpel to make the initial incision and open the entry site over the proximal tibia");
//     }
//     else if (currentStep == 3)
//     {
//       ShowTask("<b><color=#2A7FFF>Task 1:</color></b> Advance the guide wire through the medullary canal to the distal end of the tibia.");
//     }
//     else if (currentStep == 4)
//     {
//       ShowTask("<b><color=#2A7FFF>Task 1:</color></b> Place trochar on bone and  mark skin");
//     }
//   }

//   public void ShowTask(string message)
//   {
//     taskPanel.SetActive(true);
//     taskText.text = message;
//   }

//   public void HideTask()
//   {
//     taskPanel.SetActive(false);
//   }
//     private void RecordStepStartTime()
//     {
//         stepStartTimes[currentStep] = Time.time;
//     }

//     private void RecordStepDuration()
//     {
//         if (stepStartTimes.ContainsKey(currentStep))
//         {
//             float duration = Time.time - stepStartTimes[currentStep];

//             switch (currentStep)
//             {
//                 case 1:
//                     reductionDuration = duration;
//                     break;
//                 case 2:
//                     entrySiteDuration = duration;
//                     break;
//                 case 3:
//                     nailInsertionDuration = duration;
//                     break;
//                 case 4:
//                     lockingClosureDuration = duration;
//                     break;
//             }
//         }
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StepManager : MonoBehaviour
{
    public TextMeshProUGUI cornerText;
    public TextMeshProUGUI taskText;
    public EventManager eventManager;
    public GameObject taskPanel;

    private int currentStep = 1;
    private bool isTransitioning = false;
    private bool isTrainingMode;

    private Dictionary<int, float> stepStartTimes = new Dictionary<int, float>();

    public static float reductionDuration;
    public static float entrySiteDuration;
    public static float nailInsertionDuration;
    public static float lockingClosureDuration;

    private string[] stepNames =
    {
        "Step 1: Reduction",
        "Step 2: Entry Site",
        "Step 3: Nail Insertion",
        "Step 4: Locking and Closure"
    };

    void Start()
    {
        // Set training mode based on scene name
        isTrainingMode = SceneManager.GetActiveScene().name == "TrainingScene";

        taskPanel.SetActive(false);
        RecordStepStartTime();
        UpdateStepUI();
    }

      public void ReductionCompleted() { currentStep = 1; CompleteStep(); }
      public void EntrySiteCompleted() { currentStep = 2; CompleteStep(); }
      public void NailInsertionCompleted() { currentStep = 3; CompleteStep(); }
      public void Locking_ClosureCompleted() { currentStep = 4; CompleteStep(); }

    public void CompleteStep()
    {
        if (currentStep <= stepNames.Length && !isTransitioning)
        {
            RecordStepDuration();
            if (currentStep == 1)
            {
                Debug.Log($"[StepManager] Reduction Duration: {reductionDuration:F2} seconds");
            }
            if (currentStep < stepNames.Length)
            {
                StartCoroutine(TransitionToNextStep());
            }
            else if (isTrainingMode)
            {
                cornerText.text = "All steps are done now!";
                ShowTask("Congratulations! You have completed the surgery.");
            }
        }
    }

      public void ShowTractionTask()
  {
    ShowTask("<b><color=#2A7FFF>TASK : </color></b>Apply traction gently like you see until you get reduction is complete");
  }
    private IEnumerator TransitionToNextStep()
  {
    isTransitioning = true;

    if (isTrainingMode)
    {
      cornerText.text = $"Step {currentStep} is complete!";
      ShowTask($"Step {currentStep} completed successfully!");
    }

    yield return new WaitForSeconds(5f);
    HideTask();

    if (currentStep == 1)
    {
      eventManager.OnEventReductionDone();
    }

    currentStep++;
    RecordStepStartTime();

    if (isTrainingMode)
    {
      cornerText.text = $"Starting {stepNames[currentStep - 1]}...";
      ShowTask("Preparing next step...");
    }

    yield return new WaitForSeconds(2f);
    HideTask();

    UpdateStepUI();
    isTransitioning = false;
  }

    private void UpdateStepUI()
    {
        if (isTrainingMode)
        {
            cornerText.text = stepNames[currentStep - 1];
            StartCoroutine(ShowTaskNotes());
        }
    }

    private IEnumerator ShowTaskNotes()
    {
        if (!isTrainingMode) yield break;

        if (currentStep == 1)
        {
            yield return new WaitForSeconds(2f);
            ShowTask("<b><color=#2A7FFF>TASK : </color></b>Go and Grasp ankle with both hand");
            yield return new WaitForSeconds(3f);
            HideTask();
        }
        else if (currentStep == 2)
        {
            ShowTask("<b><color=#2A7FFF>Task 1: </color></b>Use the scalpel to make the initial incision and open the entry site over the proximal tibia");
        }
        else if (currentStep == 3)
        {
            ShowTask("<b><color=#2A7FFF>Task 1:</color></b> Advance the guide wire through the medullary canal to the distal end of the tibia.");
        }
        else if (currentStep == 4)
        {
            ShowTask("<b><color=#2A7FFF>Task 1:</color></b> Place trochar on bone and  mark skin");
        }
    }

    public void ShowTask(string message)
    {
        // if (!isTrainingMode) return;

        taskPanel.SetActive(true);
        taskText.text = message;
    }

    public void HideTask()
    {
        if (!isTrainingMode) return;

        taskPanel.SetActive(false);
    }

    private void RecordStepStartTime()
    {
        stepStartTimes[currentStep] = Time.time;
    }

    private void RecordStepDuration()
    {
        if (stepStartTimes.ContainsKey(currentStep))
        {
            float duration = Time.time - stepStartTimes[currentStep];

            switch (currentStep)
            {
                case 1:
                    reductionDuration = duration;
                    break;
                case 2:
                    entrySiteDuration = duration;
                    break;
                case 3:
                    nailInsertionDuration = duration;
                    break;
                case 4:
                    lockingClosureDuration = duration;
                    break;
            }
        }
    }
}
