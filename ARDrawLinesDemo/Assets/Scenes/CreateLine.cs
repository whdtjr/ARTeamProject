using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour
{

    private Camera cam; // 카메라
    public GameObject LinePrefab;  
    private LineRenderer line;

    private Vector3 mousePos;   // 마우스 포지션
    private Vector3 newPos;     // 스크린의 포지션을 월드로 변경
    public List<Vector3> linePositions = new List<Vector3>();   // 변경된 포지션들

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            mousePos.z = cam.nearClipPlane + 1f;
            newPos = cam.ScreenToWorldPoint(mousePos);
            linePositions.Add(newPos);

            GameObject obj = Instantiate(LinePrefab);
            line = obj.GetComponent<LineRenderer>();

            line.positionCount = 1; // 시작점 
            line.SetPosition(0, linePositions[0]);
        }
        else if (Input.GetMouseButton(0))
        {
            mousePos = Input.mousePosition;
            mousePos.z = cam.nearClipPlane + 1f;
            newPos = cam.ScreenToWorldPoint(mousePos);
            linePositions.Add(newPos);

            line.positionCount++;   // 그 이후 점 
            line.SetPosition(line.positionCount - 1, linePositions[line.positionCount - 1]);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            linePositions.Clear();
        }
    }
}
