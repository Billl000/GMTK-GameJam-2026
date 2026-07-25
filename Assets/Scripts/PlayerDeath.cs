using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private CharacterController playerController;
    public Vector3 spawnPosition;
    private void Awake()
    {
        spawnPosition = transform.position;
        playerController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DeathZone"))
        {
            TriggerDeath();
        }
    }

    public void TriggerDeath()
    {
        if (!playerController.enabled)
            return;

        playerController.enabled = false; // Disable player controller
        StartCoroutine(DeathCooldownRoutine());

    }

    private IEnumerator DeathCooldownRoutine()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before respawning
        Die();
    }

    private void Die()
    {
        playerController.enabled = false;
        //FindAnyObjectByType<AudioManager>().Play("PlayerDeath"); // Play death sound
        
        foreach (var hand in FindObjectsOfType<ClockHandsSpinning>())
            hand.StopSpin();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        //in the future, animation of reset hand clock
        transform.position = spawnPosition;
        playerController.enabled = true;

        Debug.Log("Player has died!");
    }
}
