using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimulationExport
{
    public int playerNumber;
    public int boardSize;
    public int resourcePercent;
    public int limitTurn;
    public int limitBuildings;
    public int foodConsumption;
    public int initialFood;
    public List<string> controllers = new List<string>();
    public int winnerId;
    public int endTurn;
    public List<int> buildings = new List<int>();
    public List<int> agents = new List<int>();
    public List<float> resources = new List<float>();
    public List<int> buildingsDestroyed = new List<int>();
    public List<int> agentsDestroyed = new List<int>();

    public SimulationExport(
      int playerNumber, int boardSize, int resourcePercent,
      int limitTurn, int limitBuildings, int foodConsumption, int initialFood, List<string> controllers,
      int winnerId, int endTurn, List<int> buildings, List<int> agents, List<float> resources,
     List<int> buildingsDestroyed,
      List<int> agentsDestroyed)
    {
        this.playerNumber = playerNumber;
        this.boardSize = boardSize;
        this.resourcePercent = resourcePercent;
        this.limitTurn = limitTurn;
        this.limitBuildings = limitBuildings;
        this.foodConsumption = foodConsumption;
        this.initialFood = initialFood;
        this.controllers = controllers;
        this.winnerId = winnerId;
        this.endTurn = endTurn;
        this.buildings = buildings;
        this.agents = agents;
        this.resources = resources;
        this.buildingsDestroyed = buildingsDestroyed;
        this.agentsDestroyed = agentsDestroyed;
    }
}
