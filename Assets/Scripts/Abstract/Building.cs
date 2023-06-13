using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class Building : Element
{
    public int teamId;
    public int cityId;
    public Inventory Stash;
    private Dictionary<string, FSMState<Building>> states = new Dictionary<string, FSMState<Building>>();
    public FiniteStateMachine<Building> fsm = new FiniteStateMachine<Building>();
    public Building()
    {
        Stash = new Inventory(500);
    }
    private void Awake()
    {
        Stash.Refined[ResourceType.Food] = GameManager.Instance.initialFood * 5;
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
        states.Add("GenerateState", new GenerateState());
        states.Add("ProcessState", new ProcessState());

        this.fsm.Configure(this, states["ProcessState"]);
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
