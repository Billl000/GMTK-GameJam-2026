using UnityEngine;
using UnityEngine.Tilemaps;

public class ConvertTilesToGameobject : MonoBehaviour
{
    [SerializeField] private TileBase tileBase;
    [SerializeField] private GameObject[] objectsToAttach;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, -0.6f, 0f);
    [SerializeField] private int sortingOrder = 1;

    private Transform[] vines;   // spawned objects, index-matched to objectsToAttach

    private void Start()
    {
        Tile tile = tileBase as Tile;
        if (tile == null || tile.sprite == null)
        {
            Debug.LogWarning($"{name}: tileBase is missing or isn't a Tile with a sprite.", this);
            return;
        }

        vines = new Transform[objectsToAttach.Length];

        for (int i = 0; i < objectsToAttach.Length; i++)
        {
            if (objectsToAttach[i] == null) continue;

            GameObject obj = new GameObject("Vines_Tile");
            obj.transform.position = objectsToAttach[i].transform.position + spawnOffset;

            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = tile.sprite;
            sr.sortingOrder = sortingOrder;

            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;   // see note below
            obj.AddComponent<BoxCollider2D>();

            vines[i] = obj.transform;   // keep the reference
        }
    }

    private void Update()
    {
        for (int i = 0; i < vines.Length; i++)
        {
            if (vines[i] != null && objectsToAttach[i] != null)
                vines[i].position = objectsToAttach[i].transform.position + spawnOffset;
        }
    }
}