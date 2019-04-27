﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        TileBase destinationTile = tilemap.GetTile(examinePos);
        if (destinationTile != null && destinationTile.name == "Hole")
        {
            examinePos -= movementInt;
            tilemap.SetTile(examinePos, null);
            if (bloodOnPath)
            {
                if (counter.Decrement() == 0)
                {
                    // WIN Level
                    print("WIN");
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
        return true;
    }
}