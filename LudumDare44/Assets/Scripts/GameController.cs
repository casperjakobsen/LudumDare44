﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    Tilemap tilemap;

    [SerializeField] List<Counter> counters = new List<Counter>();
    [SerializeField] Dictionary<Vector3Int, Counter> posToCounter = new Dictionary<Vector3Int, Counter>();

    [SerializeField] UnityEvent payEvent;
    [SerializeField] UnityEvent payedUpEvent;
    [SerializeField] UnityEvent winEvent;

    [SerializeField] TileBase holeTile;
    [SerializeField] TileBase holeWithBlood;
    [SerializeField] TileBase bloodTile;
    [SerializeField] TileBase bloodTileEntry;

    [SerializeField] float tileAnimationTime;

    [SerializeField] TileBase bloodSpikeUp;
    [SerializeField] TileBase bloodSpikeRight;
    [SerializeField] TileBase bloodSpikeDown;
    [SerializeField] TileBase bloodSpikeLeft;

    [SerializeField] Animator screenFader;

    TileSwapper tileSwapper;

    public static int lastLevel = 4;

    void Start()
    {
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        tileSwapper = GameObject.FindGameObjectWithTag("TileSwapper").GetComponent<TileSwapper>();

        foreach (Counter counter in counters)
        {
            posToCounter.Add(tilemap.WorldToCell(counter.transform.Find("Position").position), counter);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public bool Move(Vector3Int origin, Vector3Int movementInt)
    {
        Vector3Int examinePos = origin + movementInt;
        bool bloodOnPath = false;
        while (true)
        {
            TileBase tile = tilemap.GetTile(examinePos);
            if (tile == null || (bloodOnPath && TileUtility.IsHole(tile))){
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
        if (destinationTile != null && TileUtility.IsHole(destinationTile))
        {
            examinePos -= movementInt;
            TileBase lastPushedTile = tilemap.GetTile(examinePos);
            if (lastPushedTile != null && TileUtility.IsBlood(lastPushedTile))
            {
                tilemap.SetTile(examinePos, null);
                tilemap.SetTile(destination, holeWithBlood);

                tileSwapper.removeSwap(destination);
                tileSwapper.addTileSwap(holeTile, Time.time + tileAnimationTime, destination);

                payEvent.Invoke();
                DecrementCounter(destination);
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

    void DecrementCounter(Vector3Int position)
    {
        if (!posToCounter.ContainsKey(position)) return;
        Counter counter = posToCounter[position];

        if (counter.Value() == 1)
        {
            payedUpEvent.Invoke();
            posToCounter.Remove(position);
        }
        if (counter.Value() > 0) counter.Decrement();

        if (posToCounter.Count == 0)
        {
            int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentLevelIndex < lastLevel)
            {
                PlayerPrefs.SetInt("Progress", currentLevelIndex + 1);
            }

            // Level Complete
            winEvent.Invoke();
            screenFader.SetBool("Fading", true);
        }
    }

    public void TryAddBlood(Vector3Int position)
    {
        if (tilemap.GetTile(position) != null) return;

        tilemap.SetTile(position, bloodTileEntry);
        tileSwapper.addTileSwap(bloodTile, Time.time + tileAnimationTime, position);
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
