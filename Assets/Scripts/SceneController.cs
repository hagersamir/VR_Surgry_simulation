using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class BoneSelectorUI : MonoBehaviour
{
  public GameObject popUpWindow;
  public GameObject menu;
  public GameObject currentStep;
  // public GameObject xray;
  public GameObject Bone1;
  public GameObject Bone2;
  public StepManager stepManager; 
  public NurseController nurseController; 
  private bool IsTrainingMode => SceneManager.GetActiveScene().name == "TrainingScene";


  // public GameObject Bone3;

  void Start()
  {
    popUpWindow.SetActive(true);
    menu.SetActive(false);
    if(currentStep!=null) currentStep.SetActive(false);
    // xray.SetActive(false);

  }


  public void Case1()
  {
    popUpWindow.SetActive(false);
    menu.SetActive(true);
    if (currentStep != null) currentStep.SetActive(true);
    // xray.SetActive(true);
    Bone1.SetActive(true);
    if (stepManager != null && IsTrainingMode)
    StartCoroutine(stepManager.showInitially());
    // if (nurseController != null)
    nurseController.StartNurseActions();
    
  }
  public void Case2()
  {
    popUpWindow.SetActive(false);
    menu.SetActive(true);
    if (currentStep != null) currentStep.SetActive(true);
    // xray.SetActive(true);
    Bone2.SetActive(true);
    if (stepManager != null && IsTrainingMode)
    StartCoroutine(stepManager.showInitially());
    // if (nurseController != null)
    nurseController.StartNurseActions();  
  }
  // public void Case3()
  // {
  //   popUpWindow.SetActive(false);
  //   Bone3.SetActive(true);
  // }

  void Update()
  {
    if(Input.GetKeyDown(KeyCode.V)){
      Case1();
    }
  }
}
