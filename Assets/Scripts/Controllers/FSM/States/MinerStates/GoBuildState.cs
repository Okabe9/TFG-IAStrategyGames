using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GoBuildState : FSMState<Agent>
{
    override public void Enter(Agent entity)
    {
        Debug.Log("Enters GoBuildState");
    }


    override public void Execute(Agent entity)
    {
        ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;

        Vector2 cell = StateUtils.ClosestBuildableTileToCity(entity);
        List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(entity.transform.position), cell);
        factoryConfig.path = path;
        GameManager.Instance.ExecuteActionFSM("AgentWalk", entity, factoryConfig);
        FSMState<Agent> state = StateUtils.SelectNewState(entity);
        if (state != null)
        {
            if (state.GetType() == this.GetType())
            {
                Action<Element> action = ActionsFactory.Instance.CreateAction("AgentBuild");
                if (action.isPossible(entity))
                {
                    entity.fsm.ChangeState(new BuildState());
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
        Debug.Log("Exits GoBuildState");

    }
}