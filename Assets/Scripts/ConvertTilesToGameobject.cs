using UnityEngine;
using UnityEngine.Tilemaps;

public class ConvertTilesToGameobject : MonoBehaviour
{
    [SerializeField] private TileBase tileBase;
    [SerializeField] private TileBase tileBaseEnd;
    [SerializeField] private GameObject[] objectsToAttach;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, -0.6f, 0f);
    [SerializeField] private int sortingOrder = 1;

    private Transform[] objects;   // spawned objects, index-matched to objectsToAttach

    private void Start()
    {
        Tile tile = tileBase as Tile;
        Tile tileEnd = tileBaseEnd as Tile;

        if (tile == null || tile.sprite == null)
        {
            Debug.LogWarning($"{name}: tileBase is missing or isn't a Tile with a sprite.", this);
            return;
        }

        objects = new Transform[objectsToAttach.Length];

        for (int i = 0; i < objectsToAttach.Length; i++)
        {
            if (objectsToAttach[i] == null) continue;

            GameObject obj = new GameObject(gameObject.name + "_Tile");
            obj.transform.position = objectsToAttach[i].transform.position + spawnOffset;

            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = tile.sprite;
            sr.sortingOrder = sortingOrder;

            if (gameObject.name == "Vines" && tileEnd != null && tileEnd.sprite != null)
            {
                GameObject objEnd = new GameObject(gameObject.name + "_Tile_End");
                objEnd.transform.SetParent(obj.transform, false);
                SpriteRenderer srEnd = objEnd.AddComponent<SpriteRenderer>();
                srEnd.sprite = tileEnd.sprite;
                srEnd.sortingOrder = sortingOrder;
                objEnd.transform.localPosition = new Vector3(0f, -1f, 0f);  // adjust as needed

            }
            
            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;   // see note below

            BoxCollider2D bc = obj.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;



            objects[i] = obj.transform;   // keep the reference
        }
    }

    private void Update()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null && objectsToAttach[i] != null && gameObject.name == "Vines") {
                objects[i].position = objectsToAttach[i].transform.position + spawnOffset;
            }
            else if (objects[i] != null && objectsToAttach[i] != null && gameObject.name == "Spikes") {
                objects[i].transform.SetParent(objectsToAttach[i].transform, false);
                objects[i].localPosition = spawnOffset;
                objects[i].localRotation = Quaternion.Euler(0, 0, -90);
                objects[i].tag = "DeathZone";
            }
            
        }
    }
}