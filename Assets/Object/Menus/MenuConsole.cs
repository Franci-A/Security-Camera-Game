using HelperScripts.EventSystem;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuConsole : MonoBehaviour
{
    [SerializeField] private TMP_InputField controlPanelInput;
    [SerializeField] private EventObjectScriptable onValidateCommandeEvent;
    [SerializeField] private int numberOfLevels = 3;

    void Start()
    {
        controlPanelInput.ActivateInputField();
    }

    public void OnValidateCommande(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (controlPanelInput.text.Length == 0)
            return;

        string value = controlPanelInput.text.ToLower().Trim();
        controlPanelInput.text = "";

        string[] inputs = value.Split(' ');

        Command command = new Command(value, false, "Unknown command");

        if (inputs[0].CompareTo("level") == 0)
        {
            command = LoadLevel(value, inputs);
        }
        else if (inputs[0].CompareTo("play") == 0)
        {
            command = Play(value, inputs);
        }

        onValidateCommandeEvent.Call(command);
        controlPanelInput.ActivateInputField();
    }


    private Command LoadLevel(string value , string[] inputs)
    {
        if (inputs.Length < 2)
            return new Command(value, false, "Level number not specified");
        else if (int.TryParse(inputs[1], out int levelNumber))
        {
            if(levelNumber > numberOfLevels || levelNumber < 1)
                return new Command(value, false, "Invalid level");
            else
            {
                StartCoroutine(WaitLoadScene(levelNumber));
                return new Command(value, true, $"Loading level {levelNumber}...");
            }
        }
        return new Command(value, false, "Invalid level");

    }

    private Command Play(string value, string[] inputs)
    {
        if (inputs.Length < 2)
            return new Command(value, false, "Invalid Command");

        StartCoroutine(WaitLoadScene(1));
        return new Command(value, true, $"Loading level 1...");

    }

    IEnumerator WaitLoadScene(int levelNumber)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelNumber);
    }
}
