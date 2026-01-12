using UnityEngine;

public class Command
{
    string commandName;
    public string GetCommandName=> commandName;
    

    bool isValid;
    
    public bool IsValid => isValid;

    string reponse;
    public string GetResponse()
    {
        return reponse;
    }

    public Command(string name, bool valid, string response)
    {
        commandName = name;
        isValid = valid;
        reponse = response;
    }
}
