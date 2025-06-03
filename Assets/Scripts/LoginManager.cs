using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public GameObject loginUI;
    public GameObject infoUI;

    [Header("API Settings")]
    private string apiUrl = "https://aylink/login"; 

    void Start()
    {
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        passwordInput.ForceLabelUpdate();

        
        loginUI.SetActive(true);
        infoUI.SetActive(false);
    }

    public void OnLoginButtonClick()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowErrorPopup("Please enter email and password.");
            return;
        }

        StartCoroutine(LoginCoroutine(email, password));
    }

    public void OnGuestButtonClick()
    {
        SceneManager.LoadScene("TrainingScene");
    }

    IEnumerator LoginCoroutine(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(apiUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            if (www.downloadHandler.text.Contains("success")) 
            {
                SceneManager.LoadScene("MenuScene");
            }
            else
            {
                // Login failed
                loginUI.SetActive(false);
                infoUI.SetActive(true);
            }
        }
        else
        {
            ShowErrorPopup("Network error: " + www.error);
        }
    }

    void ShowErrorPopup(string message)
    {
        // Optional: Add a TMP text in infoUI for error details
        loginUI.SetActive(false);
        infoUI.SetActive(true);

        TextMeshProUGUI infoText = infoUI.GetComponentInChildren<TextMeshProUGUI>();
        if (infoText != null)
        {
            infoText.text = message;
        }
    }

    // Button to go back from Info UI
    public void OnTryAgainClick()
    {
        infoUI.SetActive(false);
        loginUI.SetActive(true);
    }
}
