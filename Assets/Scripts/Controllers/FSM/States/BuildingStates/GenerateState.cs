using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenerateState : FSMState<Building>
{
    override public void Enter(Building entity)
    {
        Debug.Log("Enters GenerateState");
    }


    override public void Execute(Building entity)
    {
        ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;
        if (ActionsFactory.Instance.CreateAction("BuildingGenerate").isPossible(entity))
        {
            GameManager.Instance.ExecuteActionFSM("BuildingGenerate", entity, factoryConfig);
            Debug.Log(entity.Stash.Unrefined[ResourceType.Stone] > entity.Stash.Refined[ResourceType.Stone]);

        }
        if (DashboardFSM.Instance.playerStates[entity.teamId] == "Mantainance" || entity.Stash.Unrefined[ResourceType.Stone] > entity.Stash.Refined[ResourceType.Stone])
        {
            entity.fsm.ChangeState(new ProcessState());
        }

    }


    override public void Exit(Building entity)
    {
        Debug.Log("Exits GenerateState");
    }
}