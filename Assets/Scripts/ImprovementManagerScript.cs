using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ImprovementManagerScript : MonoBehaviour, ITurnBased
{
    [SerializeField]
    Tilemap ImprovementTiles;

    [SerializeField]
    GameObject Improvement;

    [SerializeField]
    Tile Farm;

    [SerializeField]
    Tile Woodcutter;

    [SerializeField]
    GameObject Center;

    Dictionary<Vector2Int, GameObject> Improvements = new Dictionary<Vector2Int, GameObject>();
    Dictionary<Vector2Int, GameObject> Centers = new Dictionary<Vector2Int, GameObject>();

    Dictionary<ImprovementType, ImprovementTypeData> Specs = new Dictionary<ImprovementType, ImprovementTypeData>()
    {
        {
            ImprovementType.Farm,
            new ImprovementTypeData(
                 new Dictionary<ResourceType, int>()
                {
                    {  ResourceType.Food, 2 }
                },
                new Dictionary<ResourceType, int>(){
                    { ResourceType.Labour, 1 }
                },
                new Dictionary<ResourceType, OptionalResourceBoost>(),
                10,
                2
            )
        },{
            ImprovementType.Woodcutter,
            new ImprovementTypeData(
                 new Dictionary<ResourceType, int>()
                {
                    {  ResourceType.Wood, 2 }
                },
                new Dictionary<ResourceType, int>(){
                    { ResourceType.Labour, 1 }
                },
                new Dictionary<ResourceType, OptionalResourceBoost>(),
                10,
                2
            )
        }
    };

    public VillageCenterScript SelectCenter(Vector3Int pos)
    {
        if (Centers.ContainsKey((Vector2Int)pos))
        {
            var center = Centers[(Vector2Int)pos].GetComponent<VillageCenterScript>();
            center.IsSelected = true;
            return center;
        }
        return null;
    }

    public void DeselectCenter(Vector3Int pos)
    {
        if (Centers.ContainsKey((Vector2Int)pos))
        {
            Centers[(Vector2Int)pos].GetComponent<VillageCenterScript>().IsSelected = false;
        }
    }

    protected GameObject BuildImprovement(ImprovementType type)
    {
        if (Specs.ContainsKey(type))
        {
            var obj = Instantiate(Improvement);
            var script = obj.GetComponent<ImprovementScript>();
            script.Init(Specs[type]);
            return obj;
        }
        return null;
    }

    public void AddCenterToGrid(Vector3Int pos)
    {
        if (Centers.ContainsKey((Vector2Int)pos))
        {
            return;
        }
        var center = Instantiate(Center, pos, Quaternion.identity);

        ImprovementTiles.SetTile(pos, center.GetComponent<VillageCenterScript>().Tile);
        Centers.Add((Vector2Int)pos, center);
    }

    public void AddImprovementToGrid(ImprovementType type, Vector3Int pos, VillageCenterScript selectedVillage)
    {
        if (Improvements.ContainsKey((Vector2Int)pos))
        {
            return;
        }

         if(selectedVillage == null)
        {
            return;
        }

        var distanceToCenter = Vector3Int.Distance(pos, Vector3Int.FloorToInt(selectedVillage.transform.position));

        if (distanceToCenter >= 2)
        {
            return;
        }

        var improvement = BuildImprovement(type);
        if (!improvement)
        {
            throw new System.Exception("Couldnt build improvement.");
        }

        ImprovementTiles.SetTile(pos, MapImprovementTypeToTile(type));
        Improvements.Add((Vector2Int)pos, improvement);

        selectedVillage.AddImprovement(improvement);
    }

    private TileBase MapImprovementTypeToTile(ImprovementType type)
    {
        var map = new Dictionary<ImprovementType, TileBase>()
        {
            { ImprovementType.Farm, Farm },
            { ImprovementType.Woodcutter, Woodcutter },
        };

        return map[type];
    }

    public void EndTurn()
    {
        foreach (var i in Centers)
        {
            i.Value.GetComponent<VillageCenterScript>().EndTurn();
        }

        foreach (var i in Improvements)
        {
            i.Value.GetComponent<ImprovementScript>().EndTurn();
        }
    }

    public void StartTurn()
    {
        throw new System.NotImplementedException();
    }
}
