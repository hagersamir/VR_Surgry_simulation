using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using SimpleJSON; // Add a JSON parser like SimpleJSON (via Unity Asset Store or manually)

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public GameObject loginUI;
    public GameObject infoUI;

    [Header("API Settings")]
    private string apiUrl = "http://localhost:3000/api/auth/login";

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
        PlayerPrefs.SetInt("isGuest", 1); // Mark as guest
        PlayerPrefs.Save();
        SceneManager.LoadScene("TrainingScene");
    }

    IEnumerator LoginCoroutine(string email, string password)
{
    var json = new JSONObject();
    json["email"] = email;
    json["password"] = password;

    byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json.ToString());

    UnityWebRequest www = new UnityWebRequest(apiUrl, "POST");
    www.uploadHandler = new UploadHandlerRaw(jsonBytes);
    www.downloadHandler = new DownloadHandlerBuffer();
    www.SetRequestHeader("Content-Type", "application/json");

    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.Success)
    {
        var jsonResponse = JSON.Parse(www.downloadHandler.text);
        if (jsonResponse["userId"] != null)
        {
            string traineeId = jsonResponse["traineeProfileId"];
            PlayerPrefs.SetString("traineeProfileId", traineeId);
            PlayerPrefs.SetInt("isGuest", 0); // Mark as logged-in user
            PlayerPrefs.Save();

            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            ShowErrorPopup("Login failed. Wrong credentials.");
        }
    }
    else
    {
        ShowErrorPopup("Network error: " + www.error);
    }
}

    void ShowErrorPopup(string message)
    {
        loginUI.SetActive(false);
        infoUI.SetActive(true);

        TextMeshProUGUI infoText = infoUI.GetComponentInChildren<TextMeshProUGUI>();
        if (infoText != null)
        {
            infoText.text = message;
        }
    }

    public void OnTryAgainClick()
    {
        infoUI.SetActive(false);
        loginUI.SetActive(true);
    }
}
