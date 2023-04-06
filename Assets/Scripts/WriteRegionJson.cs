using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegionBounds
{
    public string name;
    public float minX;
    public float minY;
    public float minZ;
    public float maxX;
    public float maxY;
    public float maxZ;
}

public class WriteRegionJson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ContextMenu("WriteJson")]
    void WriteJson()
    {
        int c = transform.childCount;
        
        string sOut = "";
        int totalNum = 0;
        for(int i = 0; i < c; ++i)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
            {
                totalNum++;
            }
        }

        RegionBounds[] bounds = new RegionBounds[totalNum];
        int numCount = 0;
        for(int i = 0; i < c; ++i)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
            {
                Collider col = transform.GetChild(i).gameObject.GetComponent<Collider>();
                
                bounds[numCount] = new RegionBounds();
                bounds[numCount].name = transform.GetChild(i).gameObject.name;
                bounds[numCount].minX = col.bounds.min.x;
                bounds[numCount].minY = col.bounds.min.y;
                bounds[numCount].minZ = col.bounds.min.z;
                bounds[numCount].maxX = col.bounds.max.x;
                bounds[numCount].maxY = col.bounds.max.y;
                bounds[numCount].maxZ = col.bounds.max.z;

                sOut = sOut + JsonUtility.ToJson(bounds[numCount]);
                numCount++;
            }
        }
        
        string filenameTxt2 = "region_bounds.txt";
		System.IO.File.WriteAllText(System.IO.Path.Combine(Application.streamingAssetsPath, filenameTxt2), sOut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
