using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    Mesh waterPlaneMesh;
    public TerrainGenerator TerrainGenerator;
    private Color waterPlaneColor;
    private float relativeWaterHeight, minTerrainHeight, maxTerrainHeight;
    private int xSize, zSize;
    private float waterMinHeight = 0;
    private float waterMaxHeight = 0;


    void Start()
    {
        waterPlaneMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = waterPlaneMesh;
    }

    void Update()
    {
        UpdateTerrainGeneratorParameters();
        CreateWaterPlaneShape();
    }

    void UpdateTerrainGeneratorParameters()
    {
        relativeWaterHeight = TerrainGenerator.relativeWaterHeight;
        xSize = TerrainGenerator.terrainLenght;
        zSize = TerrainGenerator.terrainWidth;
        minTerrainHeight = TerrainGenerator.getMinTerrainheight();
        maxTerrainHeight = TerrainGenerator.getMaxTerrainheight();
        waterPlaneColor = TerrainGenerator.waterPlaneColor;
    }

    void CreateWaterPlaneShape()
    {
        Vector3[] waterVertices = new Vector3[4];

        if (TerrainGenerator.lockWaterPlaneOnCurrentHeights) {
            waterMinHeight = minTerrainHeight;
            waterMaxHeight = maxTerrainHeight;
            TerrainGenerator.lockWaterPlaneOnCurrentHeights = false;
        }
        
        if (relativeWaterHeight != 0) {
            float absoluteWaterHeight = Mathf.Lerp(waterMinHeight, waterMaxHeight, relativeWaterHeight);
            waterVertices[0] = new Vector3(1, absoluteWaterHeight, 1);
            waterVertices[1] = new Vector3(xSize+1, absoluteWaterHeight, 1);
            waterVertices[2] = new Vector3(1, absoluteWaterHeight, zSize+1);
            waterVertices[3] = new Vector3(xSize+1, absoluteWaterHeight ,zSize+1);

            int[] waterTriangles = new int[6];
            waterTriangles[0] = 2;
            waterTriangles[1] = 1;
            waterTriangles[2] = 0;
            waterTriangles[3] = 3;
            waterTriangles[4] = 1;
            waterTriangles[5] = 2;

            Color[] waterColors = new Color[4];
            for (int i = 0; i < waterColors.Length; i++)
                waterColors[i] = waterPlaneColor;
            
            waterPlaneMesh.Clear();
            waterPlaneMesh.vertices = waterVertices;
            waterPlaneMesh.triangles = waterTriangles;
            waterPlaneMesh.colors = waterColors;
            waterPlaneMesh.RecalculateNormals();
        } else {
            waterPlaneMesh.Clear();
        }
    }
}
