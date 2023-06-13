using UnityEngine;

public class AgentFactory
{
    private static AgentFactory _instance;

    public static AgentFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AgentFactory();
            }
            return _instance;
        }
    }
    private AgentFactory()
    {
    }
    public GameObject CreateAgent(AgentFactoryConfig config, string type)
    {
        switch (type)
        {
            case "Worker":
                GameObject workerInstance = GameObject.Instantiate(config.prefab);
                WorkerAgent workerAgent = workerInstance.GetComponent<WorkerAgent>();
                workerAgent.teamId = config.teamId;
                workerAgent.materialStopped = config.teamMaterialStopped;
                workerAgent.materialWorking = config.teamMaterialWorking;
                BoardController.Instance.InstantiateInCell(workerInstance, config.cell, config.height);
                return workerInstance;
            case "Explorer":
                GameObject explorerInstance = GameObject.Instantiate(config.prefab);
                ExplorerAgent explorerAgent = explorerInstance.GetComponent<ExplorerAgent>();
                explorerAgent.teamId = config.teamId;
                explorerAgent.materialStopped = config.teamMaterialStopped;
                explorerAgent.materialWorking = config.teamMaterialWorking;
                BoardController.Instance.InstantiateInCell(explorerInstance, config.cell, config.height);
                return explorerInstance;
            default:
                return null;
        }
    }
}