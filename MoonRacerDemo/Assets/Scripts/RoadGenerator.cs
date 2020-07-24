using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoadGenerator : MonoBehaviour
{
    public Transform StartTransform;
    public GameObject RoadPrefab;
    public GameObject RoadPrefab1;

    public float OffsetZ=10;
    public int SegmentCount = 10;

    public bool CreateRoad = false;
    public bool CleanRoad = false;

    private List<GameObject> RoadsList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnValidate()
    {
        if(CreateRoad)
        {
            GenerateRoad();
            CreateRoad = false;
        }

        if(CleanRoad)
        {
            CleanRoads();
            CleanRoad = false;
        }
    }

    void CleanRoads()
    {
        int count = RoadsList.Count;

        for(int i=0;i<count;i++)
        {
            GameObject obj = RoadsList[0];
            RoadsList.RemoveAt(0);
            DestroyImmediate(obj);
        }
    }

    public void GenerateRoad()
    {
        Vector3 initPos = StartTransform.position;

        Vector3 pos = initPos;

        for (int i=0;i<SegmentCount;i++)
        {
            pos.z += OffsetZ;
            GameObject segment;
            if(i%2==0)
            {
                segment = RoadPrefab;
            }
            else
            {
                segment = RoadPrefab1;
            }

            GameObject newRoad= Instantiate(segment, pos, Quaternion.identity);
            RoadsList.Add(newRoad);
        }
    }
}
