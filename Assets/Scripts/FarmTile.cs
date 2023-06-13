
using UnityEngine;
public class FarmTile : Tile
{
    public float cropSize = 1;
    // Start is called before the first frame update
    void Start()
    {
        Actions.Add(new TileHarvestAction());
        Actions.Add(new TileGrowAction());
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
