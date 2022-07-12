using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void AddResources(Dictionary<ResourceType, int> A, Dictionary<ResourceType, int> B)
    {
        foreach(var r in B)
        {
            if (A.ContainsKey(r.Key))
            {
                A[r.Key] += r.Value;
            } else
            {
                A.Add(r.Key, r.Value);
            }
        }
    }

    public static Dictionary<ResourceType, int> MultiplyResources(Dictionary<ResourceType, int> resources, int scale)
    {
        var dict = new Dictionary<ResourceType, int>();
        foreach(var r in resources)
        {
            dict.Add(r.Key, r.Value * scale);
        }
        return dict;
    }
}
