using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveForCharacterController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpSpeed = 10f;
    private float gravity = -20f;
    private float yVelocity = 0f;
    private CharacterController characterController;

    public Sprite normalCrossHair;
    public Sprite jumpCrossHair;

    public Image crossHair;
    // 시작
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // 업데이트
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3 (h , 0, v);
        moveDirection *= moveSpeed;
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        if (characterController.isGrounded)
        {
            crossHair.sprite = normalCrossHair;
            yVelocity = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                crossHair.sprite = jumpCrossHair;
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity * Time.deltaTime;
        moveDirection.y = yVelocity;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
