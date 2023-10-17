using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCursorMode : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
