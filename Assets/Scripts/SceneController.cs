using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BoneSelectorUI : MonoBehaviour
{
  public GameObject popUpWindow;
  // public GameObject[] bonesToShow;
  public GameObject Bone1;
  public GameObject Bone2;
  public GameObject Bone3;

  void Start()
  {
    popUpWindow.SetActive(true);

    // // Hide all bones initially
    // foreach (var bone in bonesToShow)
    // {
    //   bone.SetActive(false);
    // }
  }

  // public void OnCaseSelected(int caseIndex)
  // {
  //   popUpWindow.SetActive(false);

  // }

  public void Case1()
  {
    popUpWindow.SetActive(false);
    Bone1.SetActive(true);
  }
  public void Case2()
  {
    popUpWindow.SetActive(false);
    Bone2.SetActive(true);
  }
  public void Case3()
  {
    popUpWindow.SetActive(false);
    Bone3.SetActive(true);
  }

  void Update()
  {
    if(Input.GetKeyDown(KeyCode.V)){
      Case1();
    }
  }
}
