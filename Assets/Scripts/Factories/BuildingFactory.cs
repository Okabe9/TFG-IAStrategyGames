using UnityEngine;

public class BuildingFactory
{
    private static BuildingFactory _instance;
    public static BuildingFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BuildingFactory();
            }
            return _instance;
        }
    }
    private BuildingFactory()
    {
    }
    public GameObject CreateBuilding(BuildingFactoryConfig config, string type)
    {
        switch (type)
        {
            case "House":
                GameObject houseInstance = GameObject.Instantiate(config.prefab);
                HouseBuilding houseBuilding = houseInstance.GetComponent<HouseBuilding>();
                houseBuilding.teamId = config.teamId;
                houseBuilding.materialStopped = config.teamMaterialStopped;
                houseBuilding.materialWorking = config.teamMaterialWorking;
                BoardController.Instance.InstantiateInCell(houseInstance, config.cell, config.height);
                return houseInstance;
            default:
                return null;
        }
    }
}