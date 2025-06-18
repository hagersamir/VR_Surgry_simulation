
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

public class SurgeryDataSender : MonoBehaviour
{
  ReductionScript reduction = FindObjectOfType<ReductionScript>();

    private string apiUrl = "http://localhost:3000/api/surgery-attempts";

    void Start()
    {
        // Automatically send mock data on scene load (for testing)
        StartCoroutine(SendMockData());
    }

    // Optional: Call this from a Unity Button to manually send test data
    public void TriggerTestSend()
    {
        StartCoroutine(SendMockData());
    }

    IEnumerator SendMockData()
    {
        string traineeId = PlayerPrefs.GetString("traineeProfileId", null);
        if (string.IsNullOrEmpty(traineeId))
        {
            Debug.LogError("TraineeProfileId not found in PlayerPrefs.");
            yield break;
        }

        SurgeryAttemptMock mockData = new SurgeryAttemptMock
        {
          traineeProfileId = traineeId,
          reductionDuration = StepManager.reductionDuration,
          reductionNeededBoneLength = reduction.NeededBoneLength,
          reductionActualBoneLength = reduction.ActualBoneLength,
          reductionAccuracy = reduction.AlignmentAccuracy,
          reductionBeforeReductionXrayImg = "../SavedImages/BEFORE REDUCTION.png",
          reductionAfterReductionXrayImg = "../SavedImages/AFTER REDUCTION.png",
          
          
          // totalTime = 130.0f,
          // score = "8533",
          // xrayImagePath = "C:/Users/hager/OneDrive/Pictures/Saved Pictures/inspirational-wallpapers-stugon.com-2.jpg", // Replace with valid image URL if needed
          // isCompleted = true,
          // performanceDetail = "Mock performance analysis data",
          // attemptDate = DateTime.UtcNow.ToString("o") 
        };

        string jsonBody = JsonUtility.ToJson(mockData);
        Debug.Log("Sending JSON: " + jsonBody);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Surgery data sent successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Error sending surgery data: " + request.error);
            Debug.LogError("❌ Server response: " + request.downloadHandler.text);
        }
    }
}

[System.Serializable]
public class SurgeryAttemptMock
{
  public string traineeProfileId;


  public float reductionDuration;
  public float reductionNeededBoneLength;
  public float reductionActualBoneLength;
  public float reductionAccuracy;
  public string reductionBeforeReductionXrayImg;
  public string reductionAfterReductionXrayImg;

  public float entrySiteDuration;
  public float nailInsertionDuration;
  public float lockingClosureDuration;

    // public float totalTime;
  // public string score;
  // public bool isCompleted;
  // public string performanceDetail;
  // public string attemptDate; // Newly added field
}