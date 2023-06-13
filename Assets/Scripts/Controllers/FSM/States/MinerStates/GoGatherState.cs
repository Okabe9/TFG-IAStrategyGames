using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GoGatherState : FSMState<Agent>
{
    override public void Enter(Agent entity)
    {
        Debug.Log("Enters GoGatherState");
    }


    override public void Execute(Agent entity)
    {
        ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;

        Vector2 cell = StateUtils.ClosestResourceTile(entity);
        List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(entity.transform.position), cell);
        factoryConfig.path = path;
        GameManager.Instance.ExecuteActionFSM("AgentWalk", entity, factoryConfig);
        FSMState<Agent> state = StateUtils.SelectNewState(entity);
        if (state != null)
        {
            if (state.GetType() == this.GetType())
            {
                Action<Element> action = ActionsFactory.Instance.CreateAction("AgentGather");
                if (action.isPossible(entity))
                {
                    entity.fsm.ChangeState(new GatherState());
                }
            }
            else
            {
                entity.fsm.ChangeState(state);
            }
        }

    }


    override public void Exit(Agent entity)
    {
        Debug.Log("Exits GoGatherState");

    }


}