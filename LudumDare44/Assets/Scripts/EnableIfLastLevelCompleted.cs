using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIfLastLevelCompleted : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(PlayerPrefs.HasKey("Progress") && PlayerPrefs.GetInt("Progress") == GameController.lastLevel);
    }
}
