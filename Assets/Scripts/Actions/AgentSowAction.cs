using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSowAction : Action<Element>
{
    public string Name { get; set; }

    public AgentSowAction()
    {
        Name = "AgentSow";
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
            TileFactoryConfig config = GameManager.Instance.configs.farmTileConfig;
            config.cell = BoardController.Instance.getCell(buildableTile.transform.position);
            GameObject newTile = TileFactory.Instance.CreateTile(config, "Farm");
            BoardController.Instance.ReplaceTile(config.cell, newTile);

            FarmTile farm = newTile.GetComponent<FarmTile>();
            farm.SetAction(new TileGrowAction());
            buildableTile.Destroy();

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
        else if (buildableTile != null && buildableTile.building != null)
        {
            return false;
        }
        return true;
    }
}