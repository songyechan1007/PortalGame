using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookForMouse : MonoBehaviour
{
    public float sensitivity = 500f;
    public float rotationX;
    public float rotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // 업데이트
    void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        rotationX += y * sensitivity * Time.deltaTime;
        rotationY += x * sensitivity * Time.deltaTime;

        transform.localEulerAngles = new Vector3(-rotationX , rotationY,0);
    }
}
