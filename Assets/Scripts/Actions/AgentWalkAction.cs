using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentWalkAction : Action<Element>
{
    public string Name { get; set; }

    public AgentWalkAction()
    {
        Name = "AgentWalk";
    }
    public ActionRet Execute(Element agent)
    {
        AgentActionRet actionRet = new AgentActionRet();
        Agent ag = (Agent)agent;
        Vector2 cell;
        if (ag.moveSpeed < ag.path.Count)
        {
            cell = ag.path[ag.moveSpeed - 1];
            ag.path.RemoveRange(0, ag.moveSpeed);
            actionRet.completed = false;
        }
        else
        {
            if (ag.path != null || ag.path.Count != 0)
            {
                cell = ag.path[ag.path.Count - 1];

                actionRet.completed = true;
            }
            else
            {
                cell = BoardController.Instance.getCell(agent.transform.position);
                actionRet.completed = true;
            }
            StopAtBuilding(ag, cell);

        }
        Vector3 pos = BoardController.Instance.getPosition(cell);
        ag.transform.position = new Vector3(pos.x, ag.transform.position.y, pos.z);
        return actionRet;
    }
    public bool isPossible(Element element)

    {
        return true;
    }
    public void StopAtBuilding(Agent ag, Vector2 cell)
    {
        BuildableTile bt = BoardController.Instance.GetTileInCell(cell).GetComponent<BuildableTile>();
        if (bt != null)
        {
            Debug.Log("Stop at building2 ");

            if (bt.building != null)
            {
                Debug.Log("Stop at building");
                City c = BoardController.Instance.cities.Find(c => c.Id == bt.building.cityId);
                ag.inventory = c.PutResources(ag.inventory);
                ag.inventory = c.TakeResources(ag.inventory);
            }
        }
    }
}