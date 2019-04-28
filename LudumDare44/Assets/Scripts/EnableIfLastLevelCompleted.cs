using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIfLastLevelCompleted : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Progress") && PlayerPrefs.GetInt("Progress") == GameController.lastLevel)
        {
            enabled = true;
        }
        else
        {
            enabled = false;
        }
    }
}
