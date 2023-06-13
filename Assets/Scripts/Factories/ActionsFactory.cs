using UnityEngine;

public class ActionsFactory
{
    private static ActionsFactory _instance;
    public static ActionsFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ActionsFactory();
            }
            return _instance;
        }
    }
    private ActionsFactory()
    {
    }
    public Action<Element> CreateAction(string type)
    {
        switch (type)
        {
            case "AgentBuild":
                return new AgentBuildAction();
            case "AgentGather":
                return new AgentGatherAction();
            case "AgentSow":
                return new AgentSowAction();
            case "AgentHarvest":
                return new AgentHarvestAction();
            case "AgentWalk":
                return new AgentWalkAction();
            case "BuildingGenerate":
                return new BuildingGenerateAction();
            case "TileBuild":
                return new TileBuildAction();
            case "TileGather":
                return new TileGatherAction();
            case "TileGrow":
                return new TileGrowAction();
            case "TileHarvest":
                return new TileHarvestAction();
            case "BuildingProcess":
                return new BuildingProcessAction();
            default: return null;

        }
    }
}