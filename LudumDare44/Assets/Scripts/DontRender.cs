using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRender : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
