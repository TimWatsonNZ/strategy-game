using System.Collections.Generic;
using System.Linq;

public class TradeLink : ITurnBased
{
    List<TradePackage> PackagesInTransit = new List<TradePackage>();
    Dictionary<ResourceType, int> PackagesArrived = new Dictionary<ResourceType, int>();

    VillageCenterScript _destination;
    int _timeTaken;

    public TradeLink(VillageCenterScript destination, int timeTaken)
    {
        _destination = destination;
        _timeTaken = timeTaken;
    }

    public void EndTurn()
    {
        foreach(var package in PackagesInTransit)
        {
            package.EndTurn();
            if (package.TimeToTarget <= 0)
            {
                Util.AddResources(PackagesArrived, package.Resources);
            }
        }

        PackagesInTransit = PackagesInTransit
            .Where(package => package.TimeToTarget > 0)
            .ToList();

        _destination.CollectTradeGoods(PackagesArrived);
        PackagesArrived.Clear();
    }

    public void AddGoods(Dictionary<ResourceType, int> goods)
    {
        PackagesInTransit.Add(new TradePackage(goods, _timeTaken));
    }

    public void StartTurn()
    {
        throw new System.NotImplementedException();
    }
}


class TradePackage : ITurnBased
{
    public Dictionary<ResourceType, int> Resources;
    public int TimeToTarget { get; private set; }

    public TradePackage(Dictionary<ResourceType, int> resources, int timeToTarget)
    {
        Resources = resources;
        this.TimeToTarget = timeToTarget;
    }

    public void EndTurn()
    {
        TimeToTarget--;
    }

    public void StartTurn()
    {
        throw new System.NotImplementedException();
    }
}