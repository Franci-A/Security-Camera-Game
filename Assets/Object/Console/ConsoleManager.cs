using TMPro;
using UnityEngine;
using System.Collections.Generic;
using HelperScripts.EventSystem;

public class ConsoleManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI consoleTextScreen;
    private List<string> commandLines;
    [SerializeField] private EventObjectScriptable onValidateEvent;
       
    private void Awake()
    {
        commandLines = new List<string>();
        onValidateEvent.AddListener(AddCommandLine);
    }


    public void AddCommandLine(object obj)
    {
        Command command = (Command)obj;
        commandLines.Add(command.GetCommandName);
        commandLines.Add(command.GetResponse());
        UpdateConsoleScreen();
    }

    public void UpdateConsoleScreen()
    {
        consoleTextScreen.text = string.Join("\n", commandLines);
    }

    private void OnDestroy()
    {
        onValidateEvent.RemoveListener(AddCommandLine);
    }
}
