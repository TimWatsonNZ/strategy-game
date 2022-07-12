using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ImprovementScript : MonoBehaviour, ITurnBased
{
    [SerializeField]
    Tile Tile;

    ImprovementTypeData _data;
    ImprovementType improvementType;

    int _fulfilledCapacity = 0;

    private List<IOnProduceListener> _listeners = new List<IOnProduceListener>();
    private Dictionary<ResourceType, int> _production = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, int> _requirement = new Dictionary<ResourceType, int>();
    
    private IResourceSource _resourceSource;
    public void Init(ImprovementTypeData data)
    {
        _data = data;
        _production = _data.Produces;
        _requirement = _data.Requires;
    }

    public void RegisterProvider(IResourceSource source)
    {
        _resourceSource = source;
    }

    int UseResources()
    {
        if (_resourceSource == null)
        {
            return 0;
        }
        var orders = _resourceSource.ProvideResources(_requirement, _data.ProductionCapacity);
        return orders;
    }

    public void OnProduceRegister(IOnProduceListener listener)
    {
        _listeners.Add(listener);
    }

    private void Produce(int ordersFulfilled)
    {
        foreach(var listener in _listeners)
        {
            listener.ProduceListener(Util.MultiplyResources(_production, ordersFulfilled));
        }
    }

    public void EndTurn()
    {
        var orders = UseResources();
        Produce(orders);
    }

    public void StartTurn()
    {
        throw new System.NotImplementedException();
    }
}
public enum ImprovementType {
    House,
    Farm,
    Woodcutter
}

public class OptionalResourceBoost
{
    ResourceType boostedResource;
    float boost;
    float requiredResource;
}


public class ImprovementTypeData
{
    public Dictionary<ResourceType, int> Produces = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> Requires = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, OptionalResourceBoost> Optional = new Dictionary<ResourceType, OptionalResourceBoost>();

    public TerrainType TerrainType;
    public int ProduceTime = 1;
    public int ProductionCapacity = 1;

    public ImprovementTypeData(
        Dictionary<ResourceType, int> produces,
        Dictionary<ResourceType, int> requires,
        Dictionary<ResourceType, OptionalResourceBoost> optional,
        int produceTime,
        int productionCapacity
    )
    {
        Produces = produces;
        Requires = requires;
        Optional = optional;
        ProduceTime = produceTime;
        ProductionCapacity = productionCapacity;
    }
}

public enum ResourceType
{
    Wood,
    Labour,
    Food
}

public enum TerrainType
{
    Grass,
    Forest
}

public interface IOnProduceListener
{
    void ProduceListener(Dictionary<ResourceType, int> resources);
}

