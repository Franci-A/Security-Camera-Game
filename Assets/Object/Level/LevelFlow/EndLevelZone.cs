using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelZone : MonoBehaviour
{

    [SerializeField] private int finalLevelIndex = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(SceneManager.GetActiveScene().buildIndex >= finalLevelIndex)
                SceneManager.LoadScene("EndScene");
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
