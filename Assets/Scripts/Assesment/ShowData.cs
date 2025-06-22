using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowData : MonoBehaviour
{
    public void OnShowDataButtonClicked()
    {
        SceneManager.LoadScene("AttemptResults");
    }
}
