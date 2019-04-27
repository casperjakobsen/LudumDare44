using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    Tilemap tilemap;

    [SerializeField] Counter counter;

    void Start()
    {
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    public bool Move(Vector3Int origin, Vector3Int movementInt)
    {
        Vector3Int examinePos = origin + movementInt;
        bool bloodOnPath = false;
        while (true)
        {
            TileBase tile = tilemap.GetTile(examinePos);
            if (tile == null || (bloodOnPath && tile.name == "Hole")){
                break;
            }
            if (tile.name != "Blood"){
                return false;
            }
            else
            {
                bloodOnPath = true;
            }
            examinePos += movementInt;
        }

        PushObjectsOnPath(origin, movementInt, examinePos);
        return true;
    }

    void PushObjectsOnPath(Vector3Int origin, Vector3Int movementInt, Vector3Int destination)
    {
        Vector3Int examinePos = destination;
        TileBase destinationTile = tilemap.GetTile(examinePos);
        if (destinationTile != null && destinationTile.name == "Hole")
        {
            examinePos -= movementInt;
            TileBase lastPushedTile = tilemap.GetTile(examinePos);
            if (lastPushedTile != null && lastPushedTile.name == "Blood")
            {
                tilemap.SetTile(examinePos, null);
                if (counter.Decrement() == 0)
                {
                    // WIN Level
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }

        Vector3Int from = examinePos - movementInt;
        while (from != origin)
        {
            tilemap.SetTile(examinePos, tilemap.GetTile(from));
            examinePos -= movementInt;
            from -= movementInt;
        }

        tilemap.SetTile(origin + movementInt, null);
    }
}
