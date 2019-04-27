using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] TileBase bloodTile;
    bool bleedTop;
    bool bleedRight;
    bool bleedBottom;
    bool bleedLeft;

    MapController mapController;
    Tilemap tilemap;

    void Start()
    {
        mapController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapController>();
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement = Vector3.left;
        }

        if (movement == Vector3.zero)
        {
            return;
        }

        Vector3Int originCellPos = tilemap.WorldToCell(transform.position);
        Vector3Int movementInt = Vector3Int.RoundToInt(movement);
        TileBase destinationTile = tilemap.GetTile(originCellPos + movementInt);
        if (mapController.Move(originCellPos, movementInt))
        {
            transform.position += Vector3.Scale(tilemap.cellSize, movement);
            Bleed(originCellPos, movementInt);
        }
        else if (TileUtility.IsSpike(destinationTile))
        {
            HitSpike(movementInt);
        }

    }

    void HitSpike(Vector3Int movementInt)
    {
        bleedTop = bleedTop || (movementInt == Vector3Int.up);
        bleedRight = bleedRight || (movementInt == Vector3Int.right);
        bleedBottom = bleedBottom || (movementInt == Vector3Int.down);
        bleedLeft = bleedLeft || (movementInt == Vector3Int.left);
    }

    void Bleed(Vector3Int position, Vector3Int movementInt)
    {
        bool doBleed = false;
        doBleed = doBleed || (bleedTop && (movementInt == Vector3Int.down));
        doBleed = doBleed || (bleedRight && (movementInt == Vector3Int.left));
        doBleed = doBleed || (bleedBottom && (movementInt == Vector3Int.up));
        doBleed = doBleed || (bleedLeft && (movementInt == Vector3Int.right));

        if (!doBleed) return;

        tilemap.SetTile(position, bloodTile);
    }
}
