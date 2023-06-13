using UnityEngine;
public class ResourceTile : Tile
{
    [SerializeField]
    private GameObject[] resources;
    [SerializeField]
    private int[] chance;
    public int gatherSpeed = 34;
    public Resource resource;

    ResourceTile()
    {
        Actions.Add(new TileGatherAction());
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateResource();
    }

    private void GenerateResource()
    {
        int num = Random.Range(0, 100);
        int prev = 0;
        GameObject selectedTile = null;
        bool found = false;
        int x = 0;

        while (!found)
        {
            if (num >= prev && num <= chance[x])
            {
                selectedTile = resources[x];
                found = true;
            }
            else
            {
                prev = chance[x];
            }
            x += 1;
        }

        GameObject b = Instantiate(selectedTile, this.transform);
        b.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        if (b != null)
        {
            resource = b.GetComponent<Resource>();
        }
    }
    public void Destroy()
    {
        if (resource != null)
        {
            Destroy(resource.gameObject);
        }
        Destroy(gameObject);
    }

}
