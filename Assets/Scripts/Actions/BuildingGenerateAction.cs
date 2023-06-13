using System.Collections.Generic;
using UnityEngine;


public class BuildingGenerateAction : Action<Element>
{
    public string Name { get; set; }

    public BuildingGenerateAction()
    {
        Name = "BuildingGenerate";
    }
    public ActionRet Execute(Element element)
    {
        BuildingActionRet actionRet = new BuildingActionRet();
        Building building = (Building)element;
        Vector2 cell = BoardController.Instance.getCell(building.transform.position);
        AgentFactoryConfig config = GameManager.Instance.configs.workerAgentConfig;
        config.teamId = building.teamId;
        config.teamMaterialStopped = building.materialStopped;
        config.teamMaterialWorking = building.materialWorking;
        config.cell = BoardController.Instance.getCell(building.transform.position);
        if (config.cell.x + 1 >= BoardController.Instance.size - 1)
        {
            config.cell = new Vector2(config.cell.x - 1, config.cell.y);

        }
        else
        {
            config.cell = new Vector2(config.cell.x + 1, config.cell.y);
        }
        GameObject agent = AgentFactory.Instance.CreateAgent(config, "Worker");
        GameManager.Instance.players[building.teamId].units.Add(agent);
        actionRet.completed = true;
        return actionRet;
    }
    public bool isPossible(Element element)
    {
        if (element as HouseBuilding != null)
        {
            HouseBuilding building = (HouseBuilding)element;

            List<GameObject> units = GameManager.Instance.players[building.teamId].units;
            int buildings = 0;
            int agents = 0;
            foreach (GameObject unit in units)
            {

                if (unit.GetComponent<Building>() != null)
                {
                    buildings++;
                }
                if (unit.GetComponent<Agent>() != null)
                {
                    agents++;
                }
            }
            if ((agents / buildings) >= 2)
            {
                return false;
            }
            return true;

        }
        else
        {
            return false;
        }
    }

}