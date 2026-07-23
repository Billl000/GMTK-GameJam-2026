using UnityEngine;

public class ClockHandsSpinning : MonoBehaviour
{
    [SerializeField] public float rotationSpeed = 100f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
