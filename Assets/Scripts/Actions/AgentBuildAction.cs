using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBuildAction : Action<Element>
{
    public string Name { get; set; }

    public AgentBuildAction()
    {
        Name = "AgentBuild";
    }
    public ActionRet Execute(Element element)
    {
        AgentActionRet actionRet = new AgentActionRet();
        WorkerAgent agent = (WorkerAgent)element;
        Vector2 cell = BoardController.Instance.getCell(agent.transform.position);
        GameObject tileObj = BoardController.Instance.GetTileInCell(cell);
        BuildableTile buildableTile = tileObj.GetComponent<BuildableTile>();
        actionRet.completed = false;
        if (buildableTile != null)
        {
            agent.inventory.Refined[ResourceType.Stone] -= 100;

            BuildingFactoryConfig config = GameManager.Instance.configs.houseBuildingConfig;
            config.teamId = agent.teamId;
            config.teamMaterialStopped = agent.materialStopped;
            config.teamMaterialWorking = agent.materialWorking;
            config.cell = BoardController.Instance.getCell(agent.transform.position);
            GameObject building = BuildingFactory.Instance.CreateBuilding(config, "House");
            GameManager.Instance.players[agent.teamId].units.Add(building);
            buildableTile.building = building.GetComponent<HouseBuilding>();
            actionRet.completed = true;
        }
        return actionRet;
    }
    public bool isPossible(Element element)
    {
        AgentActionRet actionRet = new AgentActionRet();
        WorkerAgent agent = (WorkerAgent)element;
        Vector2 cell = BoardController.Instance.getCell(agent.transform.position);
        GameObject tileObj = BoardController.Instance.GetTileInCell(cell);
        BuildableTile buildableTile = tileObj.GetComponent<BuildableTile>();
        if (buildableTile == null || agent.inventory.Refined[ResourceType.Stone] < 100)
        {
            return false;
        }
        else if (buildableTile != null)
        {
            if (buildableTile.building != null)
            {
                return false;
            }
        }
        return true;
    }
}