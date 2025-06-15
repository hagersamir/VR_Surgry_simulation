using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonHandler : MonoBehaviour
{
    public void OnMenuButtonClick()
    {
        int isGuest = PlayerPrefs.GetInt("isGuest", 0); // Default to logged-in

        if (isGuest == 1)
        {
            SceneManager.LoadScene("LoginScene"); // Guest returns to login
        }
        else
        {
            SceneManager.LoadScene("MenuScene"); // Logged-in user returns to menu
        }
    }
}
