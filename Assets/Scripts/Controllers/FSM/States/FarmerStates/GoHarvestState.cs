using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GoHarvestState : FSMState<Agent>
{
    override public void Enter(Agent entity)
    {
        Debug.Log("Enters GoHarvestState");
    }


    override public void Execute(Agent entity)
    {
        ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;
        Vector2 cell = StateUtils.ClosestFarmTileGrown(entity);
        List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(entity.transform.position), cell);
        factoryConfig.path = path;
        GameManager.Instance.ExecuteActionFSM("AgentWalk", entity, factoryConfig);
        FSMState<Agent> state = StateUtils.SelectNewState(entity);

        if (state.GetType() == this.GetType())
        {
            Action<Element> action = ActionsFactory.Instance.CreateAction("AgentHarvest");
            if (action.isPossible(entity))
            {
                entity.fsm.ChangeState(new HarvestState());
            }
        }
        else
        {
            entity.fsm.ChangeState(state);
        }
    }

    override public void Exit(Agent entity)
    {
        Debug.Log("Exits GoHarvestState");
    }
}