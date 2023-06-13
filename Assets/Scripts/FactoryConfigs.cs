using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FactoryConfigs : ScriptableObject
{
    public TileFactoryConfig buildableTileConfig;
    public TileFactoryConfig resourceTileConfig;
    public TileFactoryConfig farmTileConfig;
    public AgentFactoryConfig workerAgentConfig;
    public AgentFactoryConfig explorerAgentConfig;
    public BuildingFactoryConfig houseBuildingConfig;
    public ActionsFactoryConfig actionsFactoryConfig;

}
