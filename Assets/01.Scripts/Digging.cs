using System;
using UnityEngine;
 
public sealed class Digging : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The width of the brush.")]
    private int brushWidth;
 
    [SerializeField]
    [Tooltip("The height of the brush.")]
    private int brushHeight;
 
    [SerializeField]
    [Tooltip("The strength of the brush.")]
    private float strength = 0.05f;
    private Camera _camera;
    private Terrain _targetTerrain;
    private TerrainData _targetTerrainData;
 
    private float _sampledHeight;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) ||
            !Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo)) return;

        if (hitInfo.transform.TryGetComponent(out Terrain terrain))
        {
            _targetTerrain = terrain;

            _targetTerrainData = _targetTerrain.terrainData;
        }
        else
        {
            return;
        }
        
        LowerTerrain(hitInfo.point);
    }

    private Vector3 WorldToTerrainPosition(Vector3 worldPosition)
    {
        Vector3 terrainPosition = worldPosition - _targetTerrain.GetPosition();
 
        Vector3 terrainSize = _targetTerrainData.size;
 
        int heightmapResolution = _targetTerrainData.heightmapResolution;
 
        terrainPosition = new Vector3(terrainPosition.x / terrainSize.x, terrainPosition.y / terrainSize.y, 
            terrainPosition.z / terrainSize.z);
 
        return new Vector3(terrainPosition.x * heightmapResolution, 0.0f, terrainPosition.z * heightmapResolution);
    }
 
    private (int, int) ClampBrushPosition(Vector3 brushWorldPosition)
    {
        Vector3 terrainPosition = WorldToTerrainPosition(brushWorldPosition);
 
        int heightmapResolution = _targetTerrainData.heightmapResolution;
 
        int clampedBrushX = (int)Math.Min(Math.Max((terrainPosition.x - brushWidth * 0.5f), 0), heightmapResolution);
        int clampedBrushY = (int)Math.Min(Math.Max((terrainPosition.z - brushHeight * 0.5f), 0), heightmapResolution);
 
        return (clampedBrushX, clampedBrushY);
    }
 
    private (int, int) ClampBrushSize(int brushX, int brushY)
    {
        int heightmapResolution = _targetTerrainData.heightmapResolution;
 
        int clampedBrushWidth = Math.Min(brushWidth, heightmapResolution - brushX);
        int clampedBrushHeight = Math.Min(brushHeight, heightmapResolution - brushY);
 
        return (clampedBrushWidth, clampedBrushHeight);
    }
 
    private void LowerTerrain(Vector3 brushWorldPosition)
    {
        (int clampedBrushX, int clampedBrushY) = ClampBrushPosition(brushWorldPosition);
 
        (int clampedBrushWidth, int clampedBrushHeight) = ClampBrushSize(clampedBrushX, clampedBrushY);
 
        float[,] heights = _targetTerrainData.GetHeights(clampedBrushX, clampedBrushY, clampedBrushWidth, clampedBrushHeight);
 
        float decrement = strength * Time.deltaTime;
 
        for (int y = 0; y < clampedBrushHeight; y++)
        {
            for (int x = 0; x < clampedBrushWidth; x++)
            {
                heights[y, x] -= decrement;
            }
        }
 
        _targetTerrainData.SetHeights(clampedBrushX, clampedBrushY, heights);
    }
}
 