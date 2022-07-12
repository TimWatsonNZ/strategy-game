using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScript : MonoBehaviour
{
    [SerializeField]
    Tilemap Terrain;

    [SerializeField]
    Tilemap TerrainFeatures;

    [SerializeField]
    Tilemap EffectTiles;

    [SerializeField]
    private Tile grassTile;

    [SerializeField]
    GameObject ImprovementManager;

    [SerializeField]
    public Tile SelectedTile;

    private Vector3Int _selectedPosition;
    private ImprovementManagerScript _builder;

    VillageCenterScript _selectedVillage;

    public delegate void VillageSelected(VillageCenterScript village);
    public event VillageSelected OnVillageClicked;

    public ClickState ClickState { get; set; }

    void Active()
    {

    }

    private void Start()
    {
        _builder = ImprovementManager.GetComponent<ImprovementManagerScript>();

        var mapGenerator = new MapGenerator();
        var mapdata = mapGenerator.GenerateMap();

        foreach (var data in mapdata.Data)
        {
            Terrain.SetTile(data.Key, MapTileTypeToTile(data.Value));
        }

        _builder.AddCenterToGrid(new Vector3Int(0, 0, 0));
        _builder.AddCenterToGrid(new Vector3Int(2, 0, 0));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            SelectTile(pos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (ClickState == ClickState.CreateWoodcutter)
            {
                _builder.AddImprovementToGrid(ImprovementType.Woodcutter, pos, _selectedVillage);
            }
            if (ClickState == ClickState.CreateVillage)
            {
                _builder.AddCenterToGrid(pos);
            }
            if (ClickState == ClickState.CreateFarm)
            {
                _builder.AddImprovementToGrid(ImprovementType.Farm, pos, _selectedVillage);
            }
        }
    }

    private void SelectTile(Vector3Int pos)
    {
        if (_selectedPosition != null)
        {
            EffectTiles.SetTile(_selectedPosition, null);
        }
        EffectTiles.SetTile(pos, SelectedTile);
        _selectedPosition = pos;
        var center = _builder.SelectCenter(pos);
        if (center != null)
        {
            OnVillageClicked?.Invoke(center);
            _selectedVillage = center;
        }
    }

    public void Deselect()
    {
        if (_selectedPosition != null)
        {
            EffectTiles.SetTile(_selectedPosition, null);
            _builder.DeselectCenter(_selectedPosition);
        }
    }

    public void RegisterOnVillageSelected(VillageSelected listener)
    {
        OnVillageClicked += listener;
    }

    private TileBase MapTileTypeToTile(TileType tileType)
    {
        var map = new Dictionary<TileType, TileBase>()
            {
                { TileType.Grass, grassTile },
            };

        return map[tileType];
    }
}
public class MapData
{
    public Dictionary<Vector3Int, TileType> Data;

    private int _width;
    private int _height;

    public MapData(TileType[,] generatedData)
    {
        this._width = generatedData.GetLength(1);
        this._height = generatedData.GetLength(0);
        Data = new Dictionary<Vector3Int, TileType>();

        for (int rowIndex = 0; rowIndex < generatedData.GetLength(0); rowIndex++)
        {
            for (int colIndex = 0; colIndex < generatedData.GetLength(1); colIndex++)
            {
                Data.Add(new Vector3Int(colIndex - _width / 2, rowIndex - _height / 2, 0), generatedData[rowIndex, colIndex]);
            }
        }
    }
}

public enum TileType
{
    Grass
}