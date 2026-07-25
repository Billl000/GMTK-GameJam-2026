using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private bool inDoors;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            inDoors = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player is in the trigger and pressed E.");
            FindAnyObjectByType<AudioManager>().Play("Level Completed");
            // Load the next scene when the player is in the trigger and presses E
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    private void Update()
    {
        if (inDoors && Input.GetKeyDown(KeyCode.E))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
