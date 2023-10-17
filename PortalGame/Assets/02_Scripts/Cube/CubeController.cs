using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private MoveCube playerMoveCubeScript;
    public bool isCollision = false;
    public MapManager mapManager;

    private string wallLayer = "Wall";
    private string portalLayer = "Portal";

    private bool hasCollided = false;

    private GameObject player;
    private GameObject targetObject;

    public PortalManager portalManager;

    private void Start()
    {
        playerMoveCubeScript = GameObject.FindWithTag("Player").GetComponentInChildren<MoveCube>();
        player = GameObject.FindWithTag("Player");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer(portalLayer)))
        {
            hasCollided = true;
            if (collision.transform.CompareTag("PrevPortal"))
            {
                targetObject = portalManager.portal2_2;
            }
            else if (collision.transform.CompareTag("NextPortal"))
            {
                targetObject = portalManager.portal2_1;
            }

            Vector3 forwardDirection = targetObject.transform.forward;
            Vector3 rightDirection = targetObject.transform.right;

            float xForward = forwardDirection.x;
            float yForward = forwardDirection.y;
            float zForward = forwardDirection.z;

            float xRight = rightDirection.x;
            float yRight = rightDirection.y;
            float zRight = rightDirection.z;
            player.GetComponent<CharacterController>().enabled = false;
            TestMode.DebugLog(targetObject.transform.forward);
            if (Mathf.Abs(xForward) > Mathf.Abs(yForward) && Mathf.Abs(xForward) > Mathf.Abs(zForward))
            {
                //TODO : 이동되기전 POST PROCESSING으로 화면 검게 변경 해야됨(2023.10.10)

                TestMode.DebugLog("앞");
                //TestMode.DebugLog(targetObject.transform.forward);
                if (targetObject.transform.position.z > targetObject.GetComponentInChildren<PortalController>().hitBlockPosition.z)
                {
                    player.transform.position = new Vector3(targetObject.transform.position.x, 1f, targetObject.transform.position.z + 2);
                    Camera.main.transform.GetComponent<CameraLookForMouse>().rotationY = 0;
                }
                else
                {
                    player.transform.position = new Vector3(targetObject.transform.position.x, 1f, targetObject.transform.position.z - 2);
                    Camera.main.transform.GetComponent<CameraLookForMouse>().rotationY = 180f;
                }

            }
            else
            {
                if (targetObject.transform.position.x > targetObject.GetComponentInChildren<PortalController>().hitBlockPosition.x)
                {
                    Camera.main.transform.GetComponent<CameraLookForMouse>().rotationY = 90f;
                    player.transform.position = new Vector3(targetObject.transform.position.x + 2, 1f, targetObject.transform.position.z);
                }
                else
                {
                    Camera.main.transform.GetComponent<CameraLookForMouse>().rotationY = -90f;
                    player.transform.position = new Vector3(targetObject.transform.position.x - 2, 1f, targetObject.transform.position.z);

                }
            }
            player.GetComponent<CharacterController>().enabled = true;
            hasCollided = false;
        }
        else if (playerMoveCubeScript.tempObject != null && collision.gameObject.layer.Equals(LayerMask.NameToLayer(wallLayer)))
        {
            TestMode.DebugLog("충돌됨2!!");
            isCollision = true;
            playerMoveCubeScript.tempObject = null;
        }
    }
    public void Update()
    {
        
        if (isCollision && !Input.GetKey(KeyCode.E))
        {
            isCollision = false;
        }

        if (transform.position.y < mapManager.startPosition.y)
        {
            transform.position = new Vector3(transform.position.x, mapManager.startPosition.y, transform.position.z);
        }
        
    }
}
