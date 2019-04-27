using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileUtility
{
    public static bool IsSpike(TileBase tile)
    {
        switch(tile.name){
            case "SpikeUp":
            case "SpikeRight":
            case "SpikeDown":
            case "SpikeLeft":
                return true;
        }
        return false;
    }
}
