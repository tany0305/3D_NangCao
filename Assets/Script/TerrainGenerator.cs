using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int depth = 20;
    public int width = 50;
    public int height = 50;
    public float scale = 20;
    public float offsetX = 100f;//dich chuyen ngang de tao mau
    public float offsetY = 100f;//dich chuyen doc de tao mau

    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;
        data = GenerateTerrain(data);

    }

    // Update is called once per frame
    void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;
        data = GenerateTerrain(data);

    }

    float[,] GenerateHeights(int width, int height)
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xcood = (float)x / width * scale + offsetX;
                float ycood = (float)y / height * scale + offsetY;
                heights[x, y] = Mathf.PerlinNoise(xcood, ycood);
            }
        }
        return heights;
    }

    public TerrainData GenerateTerrain(TerrainData data)
    {
        int width = data.heightmapResolution;
        int height = data.heightmapResolution;
        data.size = new Vector3(width, depth, height);
        data.SetHeights(0, 0, GenerateHeights(width, height));
        return data;
    }

}
