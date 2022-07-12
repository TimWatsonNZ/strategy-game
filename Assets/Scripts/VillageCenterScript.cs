using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VillageCenterScript : MonoBehaviour, IOnProduceListener, IResourceSource, ISelectable, ITurnBased
{
    [SerializeField]
    public Tile Tile;

    List<GameObject> _improvements = new List<GameObject>();
    public Dictionary<ResourceType, int> Resources { get; private set; } = new Dictionary<ResourceType, int>()
    {
        { ResourceType.Food, 10 },
        { ResourceType.Labour, 0 },
    };

    Dictionary<ResourceType, int> popRequirements = new Dictionary<ResourceType, int>()
    {
        {
            ResourceType.Food, 1
        }
    };

    public int Pops = 1;
    int housing = 10;
    float popGrowthCounter = 0;
    int popGrowthTime = 20;

    private bool _isSelected = false;

    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            OnSelect();
            _isSelected = value;
        }
    }

    public delegate void OnProduce(VillageCenterScript village);
    public event OnProduce OnSelectedVillageProduce;

    public void AddImprovement(GameObject improvement)
    {
        _improvements.Add(improvement);
        var script = improvement.GetComponent<ImprovementScript>();
        script.OnProduceRegister(this);
        script.RegisterProvider(this);
    }

    public void OnDeselect()
    {
        Debug.Log("Deselecting village");
        OnSelectedVillageProduce = null;
    }

    public void OnSelect()
    {

    }

    public void CollectTradeGoods(Dictionary<ResourceType, int> goods)
    {
        Util.AddResources(Resources, goods);
    }

    public void ProduceListener(Dictionary<ResourceType, int> resources)
    {
        Util.AddResources(Resources, resources);
        if (IsSelected)
        {
            OnSelectedVillageProduce?.Invoke(this);
        }
    }

    public void RegisterOnProduceListener(OnProduce listener)
    {
        OnSelectedVillageProduce += listener;
    }

    public int ProvideResources(Dictionary<ResourceType, int> request, int capacity)
    {
        Dictionary<ResourceType, int> capacityFulfilled = new Dictionary<ResourceType, int>();
        bool orderFulfilled = true;

        foreach(var r in request)
        {
            if (!Resources.ContainsKey(r.Key) || Resources[r.Key] < r.Value)
            {
                orderFulfilled = false;
            } else
            {
                capacityFulfilled.Add(r.Key, Resources[r.Key] / r.Value);
            }
        }

        //  Find lowest number in capacityFulfilled
        int orders = int.MaxValue;

        foreach(var r in capacityFulfilled)
        {
            if (r.Value < orders)
            {
                orders = r.Value;
            }
        }

        if (!orderFulfilled)
        {
            return 0;
        }
        orders = Math.Min(orders, capacity);
        Dictionary<ResourceType, int> orderResources = new Dictionary<ResourceType, int>();

        if (orderFulfilled)
        {
            foreach(var r in request)
            {
                orderResources.Add(r.Key, r.Value * orders);
                Resources[r.Key] -= r.Value * orders;
            }
        }

        return orders;
    }

    public void EndTurn()
    {
        var labourProduced = ProvideResources(popRequirements, Pops);
        Resources[ResourceType.Labour] += labourProduced;

        if (labourProduced > Pops / 2)
        {
            popGrowthCounter += labourProduced * 2;
        }

        if (popGrowthCounter > popGrowthTime)
        {
            popGrowthCounter = 0;
            Pops++;
        }
    }

    public void StartTurn()
    {
        throw new NotImplementedException();
    }
}

public interface IResourceSource
{
    int ProvideResources(Dictionary<ResourceType, int> request, int capacity);
}
