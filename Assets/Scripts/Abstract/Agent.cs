using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class Agent : Element
{
    public int id;
    public static int nextId = 0;
    public Inventory inventory;
    public int teamId;
    public int houseId;
    public int moveSpeed = 5;
    public List<Vector2> path;
    private Dictionary<string, FSMState<Agent>> states = new Dictionary<string, FSMState<Agent>>();
    public FiniteStateMachine<Agent> fsm = new FiniteStateMachine<Agent>();
    public Agent()
    {
        this.Actions.Add(new AgentWalkAction());

    }
    private void Awake()
    {
        inventory = new Inventory(GameManager.Instance.initialFood);
        inventory.Refined[ResourceType.Food] = GameManager.Instance.initialFood;
        id = nextId;
        nextId++;
    }
    void Update()
    {

        if (this.currentAction == null)
        {
            this.gameObject.GetComponent<Renderer>().material = this.materialStopped;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material = this.materialWorking;
        }

    }

    public override void ConfigureFSM()
    {
        states.Add("GoHarvestState", new GoHarvestState());
        states.Add("GoSowState", new GoSowState());
        states.Add("GoBuildState", new GoBuildState());
        states.Add("GoGatherState", new GoGatherState());
        states.Add("HarvestState", new HarvestState());
        states.Add("BuildState", new BuildState());
        states.Add("SowState", new SowState());
        states.Add("GatherState", new GatherState());
        states.Add("DeathState", new DeathState());
        states.Add("GoProcessState", new GoProcessState());
        states.Add("GoRestockState", new GoRestockState());
        this.fsm.Configure(this, states["GoGatherState"]);
    }
    override public void UpdateFSM()
    {
        if (isConfigured)
        {
            fsm.Update();
        }
        else
        {
            this.ConfigureFSM();
            isConfigured = true;
        }
    }
}