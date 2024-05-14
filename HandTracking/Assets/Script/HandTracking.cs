using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public UDPReceive udpReceive; // from datamanager
    public GameObject[] handPoints;
    float scaleNum = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceive.data;
        data = data.Remove(0,1); // delete [
        data = data.Remove(data.Length - 1,1); // delete ]
        string[] points = data.Split(",");
        for (int i = 0; i < 21; i++)
        {
            float point_x = 5- float.Parse(points[i * 3])/100 * scaleNum;
            float point_y = float.Parse(points[i*3 + 1])/100 * scaleNum;
            float point_z = float.Parse(points[i*3 + 2])/100 * scaleNum;
            handPoints[i].transform.localPosition = new Vector3(point_x, point_y, point_z);
        }
    }
}
