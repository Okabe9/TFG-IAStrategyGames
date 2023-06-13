using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public TeamInfo teamInfo;
    private Vector2 initialCell;
    private FactoryConfigs configs;
    [SerializeField, SerializeReference]
    public Controller controller;
    public FiniteStateMachine<Player> fsm = new FiniteStateMachine<Player>();
    public List<City> cities = new List<City>();
    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> selectedUnits = new List<GameObject>();
    public List<Action<Element>> actions = new List<Action<Element>>();
    public int currentUnitIndex = 0;
    private int nextCityId = 1;
    public int lostBuildings = 0;
    public int lostAgents = 0;
    public Player(TeamInfo teamInfo, Vector2 initialCell, FactoryConfigs configs, Controller controller)
    {
        this.teamInfo = teamInfo;
        this.initialCell = initialCell;
        this.configs = configs;
        this.controller = controller;
        this.controller.Start(this);
        lostBuildings = 0;
        int lostAgents = 0;
        InitPlayer();
    }

    public bool Update()
    {
        return this.controller.Update(this);
    }
    private void InitPlayer()
    {

        BuildingFactoryConfig buildingConfig = configs.houseBuildingConfig;
        buildingConfig.teamId = teamInfo.teamId;
        buildingConfig.teamMaterialStopped = teamInfo.teamMaterialStopped;
        buildingConfig.teamMaterialWorking = teamInfo.teamMaterialWorking;

        buildingConfig.height = 1;
        buildingConfig.cell = initialCell;
        GameObject b = BuildingFactory.Instance.CreateBuilding(buildingConfig, "House");
        units.Add(b);
        BoardController.Instance.GetTileInCell(buildingConfig.cell).GetComponent<BuildableTile>().building = b.GetComponent<Building>();
        AgentFactoryConfig agent1Config = configs.workerAgentConfig;
        agent1Config.teamId = teamInfo.teamId;
        agent1Config.teamMaterialStopped = teamInfo.teamMaterialStopped;
        agent1Config.teamMaterialWorking = teamInfo.teamMaterialWorking;

        agent1Config.height = 1;
        agent1Config.cell = new Vector2(initialCell.x, initialCell.y + 1);
        units.Add(AgentFactory.Instance.CreateAgent(agent1Config, "Worker"));

        AgentFactoryConfig agent2Config = configs.workerAgentConfig;
        agent2Config.teamId = teamInfo.teamId;
        agent2Config.teamMaterialStopped = teamInfo.teamMaterialStopped;
        agent2Config.teamMaterialWorking = teamInfo.teamMaterialWorking;

        agent2Config.height = 1;
        agent2Config.cell = new Vector2(initialCell.x + 1, initialCell.y);
        units.Add(AgentFactory.Instance.CreateAgent(agent2Config, "Worker"));

    }
    public void FindPath()
    {
        foreach (GameObject unit in selectedUnits)
        {
            Vector2 start = BoardController.Instance.getCell(unit.transform.position);
            Vector2 end = BoardController.Instance.mouseToGridCell();
        }
    }
    public bool EndTurn()
    {
        List<GameObject> auxUnits = new List<GameObject>(units);
        foreach (GameObject unit in auxUnits)
        {
            Element element = unit.GetComponent<Element>();

            if (element != null)
            {
                element.ExecuteAction();
            }
            if (element.GetComponent<Agent>())
            {
                Agent a = element.GetComponent<Agent>();
                a.inventory.Refined[ResourceType.Food] -= GameManager.Instance.foodConsumption;
                if (a.inventory.Refined[ResourceType.Food] < 0)
                {
                    lostAgents++;
                    units.Remove(unit);
                    element.Destroy();
                }
            }
            else
            {
                Building b = element.GetComponent<Building>();
                b.Stash.Refined[ResourceType.Food] -= GameManager.Instance.foodConsumption;
                if (b.Stash.Refined[ResourceType.Food] < 0)
                {
                    lostBuildings++;
                    units.Remove(unit);
                    element.Destroy();
                }
            }
        }
        FarmTile[] farmTiles = BoardController.Instance.FindFarmTilesInArray();
        foreach (FarmTile tile in farmTiles)
        {
            tile.ExecuteAction();
        }
        BoardController.Instance.RecomputeCities();
        GameManager.Instance.DeselectAll();
        return true;
    }

    public List<string> GetCommonNames()
    {
        Dictionary<string, int> nameFrequency = new Dictionary<string, int>();

        // Itera sobre cada GameObject en selectedUnits
        foreach (GameObject unit in selectedUnits)
        {
            HashSet<string> uniqueNamesInUnit = new HashSet<string>();

            // Obtiene los componentes que implementan las interfaces ITile, IAgent, IBuilding
            Element element = unit.GetComponent<Element>();

            // Agrega las acciones de los componentes al conjunto (HashSet) de nombres únicos
            if (element != null)
            {
                foreach (Action<Element> action in element.Actions)
                {
                    if (action.isPossible(element))
                    {
                        uniqueNamesInUnit.Add(action.Name);

                    }
                }
            }


            // Incrementa la frecuencia de cada nombre único en el diccionario
            foreach (string name in uniqueNamesInUnit)
            {
                if (nameFrequency.ContainsKey(name))
                {
                    nameFrequency[name]++;
                }
                else
                {
                    nameFrequency[name] = 1;
                }
            }
        }

        List<string> commonNames = new List<string>();

        // Itera sobre el diccionario y agrega los nombres con frecuencia igual al número total de unidades seleccionadas
        foreach (KeyValuePair<string, int> entry in nameFrequency)
        {
            if (entry.Value == selectedUnits.Count)
            {
                commonNames.Add(entry.Key);
            }
        }

        return commonNames;
    }
    public void SelectNextCurrentUnit()

    {

        bool foundUnit = false;
        while (!foundUnit && currentUnitIndex < units.Count)
        {
            GameObject unit = units[currentUnitIndex];
            Element e = unit.GetComponent<Element>();
            if (e.currentAction == null)
            {
                GameManager.Instance.ClickSelect(unit);
                if (GetCommonNames().Count <= 0)
                {
                    GameManager.Instance.DeselectAll();
                }
                else
                {
                    foundUnit = true;

                }
            }
            currentUnitIndex += 1;
        }
        if (currentUnitIndex == units.Count)
        {
            currentUnitIndex = 0;
        }
    }

    public List<Building> SearchAdjacentBuildings(Building building)
    {
        Vector2 originalPos = BoardController.Instance.getCell(building.gameObject.transform.position);
        List<Building> neighbours = new List<Building>();
        if (originalPos.x > 0)
        {
            Tile t = BoardController.Instance.GetTileInCell(new Vector2(originalPos.x - 1, originalPos.y)).GetComponent<Tile>();
            BuildableTile bt = t as BuildableTile;
            if (bt != null)
            {
                if (bt.building != null)
                {
                    neighbours.Add(bt.building);
                }
            }
        }
        if (originalPos.x < BoardController.Instance.size - 1)
        {
            Tile t = BoardController.Instance.GetTileInCell(new Vector2(originalPos.x + 1, originalPos.y)).GetComponent<Tile>();
            BuildableTile bt = t as BuildableTile;
            if (bt != null)
            {
                if (bt.building != null)
                {
                    neighbours.Add(bt.building);
                }
            }
        }
        if (originalPos.y > 0)
        {
            Tile t = BoardController.Instance.GetTileInCell(new Vector2(originalPos.x, originalPos.y - 1)).GetComponent<Tile>();
            BuildableTile bt = t as BuildableTile;
            if (bt != null)
            {
                if (bt.building != null)
                {
                    neighbours.Add(bt.building);
                }
            }
        }
        if (originalPos.y < BoardController.Instance.size - 1)
        {
            Tile t = BoardController.Instance.GetTileInCell(new Vector2(originalPos.x, originalPos.y + 1)).GetComponent<Tile>();
            BuildableTile bt = t as BuildableTile;
            if (bt != null)
            {
                if (bt.building != null)
                {
                    neighbours.Add(bt.building);
                }
            }
        }
        return neighbours;
    }

}