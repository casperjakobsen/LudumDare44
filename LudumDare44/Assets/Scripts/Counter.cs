using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    [SerializeField] int count;

    TextMeshProUGUI textMesh;
    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = count.ToString();
    }
       
    public int Decrement()
    {
        count--;
        textMesh.text = count.ToString();
        return count;
    }
}
