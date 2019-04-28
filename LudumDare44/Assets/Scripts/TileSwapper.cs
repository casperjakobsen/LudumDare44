using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSwapper : MonoBehaviour
{
    Dictionary<Vector3Int, TileSwapInfo> positionToSwapInfo = new Dictionary<Vector3Int, TileSwapInfo>();
    List<TileSwapInfo> swaps = new List<TileSwapInfo>();

    Tilemap tilemap;

    private void Start()
    {
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    void Update()
    {
        while (true)
        {
            if (swaps.Count == 0) return;
            TileSwapInfo swap = swaps[0];

            if (swap.swapTime <= Time.time)
            {
                positionToSwapInfo.Remove(swap.position);
                swaps.RemoveAt(0);
                tilemap.SetTile(swap.position, swap.swapTile);
            }
            else
            {
                break;
            }
        }
    }

    public void addTileSwap(TileBase swapTile, float swapTime, Vector3Int position)
    {
        TileSwapInfo info = new TileSwapInfo(swapTile, swapTime, position);
        swaps.Add(info);
        positionToSwapInfo.Add(position, info);
    }

    public void moveSwap(Vector3Int from, Vector3Int to)
    {
        if (!positionToSwapInfo.ContainsKey(from)) return;
        TileSwapInfo swap = positionToSwapInfo[from];

        removeSwap(swap.position);
        addTileSwap(swap.swapTile, swap.swapTime, to);
    }

    public void removeSwap(Vector3Int position)
    {
        if (!positionToSwapInfo.ContainsKey(position)) return;
        swaps.Remove(positionToSwapInfo[position]);
        positionToSwapInfo.Remove(position);
    }
}

public struct TileSwapInfo
    {
        public TileBase swapTile;
        public float swapTime;
        public Vector3Int position;

        public TileSwapInfo(TileBase swapTile, float swapTime, Vector3Int position)
        {
            this.swapTile = swapTile;
            this.swapTime = swapTime;
            this.position = position;
        }
    }
