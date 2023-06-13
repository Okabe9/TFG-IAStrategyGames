using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class City
{
    public int Id = 0;
    public int teamId;
    public List<BuildableTile> Tiles = new List<BuildableTile>();
    public Inventory Stash { get; set; }


    public City(int id)
    {
        Id = id;
        Tiles = new List<BuildableTile>();
        Stash = new Inventory(25000);

    }


    public void AddBuilding(BuildableTile tile)
    {

        Tiles.Add(tile);
        tile.building.cityId = Id;  // Update the city ID of the building
        UpdateInventory(tile.building);
    }

    private void UpdateInventory(Building b)
    {
        Stash.capacity += 200;
        Stash.Refined[ResourceType.Food] += b.Stash.Refined[ResourceType.Food];
        Stash.Unrefined[ResourceType.Food] += b.Stash.Unrefined[ResourceType.Food];
        Stash.Refined[ResourceType.Stone] += b.Stash.Refined[ResourceType.Stone];
        Stash.Unrefined[ResourceType.Stone] += b.Stash.Unrefined[ResourceType.Stone];
        SparseResources();
    }

    public void ProcessResources()
    {
        Stash.Refined[ResourceType.Food] += Stash.Unrefined[ResourceType.Food];
        Stash.Refined[ResourceType.Stone] += Stash.Unrefined[ResourceType.Stone];
        Stash.Unrefined[ResourceType.Food] = 0;
        Stash.Unrefined[ResourceType.Stone] = 0;
        SparseResources();
    }

    public Inventory PutResources(Inventory workerInv)
    {
        Stash.Unrefined[ResourceType.Food] += workerInv.Unrefined[ResourceType.Food];
        Stash.Unrefined[ResourceType.Stone] += workerInv.Unrefined[ResourceType.Stone];

        workerInv.Unrefined[ResourceType.Food] = 0;
        workerInv.Unrefined[ResourceType.Stone] = 0;
        SparseResources();
        return workerInv;
    }

    public Inventory TakeResources(Inventory workerInv)
    {
        List<ResourceType> resType = new List<ResourceType>() { ResourceType.Food, ResourceType.Stone };
        foreach (ResourceType resource in resType)
        {
            float available = Mathf.Min(workerInv.capacity - workerInv.Refined[resource], Stash.Refined[resource]);
            workerInv.Refined[resource] += available;
            Stash.Refined[resource] -= available;
        }

        SparseResources();
        return workerInv;
    }

    public void SparseResources()
    {
        Inventory DividedInventory = new Inventory(25000);
        DividedInventory.Refined[ResourceType.Food] = Stash.Refined[ResourceType.Food] / Tiles.Count;
        DividedInventory.Unrefined[ResourceType.Food] = Stash.Unrefined[ResourceType.Food] / Tiles.Count;
        DividedInventory.Refined[ResourceType.Stone] = Stash.Refined[ResourceType.Stone] / Tiles.Count;
        DividedInventory.Unrefined[ResourceType.Stone] = Stash.Unrefined[ResourceType.Stone] / Tiles.Count;
        foreach (BuildableTile tile in Tiles)
        {
            tile.building.Stash = DividedInventory;
        }
    }
}