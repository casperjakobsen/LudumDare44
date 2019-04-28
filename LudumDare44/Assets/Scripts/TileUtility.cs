using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileUtility
{
    public static bool CheckIfSpiked(TileBase tile, Vector3Int movementInt)
    {
        switch(tile.name){
            case "SpikeUp":
            case "SpikeUpBlood":
                if (movementInt == Vector3Int.down) return true;
                break;
            case "SpikeRight":
            case "SpikeRightBlood":
                if (movementInt == Vector3Int.left) return true;
                break;
            case "SpikeDown":
            case "SpikeDownBlood":
                if (movementInt == Vector3Int.up) return true;
                break;
            case "SpikeLeft":
            case "SpikeLeftBlood":
                if (movementInt == Vector3Int.right) return true;
                break;
        }
        return false;
    }
}
