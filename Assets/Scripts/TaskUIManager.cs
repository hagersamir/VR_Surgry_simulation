using UnityEngine;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public TextMeshProUGUI taskText;

    public void UpdateTask(string newTask)
    {
        taskText.text = $"<b><color=#2A7FFF>TASK</color></b>\n{newTask}";
    }
}
