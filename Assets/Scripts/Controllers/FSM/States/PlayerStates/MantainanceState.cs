using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MantainanceState : FSMState<Player>
{
    override public void Enter(Player entity)
    {
        Debug.Log("Enters MantainanceState");
    }


    override public void Execute(Player entity)
    {

        DashboardFSM.Instance.playerStates[entity.teamInfo.teamId] = "Mantainance";
        List<GameObject> goAgents = entity.units.FindAll(unit => unit.GetComponent<Agent>() != null);
        List<Agent> agents = new List<Agent>();
        DashboardFSM.Instance.roles = new Dictionary<int, string>();

        foreach (GameObject a in goAgents)
        {
            agents.Add(a.GetComponent<Agent>());
        }
        foreach (Agent agent in agents)
        {
            DashboardFSM.Instance.roles[agent.id] = "Farmer";
        }
        for (int i = 0; i < Mathf.Ceil(DashboardFSM.Instance.roles.Count * 0.33f); i++)
        {
            DashboardFSM.Instance.roles[agents[i].id] = "Miner";
        }
        Debug.Log("P=" + entity.units.Count * GameManager.Instance.foodConsumption * 1);
        Debug.Log("GF=" + StateUtils.GetCompleteInventory(entity.teamInfo.teamId).Refined[ResourceType.Food]);

        if ((entity.units.Count * GameManager.Instance.foodConsumption * 1) <
         StateUtils.GetCompleteInventory(entity.teamInfo.teamId).Refined[ResourceType.Food])
        {
            entity.fsm.ChangeState(new ExpansionState());
        }
    }


    override public void Exit(Player entity)
    {
        Debug.Log("Exits MantainanceState");

    }
}