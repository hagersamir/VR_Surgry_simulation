
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;

public class SurgeryDataSender : MonoBehaviour
{
  ReductionScript reduction = FindObjectOfType<ReductionScript>();
  Blade cuttingEntrySite = FindObjectOfType<Blade>();
  THandleGuideWire THandleWire = FindObjectOfType<THandleGuideWire>();
  GuideWire guideWire = FindObjectOfType<GuideWire>();
  Nail nail = FindObjectOfType<Nail>();
  EventManager manager = FindObjectOfType<EventManager>();

  public ScrewAttachment screw1Data;
  public ScrewAttachment screw2Data;
  public ScrewAttachment screw3Data;
  public stepsAccuracy stepsData;

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
      reductionErrorLength = reduction.reductionErrorLength,
      reductionAccuracy = reduction.AlignmentAccuracy,
      reductionBeforeReductionXrayImg = "../SavedImages/BEFORE REDUCTION.png",
      reductionAfterReductionXrayImg = "../SavedImages/AFTER REDUCTION.png",
      entrySiteDuration = THandleWire.entrySiteDuration,
      cuttingScreenshotImg = cuttingEntrySite.cuttingScreenshotImg,
      cuttingAccuracy = cuttingEntrySite.cuttingAccuracy,
      neededThandleDepth = THandleWire.neededThandleDepth,
      actualThandleDepth = THandleWire.actualThandleDepth,
      tHandleAccuracy = THandleWire.tHandleAccuracy,
      nailInsertionDuration = guideWire.nailInsertionDuration,
      guideWireXrayImg = guideWire.guideWireXrayImg,
      neededWireDepth = guideWire.neededWireDepth,
      actualWireDepth = guideWire.actualWireDepth,
      wirePositionAccuracy = guideWire.wirePositionAccuracy,
      neededNailDepth = nail.neededNailDepth,
      actualNailDepth = nail.actualNailDepth,
      nailPositionAccuracy = nail.nailPositionAccuracy,
      toolUsageOrder = manager.toolUsageOrder,





      NailLockingSteps = stepsData.NailLockingSteps,
      stepsAccurcy = stepsData.stepsAccurcy,

      firstProximalLockingXray = "../SavedImages/First Proximal.png",
      secondProximalLockingXray = "../SavedImages/Second Proximal.png",
      distalLockingXrayTopView = "../SavedImages/Distal Top View.png",
      distalLockingXraySideView = "../SavedImages/Distal Side View.png",


      firstProximalLockingScrewPosAccurcy = screw1Data.ScrewPositionAcc,
      secondProximalLockingScrewPosAccurcy = screw2Data.ScrewPositionAcc,
      DistalLockingScrewPosAccurcy = screw3Data.ScrewPositionAcc,



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

  // reduction data 
  public float reductionDuration;
  public float reductionErrorLength;
  public float reductionAccuracy;
  public string reductionBeforeReductionXrayImg;
  public string reductionAfterReductionXrayImg;
  // cutting data 
  public float entrySiteDuration;
  public string cuttingScreenshotImg;
  public float cuttingAccuracy;
  public float neededThandleDepth;
  public float actualThandleDepth;
  public float tHandleAccuracy;
  // guide and nail insertion data
  public string guideWireXrayImg;
  public string nailXrayImg;
  public StepToolAccuracy stepToolAccuracy;
  public float neededWireDepth;
  public float actualWireDepth;
  public float wirePositionAccuracy;
  public float neededNailDepth;
  public float actualNailDepth;
  public float nailPositionAccuracy;

  public float nailInsertionDuration;
  public float lockingClosureDuration;
  public List<string> toolUsageOrder;




  public Dictionary<string, (bool isDone, int stepOrder)> NailLockingSteps;

  public float stepsAccurcy;

  //Images
  public string firstProximalLockingXray;
  public string secondProximalLockingXray;
  public string distalLockingXrayTopView;
  public string distalLockingXraySideView;


  //Screw Positions Accurcy 


  public float firstProximalLockingScrewPosAccurcy;
  public float secondProximalLockingScrewPosAccurcy;
  public float DistalLockingScrewPosAccurcy;



  // public float totalTime;
  // public string score;
  // public bool isCompleted;
  // public string performanceDetail;
  // public string attemptDate; // Newly added field
}

[System.Serializable]
public class StepToolAccuracy
{
  public bool THandle;
  public bool Awl;
  public bool GuideWire;
  public bool Nail;
  public bool GuideWireRemoval;
}
