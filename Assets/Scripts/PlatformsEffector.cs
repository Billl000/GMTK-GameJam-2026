using UnityEngine;

public class PlatformsEffector : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D platformEffector;
    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
       platformEffector.rotationalOffset = -transform.eulerAngles.z;
    }
}
