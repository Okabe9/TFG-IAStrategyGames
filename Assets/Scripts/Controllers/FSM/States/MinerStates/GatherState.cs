using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GatherState : FSMState<Agent>
{
    override public void Enter(Agent entity)
    {
        Debug.Log("Enters GatherState");
    }


    override public void Execute(Agent entity)
    {
        ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;
        GameManager.Instance.ExecuteActionFSM("AgentGather", entity, factoryConfig);

        if (!new AgentGatherAction().isPossible(entity))
        {
            FSMState<Agent> state = StateUtils.SelectNewState(entity);
            entity.fsm.ChangeState(state);
        }

    }


    override public void Exit(Agent entity)
    {
        Debug.Log("Exits GatherState");

    }
}