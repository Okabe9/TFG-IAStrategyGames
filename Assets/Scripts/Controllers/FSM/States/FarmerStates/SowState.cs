using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SowState : FSMState<Agent>
{
    override public void Enter(Agent entity)
    {
        Debug.Log("Enters SowState");
    }


    override public void Execute(Agent entity)
    {
        ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;
        GameManager.Instance.ExecuteActionFSM("AgentSow", entity, factoryConfig);
        FSMState<Agent> state = StateUtils.SelectNewState(entity);
        entity.fsm.ChangeState(state);
    }


    override public void Exit(Agent entity)
    {
        Debug.Log("Exits SowState");

    }
}