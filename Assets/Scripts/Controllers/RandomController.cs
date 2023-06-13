using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomController : Controller
{
    public void Start(Player p)
    {

    }
    public bool Update(Player p)
    {
        foreach (GameObject unit in p.units)
        {
            Element element = unit.GetComponent<Element>();
            if (!element.isWorking())
            {
                ActionsFactoryConfig factoryConfig = GameManager.Instance.configs.actionsFactoryConfig;
                GameManager.Instance.DeselectAll();
                p.selectedUnits.Add(unit);
                List<string> actions = p.GetCommonNames();
                if (actions.Count > 0)
                {
                    int index = Random.Range(0, actions.Count);
                    string selectedAction = actions[index];
                    if ("AgentWalk" == selectedAction)
                    {
                        Vector2 cell = selectCellToWalk(element, p);
                        List<Vector2> path = Pathfinding.AStar(BoardController.Instance.getCell(unit.transform.position), cell);
                        factoryConfig.path = path;
                    }
                    GameManager.Instance.ExecuteActionAuto(selectedAction, factoryConfig);
                }
            }
        }
        return p.EndTurn();
    }
    public Vector2 selectCellToWalk(Element element, Player p)
    {
        WorkerAgent worker = (WorkerAgent)element;

        if (worker.inventory.Refined[ResourceType.Stone] >= 100)
        {
            //Search to build
            List<GameObject> elements = BoardController.Instance.GetAllTypeInstances("Buildable");
            int index = Random.Range(0, elements.Count - 1);
            return BoardController.Instance.getCell(elements[index].transform.position);
        }

        else
        {
            //Search to mine
            List<GameObject> elements = BoardController.Instance.GetAllTypeInstances("Resource");
            if (elements.Count > 0)
            {
                int index = Random.Range(0, elements.Count - 1);
                return BoardController.Instance.getCell(elements[index].transform.position);
            }
            return new Vector2(0, 0);
        }
    }
}