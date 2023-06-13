using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AgentGatherAction : Action<Element>
{
    public string Name { get; set; }

    public AgentGatherAction()
    {
        Name = "AgentGather";
    }
    public ActionRet Execute(Element agent)
    {
        AgentActionRet actionRet = new AgentActionRet();
        WorkerAgent ag = (WorkerAgent)agent;
        Vector2 cell = BoardController.Instance.getCell(ag.transform.position);
        GameObject tileObj = BoardController.Instance.GetTileInCell(cell);
        ResourceTile resTile = tileObj.GetComponent<ResourceTile>();
        if (resTile != null)
        {
            resTile.SetAction(new TileGatherAction());
            ActionRet ret = resTile.ExecuteAction();
            TileActionRet tileRet = (TileActionRet)ret;
            ag.inventory.Unrefined[ResourceType.Stone] += tileRet.resourceAmount;


            if (tileRet.completed)
            {
                actionRet.completed = true;
                TileFactoryConfig config = GameManager.Instance.configs.buildableTileConfig;
                config.cell = BoardController.Instance.getCell(resTile.transform.position);
                resTile.Destroy();
                GameObject newTile = TileFactory.Instance.CreateTile(config, "Buildable");
                BoardController.Instance.ReplaceTile(config.cell, newTile);
            }

        }
        else
        {
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
        ResourceTile resourceTile = tileObj.GetComponent<ResourceTile>();
        if (resourceTile == null)
        {
            return false;
        }

        return true;
    }
}