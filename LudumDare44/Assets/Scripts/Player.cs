using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject bleedTopObj;
    [SerializeField] GameObject bleedRightObj;
    [SerializeField] GameObject bleedBottomObj;
    [SerializeField] GameObject bleedLeftObj;

    [SerializeField] bool canSpike = true;

    bool bleedTop;
    bool bleedRight;
    bool bleedBottom;
    bool bleedLeft;

    MapController mapController;
    Tilemap tilemap;

    [SerializeField] UnityEvent moveEvent;
    [SerializeField] UnityEvent hurtEvent;

    Animator anim;

    void Start()
    {
        mapController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapController>();
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();

        anim = GetComponent<Animator>();

        bleedTopObj.SetActive(false);
        bleedRightObj.SetActive(false);
        bleedBottomObj.SetActive(false);
        bleedLeftObj.SetActive(false);
    }

    void Update()
    {
        anim.ResetTrigger("Moved");
        anim.ResetTrigger("Spiked");

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

        anim.SetInteger("Horizontal", movementInt.x);
        anim.SetInteger("Vertical", movementInt.y);

        if (mapController.Move(originCellPos, movementInt))
        {
            anim.SetTrigger("Moved");
  
            moveEvent.Invoke();
            transform.position += Vector3.Scale(tilemap.cellSize, movement);
            Bleed(originCellPos, movementInt);
        }
        else if (canSpike && TileUtility.CheckIfSpiked(destinationTile, movementInt))
        {
            anim.SetTrigger("Spiked");

            hurtEvent.Invoke();
            HitSpike(movementInt);
            mapController.ApplyBloodToSpike(Vector3Int.RoundToInt(originCellPos + movementInt));
        }

    }

    void HitSpike(Vector3Int movementInt)
    {
        bleedTop = bleedTop || (movementInt == Vector3Int.up);
        bleedRight = bleedRight || (movementInt == Vector3Int.right);
        bleedBottom = bleedBottom || (movementInt == Vector3Int.down);
        bleedLeft = bleedLeft || (movementInt == Vector3Int.left);

        bleedTopObj.SetActive(bleedTop);
        bleedRightObj.SetActive(bleedRight);
        bleedBottomObj.SetActive(bleedBottom);
        bleedLeftObj.SetActive(bleedLeft);

    }

    void Bleed(Vector3Int position, Vector3Int movementInt)
    {
        bool doBleed = false;
        doBleed = doBleed || (bleedTop && (movementInt == Vector3Int.down));
        doBleed = doBleed || (bleedRight && (movementInt == Vector3Int.left));
        doBleed = doBleed || (bleedBottom && (movementInt == Vector3Int.up));
        doBleed = doBleed || (bleedLeft && (movementInt == Vector3Int.right));

        if (!doBleed) return;

        mapController.AddBlood(position);
    }
}
