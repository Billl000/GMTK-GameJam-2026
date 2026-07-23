using UnityEngine;

public class ClockHandsSpinning : MonoBehaviour
{
    [SerializeField] public float rotationSpeed = 100f;
    
    public void ResetClock()
    {
        transform.rotation = Quaternion.identity;
    }
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

    }
}
