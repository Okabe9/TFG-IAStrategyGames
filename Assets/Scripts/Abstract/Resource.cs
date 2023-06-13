using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public abstract class Resource : MonoBehaviour
{
    public int amount = 100;
    public ResourceType Type { get; set; }
    private void Update()
    {
        if (amount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
[System.Serializable]
public enum ResourceType
{
    Food,
    Stone
}

[System.Serializable]
public class Inventory
{
    public int capacity;
    [SerializeField]
    public SerializableDictionary<ResourceType, float> Refined = new SerializableDictionary<ResourceType, float>(new EqualityComparer());

    [SerializeField]
    public SerializableDictionary<ResourceType, float> Unrefined = new SerializableDictionary<ResourceType, float>(new EqualityComparer());

    public Inventory(int capacity)
    {
        this.capacity = capacity;
        Refined.Add(ResourceType.Food, 0);
        Refined.Add(ResourceType.Stone, 0);
        Unrefined.Add(ResourceType.Food, 0);
        Unrefined.Add(ResourceType.Stone, 0);
    }

    public void SumInventory(Inventory i)
    {
        Refined[ResourceType.Food] += i.Refined[ResourceType.Food];
        Unrefined[ResourceType.Food] += i.Unrefined[ResourceType.Food];
        Refined[ResourceType.Stone] += i.Refined[ResourceType.Stone];
        Unrefined[ResourceType.Stone] += i.Unrefined[ResourceType.Stone];
    }
}