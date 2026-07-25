using System.Collections;
using UnityEngine;

public class DropThroughPlatform : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private float disableTime = 0.4f;

    private void Update()
    {
        bool holdingDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        if (holdingDown && Input.GetButtonDown("Jump"))
            TryDropThrough();
    }
    private void TryDropThrough()
    {
        // Find the platform under the player
        RaycastHit2D hit = Physics2D.Raycast(
            playerCollider.bounds.center, Vector2.down,
            playerCollider.bounds.extents.y + 0.2f,
            LayerMask.GetMask("OneWayPlatform"));

        if (hit.collider != null &&
            hit.collider.TryGetComponent(out PlatformEffector2D _))
        {
            StartCoroutine(DisableCollision(hit.collider));
        }
    }

    private IEnumerator DisableCollision(Collider2D platform)
    {
        Physics2D.IgnoreCollision(playerCollider, platform, true);
        yield return new WaitForSeconds(disableTime);
        Physics2D.IgnoreCollision(playerCollider, platform, false);
    }
}