using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataHandler : MonoBehaviour
{
  [Header("Reduction")]
    public TextMeshProUGUI ReductionDurationValue;

    public TextMeshProUGUI ReductionDistanceErrorValue;
    public TextMeshProUGUI ReductionStepAccuracyValue;
    public Image ReductionImage01;
    public Image ReductionImage02;

  [Header("Entry / Cutting")]
    
    public TextMeshProUGUI CuttingDistanceErrorValue;
    public TextMeshProUGUI CuttingStepAccuracyValue;
    public Image CuttingXRayImage;

    [Header("Insertion")]
    public TextMeshProUGUI InsertionDurationValue;
    public TextMeshProUGUI InsertionDistanceErrorValue;
    public TextMeshProUGUI InsertionStepAccuracyValue;
    public Image InsertionImage01;
    public Image InsertionImage02;

    [Header("Locking")]
    public TextMeshProUGUI LockingDurationValue;
    public TextMeshProUGUI LockingDistanceErrorValue;
    public TextMeshProUGUI LockingDistanceErrorValue02;
    public TextMeshProUGUI LockingDistanceErrorValue03;
    public TextMeshProUGUI LockingStepAccuracyValue;
    public Image LockingImage01;
    public Image LockingImage02;
    public Image LockingImage03;
    public Image LockingImage04;

    [Header("Locking Step Toggles")]
    public Toggle FirstProximalCutToggle;
    public Toggle FirstProximalDrillToggle;
    public Toggle FirstProximalScrewLockingToggle;

    public Toggle SecondProximalCutToggle;
    public Toggle SecondProximalDrillToggle;
    public Toggle SecondProximalScrewLockingToggle;

    public Toggle DistalCutToggle;
    public Toggle DistalDrillToggle;
    public Toggle DistalScrewLockingToggle;

    public void SetAssessmentValues(
        float reductionDuration,float reductionError, float reductionAccuracy,
        float cuttingError, float cuttingAccuracy,
        float insertionDuration, float insertionError, float insertionAccuracy,
        float lockingDuration, float lockingError1, float lockingError2, float lockingError3, float lockingAccuracy,
        Sprite reductionImg1, Sprite reductionImg2,
        Sprite cuttingImg,
        Sprite insertionImg1, Sprite insertionImg2,
        Sprite lockingImg1, Sprite lockingImg2, Sprite lockingImg3, Sprite lockingImg4,
        bool firstProxCut, bool firstProxDrill, bool firstProxScrew,
        bool secondProxCut, bool secondProxDrill, bool secondProxScrew,
        bool distalCut, bool distalDrill, bool distalScrew
    )
    {
    // Reduction
        ReductionDurationValue.text = $"{reductionDuration:F2} sec";

        ReductionDistanceErrorValue.text = $"{reductionError:F2} mm";
        ReductionStepAccuracyValue.text = $"{reductionAccuracy * 100:F1}%";
        ReductionImage01.sprite = reductionImg1;
        ReductionImage02.sprite = reductionImg2;

        // Entry / Cutting
        CuttingDistanceErrorValue.text = $"{cuttingError:F2} mm";
        CuttingStepAccuracyValue.text = $"{cuttingAccuracy * 100:F1}%";
        CuttingXRayImage.sprite = cuttingImg;

        // Insertion
        InsertionDurationValue.text = $"{insertionDuration:F2} sec";
        InsertionDistanceErrorValue.text = $"{insertionError:F2} mm";
        InsertionStepAccuracyValue.text = $"{insertionAccuracy * 100:F1}%";
        InsertionImage01.sprite = insertionImg1;
        InsertionImage02.sprite = insertionImg2;

        // Locking
        LockingDurationValue.text = $"{lockingDuration:F2} sec";
        LockingDistanceErrorValue.text = $"{lockingError1:F2} mm";
        LockingDistanceErrorValue02.text = $"{lockingError2:F2} mm";
        LockingDistanceErrorValue03.text = $"{lockingError3:F2} mm";
        LockingStepAccuracyValue.text = $"{lockingAccuracy * 100:F1}%";
        LockingImage01.sprite = lockingImg1;
        LockingImage02.sprite = lockingImg2;
        LockingImage03.sprite = lockingImg3;
        LockingImage04.sprite = lockingImg4;

        // Locking Toggles
        FirstProximalCutToggle.isOn = firstProxCut;
        FirstProximalDrillToggle.isOn = firstProxDrill;
        FirstProximalScrewLockingToggle.isOn = firstProxScrew;

        SecondProximalCutToggle.isOn = secondProxCut;
        SecondProximalDrillToggle.isOn = secondProxDrill;
        SecondProximalScrewLockingToggle.isOn = secondProxScrew;

        DistalCutToggle.isOn = distalCut;
        DistalDrillToggle.isOn = distalDrill;
        DistalScrewLockingToggle.isOn = distalScrew;
    }
}

/*
ExampleUsage:

DataHandler.SetAssessmentValues(
    12.5f , 2.4f, 0.95f,
    1.5f, 0.88f,
    30f, 2.1f, 0.9f,
    42f, 1.2f, 2.2f, 1.1f, 0.92f,
    img1, img2, cutXRay, ins1, ins2, lock1, lock2, lock3, lock4,

    true, true, false,    // First proximal
    true, false, true,    // Second proximal
    true, true, true      // Distal
);
*/