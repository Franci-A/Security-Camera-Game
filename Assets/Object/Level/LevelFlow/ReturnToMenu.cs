using UnityEngine;
using UnityEngine.InputSystem;

public class ReturnToMenu : MonoBehaviour
{
    float timer = 0f;
    private void Start()
    {
        timer = Time.time;
    }

    public void PressAnyKey(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (Time.time - timer > 3f)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
