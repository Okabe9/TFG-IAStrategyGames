using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProcessState : FSMState<Building>
{
    override public void Enter(Building entity)
    {
        Debug.Log("Enters ProcessState");
    }


    override public void Execute(Building entity)
    {
        ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;
        GameManager.Instance.ExecuteActionFSM("BuildingProcess", entity, factoryConfig); if (DashboardFSM.Instance.playerStates[entity.teamId] == "Expansion")
        {
            entity.fsm.ChangeState(new GenerateState());
        }
    }


    override public void Exit(Building entity)
    {
        Debug.Log("Exits ProcessState");

    }
}