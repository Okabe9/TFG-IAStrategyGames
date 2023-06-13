using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GoRestockState : FSMState<Agent>
{
    override public void Enter(Agent entity)
    {
        Debug.Log("Enters GoRestockState");
    }


    override public void Execute(Agent entity)
    {
        ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;
        Vector2 cell = StateUtils.ClosestCityTile(entity);
        List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(entity.transform.position), cell);
        factoryConfig.path = path;
        GameManager.Instance.ExecuteActionFSM("AgentWalk", entity, factoryConfig);
        FSMState<Agent> state = StateUtils.SelectNewState(entity);
        entity.fsm.ChangeState(state);
    }


    override public void Exit(Agent entity)
    {
        Debug.Log("Exits GoRestockState");

    }
    public Vector2 ClosestCityTileWithFood(Agent entity)
    {
        return new Vector2();
    }
}