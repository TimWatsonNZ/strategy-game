using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerScript : MonoBehaviour
{
    public delegate void EscapePressed();
    public event EscapePressed OnEscapePressed;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapePressed?.Invoke();
        }
    }

    public void RegisterOnEscape(EscapePressed listener)
    {
        OnEscapePressed += listener;
    }
}
