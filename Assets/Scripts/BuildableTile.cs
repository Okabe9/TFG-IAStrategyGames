
using UnityEngine;

[System.Serializable]
public class BuildableTile : Tile
{
    public bool isOccupied = false;
    public Building building;

    // Start is called before the first frame update
    void Start()
    {
        Actions.Add(new TileBuildAction());

    }

    public void Destroy()
    {

        Destroy(gameObject);
    }

}
