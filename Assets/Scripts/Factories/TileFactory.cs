using UnityEngine;

public class TileFactory
{

    private static TileFactory _instance;
    public static TileFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TileFactory();
            }
            return _instance;
        }
    }
    private TileFactory()
    {
    }
    public GameObject CreateTile(TileFactoryConfig config, string type)
    {
        switch (type)
        {
            case "Buildable":
                GameObject buildableTile = GameObject.Instantiate(config.prefab);
                BoardController.Instance.InstantiateInCell(buildableTile, config.cell, config.height);
                return buildableTile;
            case "Resource":
                GameObject resourceTile = GameObject.Instantiate(config.prefab);
                BoardController.Instance.InstantiateInCell(resourceTile, config.cell, config.height);
                return resourceTile;
            case "Farm":
                GameObject farmTile = GameObject.Instantiate(config.prefab);
                BoardController.Instance.InstantiateInCell(farmTile, config.cell, config.height);
                return farmTile;
            default:
                return null;
        }

    }
}