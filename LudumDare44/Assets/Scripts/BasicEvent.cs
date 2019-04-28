using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicEvent : MonoBehaviour
{
    [SerializeField] UnityEvent basicEvent;

    public void InvokeEvent()
    {
        basicEvent.Invoke();
    }
}
