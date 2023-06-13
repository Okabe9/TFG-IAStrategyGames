public class WorkerAgent : Agent
{
    // Start is called before the first frame update
    void Start()
    {
        Actions.Add(new AgentBuildAction());
        Actions.Add(new AgentGatherAction());
        Actions.Add(new AgentSowAction());
        Actions.Add(new AgentHarvestAction());

        moveSpeed = 2;
        //this.currentAction.Execute(this);
    }

}
