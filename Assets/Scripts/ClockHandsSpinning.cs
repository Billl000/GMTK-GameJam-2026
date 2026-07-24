using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockHandsSpinning : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public bool triggersDeath = false;  

    public float countdownDuration = 20f;
    private float timeRemaining;
    private PlayerDeath playerDeath;
    private bool hasTriggered;

    private void Awake()
    {
        timeRemaining = countdownDuration;
        if (triggersDeath)
            playerDeath = FindObjectOfType<PlayerDeath>();
    }

    private void Update()
    {
        if (triggersDeath)
        {
            if (hasTriggered) return;

            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
            else
            {
                hasTriggered = true;
                playerDeath.TriggerDeath();
            }
        }
        else
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
