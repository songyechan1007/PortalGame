using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Vector3 laserDirection;

    private LineRenderer lineRenderer;
    private RaycastHit hit;

    public MapManager mapManager;
    public PortalManager portalManager;
    GameObject targetObject;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
    }

    void Update()
    {

        lineRenderer.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, laserDirection, out hit))
        {
            lineRenderer.SetPosition(1, hit.point);
            if (hit.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Portal")))
            {
                if (hit.transform.CompareTag("PrevPortal"))
                {
                    targetObject = portalManager.portal2_2;
                }
                else if (hit.transform.CompareTag("NextPortal"))
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

                targetObject.GetComponentInChildren<PortalController>().isLaserCollisied = true;
                if (Mathf.Abs(xForward) > Mathf.Abs(yForward) && Mathf.Abs(xForward) > Mathf.Abs(zForward))
                {
                    if (targetObject.transform.position.z > targetObject.GetComponentInChildren<PortalController>().hitBlockPosition.z)
                    {
                        targetObject.GetComponentInChildren<PortalController>().laserDirection = transform.forward;
                    }
                    else if (targetObject.transform.position.z < targetObject.GetComponentInChildren<PortalController>().hitBlockPosition.z)
                    {
                        targetObject.GetComponentInChildren<PortalController>().laserDirection = -transform.forward;
                    }
                }
                else
                {
                    if (targetObject.transform.position.x > targetObject.GetComponentInChildren<PortalController>().hitBlockPosition.x)
                    {
                        targetObject.GetComponentInChildren<PortalController>().laserDirection = transform.right;
                    }
                    else if (targetObject.transform.position.x < targetObject.GetComponentInChildren<PortalController>().hitBlockPosition.x)
                    {
                        targetObject.GetComponentInChildren<PortalController>().laserDirection = -transform.right;
                    }
                }
            }
            else
            {
                if (targetObject != null)
                {
                    TestMode.Clear();
                    TestMode.DebugLog("여기들어옴");
                    targetObject.GetComponentInChildren<PortalController>().isLaserCollisied = false;
                    targetObject = null;
                }
            }
        }
        else
        {
            // 레이가 충돌하지 않으면 원하는 최대 거리로 설정
            float maxDistance = (mapManager.roomHeight * mapManager.roomWidth) /2;
            Vector3 endPoint = transform.position + laserDirection * maxDistance;
            lineRenderer.SetPosition(1, endPoint);
        }
    }
}
