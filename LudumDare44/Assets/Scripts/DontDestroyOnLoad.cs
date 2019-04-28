using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] string id;
    public static List<string> instantiatedIDs = new List<string>();
    void Awake()
    {
        if (instantiatedIDs.Contains(id))
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instantiatedIDs.Add(id);
        }
    }
}
