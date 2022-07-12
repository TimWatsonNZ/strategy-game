using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    public MapData GenerateMap()
    {
        var mapData = GenerateStaticMap();
        return mapData;
    }

    private MapData GenerateStaticMap()
    {
        var staticData = GetStaticMapData();
        var mapData = new MapData(staticData);

        return mapData;
    }

    private TileType[,] GetStaticMapData()
    {
        var mapData = new TileType[,]
        {
            {
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass
            },
            {
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass
            },
            {
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass
            },
            {
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass
            },
            {
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass
            },
            {
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass,
                TileType.Grass
            },
        };

        return mapData;
    }
}
