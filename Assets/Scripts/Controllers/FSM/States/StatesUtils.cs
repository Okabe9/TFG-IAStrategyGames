using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StateUtils
{
    public static FSMState<Agent> SelectNewState(Agent a)
    {
        Debug.Log(DashboardFSM.Instance.playerStates[a.teamId]);

        Inventory teamInventory = GetCompleteInventory(a.teamId);
        if (a.inventory.Unrefined[ResourceType.Stone] >= 100)
        {
            return new GoProcessState();
        }
        if (a.inventory.Refined[ResourceType.Food] < 30)
        {
            return new GoRestockState();
        }
        if (DashboardFSM.Instance.roles[a.id] == "Farmer")
        {

            if ((DashboardFSM.Instance.playerStates[a.teamId] == "Expansion" && a.inventory.Refined[ResourceType.Stone] >= 100) || teamInventory.Refined[ResourceType.Stone] > 200)
            {
                return new GoSowState();
            }
            else if (DashboardFSM.Instance.playerStates[a.teamId] == "Expansion" && a.inventory.Refined[ResourceType.Stone] < 100)
            {
                return new GoGatherState();
            }
            else if (DashboardFSM.Instance.playerStates[a.teamId] == "Mantainance" || teamInventory.Refined[ResourceType.Food] < 200)
            {
                return new GoHarvestState();
            }


        }
        else
        {
            if (DashboardFSM.Instance.playerStates[a.teamId] == "Expansion" && a.inventory.Refined[ResourceType.Stone] >= 100)
            {
                return new GoBuildState();
            }
            else if (DashboardFSM.Instance.playerStates[a.teamId] == "Mantainance" || teamInventory.Refined[ResourceType.Stone] < 200)
            {
                return new GoGatherState();
            }

        }
        return new GoGatherState();
    }

    public static Inventory GetCompleteInventory(int teamId)
    {
        Inventory inv = new Inventory(25000);
        foreach (City city in BoardController.Instance.cities)
        {
            if (city.teamId == teamId)
            {
                inv.SumInventory(city.Stash);
            }
        }
        return inv;
    }
    public static Vector2 ClosestFarmTileGrown(Agent entity)
    {
        FarmTile[] farmTiles = BoardController.Instance.FindFarmTile();
        FarmTile closestTile = null;
        int closestDistance = int.MaxValue;

        foreach (FarmTile farmTile in farmTiles)
        {
            if (farmTile.cropSize > 3) // Comprobar si el cultivo ha crecido lo suficiente
            {
                List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(entity.transform.position), BoardController.Instance.getCell(farmTile.transform.position));
                if (path.Count < closestDistance) // Si esta celda está más cerca
                {
                    closestDistance = path.Count;
                    closestTile = farmTile;
                }
            }
        }
        if (closestTile == null)
        {
            return ClosestResourceTile(entity);
        }
        return BoardController.Instance.getCell(closestTile.transform.position);
    }
    public static Vector2 ClosestBuildableTileToCity(Agent entity)
    {
        BuildableTile[] buildableTiles = BoardController.Instance.FindBuildableTile();
        BuildableTile closestTile = null;
        int closestDistance = int.MaxValue;

        foreach (BuildableTile buildTile in buildableTiles)
        {
            if (buildTile.building == null) // Comprobar si el BuildableTile no tiene un edificio
            {
                foreach (var neighbour in BoardController.Instance.GetNeighbours(BoardController.Instance.getCell(buildTile.transform.position)))
                {
                    BuildableTile neighbourTile = neighbour.GetComponent<BuildableTile>();
                    if (neighbourTile != null)
                    {
                        if (neighbourTile.building != null && neighbourTile.building.teamId == entity.teamId) // Comprobar si el vecino tiene un edificio
                        {
                            List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(entity.transform.position), BoardController.Instance.getCell(buildTile.transform.position));
                            if (path.Count < closestDistance) // Si esta celda está más cerca
                            {
                                closestDistance = path.Count;
                                closestTile = buildTile;
                                break;
                            }
                        }
                    }
                }
            }

        }
        if (closestTile == null)
        {
            return ClosestResourceTile(entity);
        }
        return BoardController.Instance.getCell(closestTile.transform.position);
    }

    public static Vector2 ClosestResourceTile(Agent entity)
    {
        ResourceTile[] resourceTiles = BoardController.Instance.FindResourceTile();
        ResourceTile closestTile = null;
        int closestDistance = int.MaxValue;

        foreach (ResourceTile tile in resourceTiles)
        {
            List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(entity.transform.position), BoardController.Instance.getCell(tile.transform.position));
            if (path.Count < closestDistance)
            {
                closestDistance = path.Count;
                closestTile = tile;
            }
        }
        if (closestTile == null)
        {
            return new Vector2(0, 0);
        }
        return BoardController.Instance.getCell(closestTile.transform.position);
    }

    public static Vector2 ClosestCityTile(Agent entity)
    {
        BuildableTile[] buildableTiles = BoardController.Instance.FindBuildableTile();
        BuildableTile closestTile = null;
        int closestDistance = int.MaxValue;

        foreach (BuildableTile buildTile in buildableTiles)
        {
            if (buildTile.building != null && buildTile.building.teamId == entity.teamId) // Comprobar si el BuildableTile tiene un edificio
            {
                List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(entity.transform.position), BoardController.Instance.getCell(buildTile.transform.position));
                if (path.Count < closestDistance) // Si esta celda está más cerca
                {
                    closestDistance = path.Count;
                    closestTile = buildTile;
                }
            }
        }
        if (closestTile == null)
        {
            return ClosestResourceTile(entity);
        }
        return BoardController.Instance.getCell(closestTile.transform.position);
    }


}