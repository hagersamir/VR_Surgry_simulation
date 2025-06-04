using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_manager : MonoBehaviour
{
    public void LoadTraining()
    {
        SceneManager.LoadScene("TrainingScene"); // Change the scene name
    }

    public void LoadAssessment()
    {
        SceneManager.LoadScene("AssessmentScene"); // Change the scene name
    }

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
