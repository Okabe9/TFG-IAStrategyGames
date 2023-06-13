using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AgentHarvestAction : Action<Element>
{
    public string Name { get; set; }

    public AgentHarvestAction()
    {
        Name = "AgentHarvest";
    }
    public ActionRet Execute(Element agent)
    {
        Debug.Log("a");
        AgentActionRet actionRet = new AgentActionRet();
        WorkerAgent ag = (WorkerAgent)agent;
        Vector2 cell = BoardController.Instance.getCell(ag.transform.position);
        GameObject tileObj = BoardController.Instance.GetTileInCell(cell);
        FarmTile farmTile = tileObj.GetComponent<FarmTile>();
        if (farmTile != null)
        {
            farmTile.SetAction(new TileHarvestAction());
            ActionRet ret = farmTile.ExecuteAction();
            TileActionRet tileRet = (TileActionRet)ret;
            farmTile.SetAction(new TileGrowAction());
            ag.inventory.Unrefined[ResourceType.Food] += tileRet.resourceAmount;


            if (tileRet.completed)
            {
                actionRet.completed = true;
                farmTile.cropSize = 0;
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
        FarmTile farmTile = tileObj.GetComponent<FarmTile>();
        if (farmTile == null)
        {
            return false;
        }

        return true;
    }
}