using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MapController : MonoBehaviour
{
    Tilemap tilemap;

    [SerializeField] Counter counter;

    [SerializeField] UnityEvent payEvent;
    [SerializeField] UnityEvent winEvent;

    [SerializeField] TileBase bloodTile;
    [SerializeField] TileBase bloodTileEntry;
    [SerializeField] float bloodEntryTime;

    [SerializeField] TileBase bloodSpikeUp;
    [SerializeField] TileBase bloodSpikeRight;
    [SerializeField] TileBase bloodSpikeDown;
    [SerializeField] TileBase bloodSpikeLeft;

    TileSwapper tileSwapper;

    void Start()
    {
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        tileSwapper = GameObject.FindGameObjectWithTag("TileSwapper").GetComponent<TileSwapper>();
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
            if (!TileUtility.IsBlood(tile)){
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
            if (lastPushedTile != null && TileUtility.IsBlood(lastPushedTile))
            {
                tilemap.SetTile(examinePos, null);
                payEvent.Invoke();
                if (counter.Decrement() == 0)
                {
                    winEvent.Invoke();
                    // WIN Level
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }

        tileSwapper.removeSwap(examinePos);
        Vector3Int from = examinePos - movementInt;
        while (from != origin)
        {
            tilemap.SetTile(examinePos, tilemap.GetTile(from));
            tileSwapper.moveSwap(from, examinePos);

            examinePos -= movementInt;
            from -= movementInt;
        }

        tilemap.SetTile(origin + movementInt, null);
    }

    public void AddBlood(Vector3Int position)
    {
        tilemap.SetTile(position, bloodTileEntry);
        tileSwapper.addTileSwap(bloodTile, Time.time + bloodEntryTime, position);
    }

    public void ApplyBloodToSpike(Vector3Int position)
    {
        TileBase examinedTile = tilemap.GetTile(position);
        if (examinedTile == null) return;

        switch(examinedTile.name){
            case "SpikeUp":
                tilemap.SetTile(position, bloodSpikeUp);
                break;
            case "SpikeRight":
                tilemap.SetTile(position, bloodSpikeRight);
                break;
            case "SpikeDown":
                tilemap.SetTile(position, bloodSpikeDown);
                break;
            case "SpikeLeft":
                tilemap.SetTile(position, bloodSpikeLeft);
                break;
        }
    }
}
