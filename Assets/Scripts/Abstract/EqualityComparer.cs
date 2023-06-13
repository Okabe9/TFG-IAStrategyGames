using System.Collections.Generic;
using UnityEngine;

public class EqualityComparer : IEqualityComparer<ResourceType>
{
    public bool Equals(ResourceType first, ResourceType other)
    {
        return true;
    }

    public int GetHashCode(ResourceType obj)
    {
        return obj.GetHashCode(); //Already an int
    }
}