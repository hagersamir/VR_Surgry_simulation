using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HandAnimatorController : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;
    public float moveDistance = 0.1f; 
    public float duration = 2f;
    public int repeatCount = 3;
    public StepManager stepManager; 

    public void StartSliding()
    {
        StartCoroutine(SlideHands());
    }

    private IEnumerator SlideHands()
    {
        rightHand.SetActive(true);
        leftHand.SetActive(true);

        Vector3 rightStart = rightHand.transform.position;
        Vector3 leftStart = leftHand.transform.position;
    if (SceneManager.GetActiveScene().name == "TrainingScene")
    {
      stepManager.ShowTractionTask();
    }

        for (int i = 0; i < repeatCount; i++)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = Mathf.Sin((elapsed / duration) * Mathf.PI);
                rightHand.transform.position = rightStart - transform.up * moveDistance * t;
                leftHand.transform.position = leftStart + transform.up * moveDistance * t;
                elapsed += Time.deltaTime;
                yield return null;
            }

            rightHand.transform.position = rightStart;
            leftHand.transform.position = leftStart;

            yield return new WaitForSeconds(0.5f);
        }

        rightHand.SetActive(false);
        leftHand.SetActive(false);

        stepManager.HideTask();
    }
}