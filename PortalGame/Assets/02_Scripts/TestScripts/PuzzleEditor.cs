using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

using UnityEngine;

public class PuzzleEditor : EditorWindow
{
    //TODO : �������� �ð��ʴ� �����̰� ����, ������ ������(bool ������·� üũ �� Ŭ���ϸ� �ش� ��ǥ�� ����) 2023.10.23
    //��Ż �� linerender enalbede false ���·� �޾Ƴ��� ���� �������� ������ �ݴ� ��Ż�� renderer �� ���ְ� ������Ʈ�� ���������� infinityMathf ���·� ����
    //OnSceneGUI ���������� ����

    public static Vector3 startPoint;
    public static Vector3 endPoint;
    public LineRenderer lineRenderer;
    public static GameObject targetObject;
    public static bool startPointClick = false;
    public static bool endPointClick = false;

    public static GameObject prefab;
    private static bool isObjectCreate = false;

    public static MapManager mapManager;
    public static PortalManager portalManager;

    public string createLaserBtnText = "�� ����";


    [MenuItem("Custom/PuzzleEditor")]
    public static void ShowWindow()
    {
        GetWindow<PuzzleEditor>("PuzzleEditor");
    }

    static PuzzleEditor()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }



    
    private static void OnSceneGUI(SceneView sceneView)
    {
        if (startPointClick || endPointClick || isObjectCreate)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (startPointClick)
                    {
                        startPoint = hit.transform.position;
                        targetObject = hit.transform.gameObject;
                        startPointClick = false;
                    }
                    else if (endPointClick)
                    {
                        endPoint = hit.transform.position;
                        endPointClick = false;
                    }
                    else if (isObjectCreate)
                    {
                        Vector3 normal = hit.normal;
                        float normalX = normal.x;
                        float normalY = normal.y;
                        float normalZ = normal.z;

                        TestMode.DebugLog("X = " + normalX);
                        TestMode.DebugLog("Z = " + normalZ);

                        Vector3 respawnPosition = new Vector3(hit.transform.position.x + normalX, hit.transform.position.y+normalY, hit.transform.position.z + normalZ);

                        GameObject createdObject = Instantiate(prefab, respawnPosition, Quaternion.identity);
                        if(mapManager != null && createdObject.GetComponent<CubeController>() != null) createdObject.GetComponent<CubeController>().mapManager = mapManager;
                        if (portalManager != null && createdObject.GetComponent<CubeController>() != null) createdObject.GetComponent<CubeController>().portalManager = portalManager;
                        if (prefab.transform.CompareTag("Laser"))
                        {
                            TestMode.DebugLog("���� ���");
                            createdObject.GetComponent<LaserController>().mapManager = mapManager;
                            createdObject.GetComponent<LaserController>().portalManager = portalManager;
                            createdObject.GetComponent<LaserController>().laserDirection = normal;

                        }
                    }


                }

                Event.current.Use();
            }
        }
        
    }

    private void OnGUI()
    {
        prefab = EditorGUILayout.ObjectField("Prefab",prefab,typeof(GameObject),true) as GameObject;



        mapManager = EditorGUILayout.ObjectField("MapManager", mapManager, typeof(MapManager), true) as MapManager;
        portalManager = EditorGUILayout.ObjectField("PortalManager", portalManager, typeof(PortalManager), true) as PortalManager;
        startPoint = EditorGUILayout.Vector3Field("startPoint", startPoint);
        endPoint = EditorGUILayout.Vector3Field("endPoint", endPoint);
        if (EditorGUILayout.Toggle("startPointClick", startPointClick))
        {
            startPointClick = true;
            endPointClick = false;
        }
        if (EditorGUILayout.Toggle("endPointClick", endPointClick))
        {
            startPointClick = false;
            endPointClick = true;
        }

        if (GUILayout.Button(createLaserBtnText) && prefab != null)
        {
            isObjectCreate = !isObjectCreate;
            if (isObjectCreate) createLaserBtnText = "�� ���� ���";
            else createLaserBtnText = "�� ����";
        }

        if (GUILayout.Button("������ ����"))
        {   
            if(targetObject.GetComponent<LineRenderer>() == null) lineRenderer = targetObject.AddComponent<LineRenderer>();
            else lineRenderer = targetObject.GetComponent<LineRenderer>();

            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            // �������� ���� ����
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
        if (GUILayout.Button("������ ����"))
        {
            if (targetObject.GetComponent<LineRenderer>() != null)
            {
                DestroyImmediate(targetObject.GetComponent<LineRenderer>());
            }
        }


    }
}

