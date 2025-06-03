using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class SurgeryDataSender : MonoBehaviour
{
    private string apiUrl = "http://localhost:3000/api/surgery-attempts";

    void Start()
    {
        StartCoroutine(SendMockData());
    }

    IEnumerator SendMockData()
    {
        string traineeId = PlayerPrefs.GetString("traineeProfileId", null);
        if (string.IsNullOrEmpty(traineeId))
        {
            Debug.LogError("TraineeProfileId not found.");
            yield break;
        }

        string jsonBody = JsonUtility.ToJson(new SurgeryAttemptMock()
        {
            traineeProfileId = traineeId,
            totalTime = 120.0f,
            score = 85,
            xrayImagePath = "path/to/xray.png",
            isCompleted = true,
            performanceDetail = "Mock performance analysis data"
        });

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Surgery data sent successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error sending surgery data: " + request.error);
        }
    }
}

[System.Serializable]
public class SurgeryAttemptMock
{
    public string traineeProfileId;
    public float totalTime;
    public int score;
    public string xrayImagePath;
    public bool isCompleted;
    public string performanceDetail;
}
