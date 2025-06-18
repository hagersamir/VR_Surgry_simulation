using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hiddenInteractor : MonoBehaviour
{
  // Start is called before the first
  //  frame update
  public GameObject interactorRight;
  public GameObject interactorLeft;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.N))
    {
      interactorLeft.SetActive(true);
      interactorRight.SetActive(true);
        }
    if (Input.GetKeyDown(KeyCode.B))
    {
      interactorLeft.SetActive(false);
      interactorRight.SetActive(false);
        }
    }
    }

