using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public List<Player> players;
    public List<TeamInfo> teamsInfo;
    public FactoryConfigs configs;
    public int currentPlayerIndex = 0;
    public int turn = 0;
    public string lastAction;
    public bool twoTriggerAction = false;
    public Material pathfindingMaterial;
    public int maxTurn;
    public bool finishedGame = false;
    public int winnerId = -1;
    public int gameCount = 10;
    public int game = 0;
    public int foodConsumption = 5;
    public int initialFood = 100;
    public int buildingLimit = 10;
    public GameConfigsList gameConfigs;
    public GameConfigs currentConfig;
    public int initGame = 0;
    // Start is called before the first frame update

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (GameManager)";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        initGame = game;

    }
    void Start()
    {
        int configIndex = game % ((gameCount - initGame) / gameConfigs.configs.Count);
        currentConfig = gameConfigs.configs[configIndex];
        foodConsumption = currentConfig.foodConsumption;
        initialFood = currentConfig.startingFood;
        maxTurn = currentConfig.limitTurn;
        this.buildingLimit = currentConfig.limitBuildings;
        turn = 0;
        finishedGame = false;
        lastAction = null;
        players = new List<Player>();
        currentPlayerIndex = 0;
        twoTriggerAction = false;
        InitTeams();
    }

    void Update()
    {
        if (!finishedGame)
        {
            if (players.Count > 0)
            {
                currentPlayerIndex = turn % players.Count;
                if (players[currentPlayerIndex].Update())
                {
                    turn += 1;
                }
                twoTriggerListener();

                if (turn >= maxTurn || PlayerVanished() || MaxBuildings())
                {
                    FinishGame();
                }
            }

        }

    }

    private void InitTeams()
    {
        List<Vector2> freeCells = BoardController.Instance.findFreeCells();
        Controller controller = new ManualController();
        foreach (TeamInfo tinfo in teamsInfo)
        {
            switch (tinfo.controllerType)
            {
                case ControllerType.Manual:
                    controller = new ManualController();
                    break;
                case ControllerType.Random:
                    controller = new RandomController();
                    break;

                case ControllerType.FSM:
                    controller = new FSMController();
                    break;
            }
            Debug.Log(tinfo.ToString() + freeCells[Random.Range(0, freeCells.Count - 1)].ToString() + configs.ToString() + controller.ToString());
            if (tinfo != null && freeCells.Count > 0 && configs != null && controller != null)
            {
                Player player = new Player(tinfo, freeCells[Random.Range(0, freeCells.Count - 1)], configs, controller);
                players.Add(player);
            }
            else
            {
                InitTeams();
            }
        }
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        players[currentPlayerIndex].selectedUnits.Add(unitToAdd);
        unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ClickShiftSelect(GameObject unitToAdd)
    {
        if (!players[currentPlayerIndex].selectedUnits.Contains(unitToAdd))
        {
            players[currentPlayerIndex].selectedUnits.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);

        }
        else
        {
            players[currentPlayerIndex].selectedUnits.Remove(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(false);

        }
    }
    public void DragSelect(GameObject unitToAdd)
    {
        if (!players[currentPlayerIndex].selectedUnits.Contains(unitToAdd))
        {
            players[currentPlayerIndex].selectedUnits.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);

        }
    }
    public void DeselectAll()
    {
        foreach (GameObject unit in players[currentPlayerIndex].selectedUnits)
        {
            unit.transform.GetChild(0).gameObject.SetActive(false);
        }
        players[currentPlayerIndex].selectedUnits.Clear();

    }
    public List<string> GetCommonActions()
    {
        return this.players[currentPlayerIndex].GetCommonNames();
    }
    public void ExecuteActionUI(string action)
    {

        for (int i = 0; i < this.players[currentPlayerIndex].selectedUnits.Count; i++)
        {
            Element unit = this.players[currentPlayerIndex].selectedUnits[i].GetComponent<Element>();

            if (unit != null)
            {
                if (action == "AgentWalk")
                {
                    twoTriggerAction = true;
                    BoardController.Instance.ResetOriginalMaterials();

                }
                else
                {
                    GameManager.Instance.DeselectAll();

                    Action<Element> act = ActionsFactory.Instance.CreateAction(action);
                    unit.SetAction(act);

                }

                lastAction = action;

            }
        }
    }
    public void ExecuteActionAuto(string action, ActionsFactoryConfig config)
    {

        for (int i = 0; i < this.players[currentPlayerIndex].selectedUnits.Count; i++)
        {
            Element unit = this.players[currentPlayerIndex].selectedUnits[i].GetComponent<Element>();

            if (unit != null)
            {

                if (action == "AgentWalk")
                {
                    Agent agent = (Agent)unit;
                    agent.path = config.path;
                }
                Action<Element> act = ActionsFactory.Instance.CreateAction(action);
                unit.SetAction(act);



                lastAction = action;

            }
        }
    }
    public void ExecuteActionFSM(string action, Element unit, ActionsFactoryConfig config)
    {
        if (unit != null)
        {

            if (action == "AgentWalk")
            {
                Agent agent = (Agent)unit;
                agent.path = config.path;
            }
            Action<Element> act = ActionsFactory.Instance.CreateAction(action);

            unit.SetAction(act);

            lastAction = action;
        }
    }

    public void twoTriggerListener()
    {

        if (GameManager.Instance.twoTriggerAction)
        {
            if (GameManager.Instance.lastAction == "AgentWalk")
            {
                for (int i = 0; i < this.players[currentPlayerIndex].selectedUnits.Count; i++)
                {
                    Element unit = this.players[currentPlayerIndex].selectedUnits[i].GetComponent<Element>();
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    Vector2 destinationCell = new Vector2(0, 0);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        destinationCell = BoardController.Instance.getCell(hit.collider.gameObject.transform.position);
                    }
                    List<Vector2> path = BoardController.Instance.DrawPathfinding(i, BoardController.Instance.getCell(unit.transform.position), destinationCell, pathfindingMaterial);

                    if (Input.GetMouseButtonDown(0))
                    {
                        this.lastAction = null;
                        this.twoTriggerAction = false;
                        this.players[currentPlayerIndex].selectedUnits[i].GetComponent<Agent>().path = path;

                        unit.SetAction(ActionsFactory.Instance.CreateAction("AgentWalk"));
                        BoardController.Instance.RevertPathColor(i);
                    }
                }
            }
        }
    }
    public bool PlayerVanished()
    {
        foreach (Player p in players)
        {
            if (p.units.Count == 0)
            {
                return true;
            }
        }
        return false;
    }
    public void FinishGame()
    {

        int winnerId = -1;
        int mostBuildings = 0;
        int mostAgents = 0;
        foreach (Player player in players)
        {
            List<GameObject> buildings = player.units.FindAll(unit => unit.GetComponent<Building>() != null);
            if (buildings.Count > mostBuildings)
            {
                winnerId = player.teamInfo.teamId;
                mostBuildings = buildings.Count;
            }
            else if (buildings.Count == mostBuildings)
            {
                List<GameObject> agents = player.units.FindAll(unit => unit.GetComponent<Agent>() != null);
                if (agents.Count > mostAgents)
                {
                    winnerId = player.teamInfo.teamId;
                    mostAgents = agents.Count;
                }
            }
        }
        this.winnerId = winnerId;
        SaveData();
        this.finishedGame = true;
        Debug.Log("The game has ended, the winner is: " + winnerId);
        game += 1;
        if (game < gameCount)
        {
            ResetGame();
        }
    }

    public void ResetGame()
    {
        BoardController.Instance.ResetBoard();
        DashboardFSM.Instance.playerStates = new Dictionary<int, string>();
        foreach (Resource o in Object.FindObjectsOfType<Resource>())
        {
            Destroy(o);
        }
        DashboardFSM.Instance.roles = new Dictionary<int, string>();
        Agent.nextId = 0;
        players.ForEach(player =>
               {
                   player.units.ForEach(unit =>
                   {
                       Destroy(unit);
                   });
               });

        this.Start();

    }
    public bool MaxBuildings()
    {
        foreach (Player player in players)
        {
            List<GameObject> buildings = player.units.FindAll(unit => unit.GetComponent<Building>() != null);
            if (buildings.Count >= buildingLimit)
            {
                return true;
            }

        }
        return false;
    }
    public void SaveData()
    {
        List<int> b = new List<int>();
        List<int> a = new List<int>();
        List<float> r = new List<float>();
        List<int> destroyedb = new List<int>();
        List<int> destroyeda = new List<int>();
        foreach (Player player in players)
        {
            List<GameObject> buildings = player.units.FindAll(unit => unit.GetComponent<Building>() != null);
            List<GameObject> agents = player.units.FindAll(unit => unit.GetComponent<Agent>() != null);
            Inventory i = StateUtils.GetCompleteInventory(player.teamInfo.teamId);
            r.Add(i.Refined[ResourceType.Food] + i.Refined[ResourceType.Stone] + i.Unrefined[ResourceType.Food] + i.Unrefined[ResourceType.Stone]);
            b.Add(buildings.Count);
            a.Add(agents.Count);
            destroyeda.Add(player.lostAgents);
            destroyedb.Add(player.lostBuildings);
        }

        SimulationExport export = new SimulationExport(players.Count, BoardController.Instance.size, BoardController.Instance.chances[1], maxTurn, 0, foodConsumption, initialFood, new List<string>() { players[0].controller.GetType().ToString(), players[1].controller.GetType().ToString() }, this.winnerId, this.turn, b, a, r, destroyedb, destroyeda);
        FileHandler.SaveToJSON<SimulationExport>(export, players[0].controller.GetType().ToString() + players[1].controller.GetType().ToString() + game + ".json");
    }

}

