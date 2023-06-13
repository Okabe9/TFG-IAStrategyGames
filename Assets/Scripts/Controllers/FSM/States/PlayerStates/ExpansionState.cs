using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ExpansionState : FSMState<Player>
{
    override public void Enter(Player entity)
    {
        Debug.Log("Enters ExpansionState");
    }


    override public void Execute(Player entity)
    {
        DashboardFSM.Instance.playerStates[entity.teamInfo.teamId] = "Expansion";
        List<GameObject> goAgents = entity.units.FindAll(unit => unit.GetComponent<Agent>() != null);
        List<Agent> agents = new List<Agent>();
        DashboardFSM.Instance.roles = new Dictionary<int, string>();

        foreach (GameObject go in goAgents)
        {
            Agent a = go.GetComponent<Agent>();

            if (a.teamId == entity.teamInfo.teamId)
            {
                agents.Add(a);

            }
        }
        foreach (Agent agent in agents)
        {
            DashboardFSM.Instance.roles[agent.id] = "Miner";
        }
        for (int i = 0; i < Mathf.Ceil(agents.Count * 0.33f); i++)
        {
            DashboardFSM.Instance.roles[agents[i].id] = "Farmer";
        }

        if ((entity.units.Count * GameManager.Instance.foodConsumption * 1) >
            StateUtils.GetCompleteInventory(entity.teamInfo.teamId).Refined[ResourceType.Food])
        {
            entity.fsm.ChangeState(new MantainanceState());
        }

    }


    override public void Exit(Player entity)
    {
        Debug.Log("Exits ExpansionState");

    }
}