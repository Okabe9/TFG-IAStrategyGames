using UnityEngine;
using System;
using System.Collections.Generic;
public class BoardController : MonoBehaviour
{
    private static BoardController _instance;
    private GameObject[,] board;
    private bool[,] visited;
    public List<City> cities;

    public FactoryConfigs configs;
    [SerializeField]
    private GameObject[] tiles;

    [SerializeField]
    public int[] chances;

    [SerializeField]
    public int size;
    private static readonly int[] dx = { -1, 0, 1, 0 };
    private static readonly int[] dy = { 0, 1, 0, -1 };
    [SerializeField]
    private float gridOffset = 1.1f;
    private List<Vector2> currentPath = new List<Vector2>();
    private Dictionary<int, List<Vector2>> currentPaths = new Dictionary<int, List<Vector2>>();
    private Dictionary<Vector2, Material> originalMaterials = new Dictionary<Vector2, Material>();
    public static BoardController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BoardController>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<BoardController>();
                    singletonObject.name = typeof(BoardController).ToString() + " (BoardController)";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        Init();
    }
    private void Init()
    {
        if (GameManager.Instance.currentConfig != null)
        {
            size = GameManager.Instance.currentConfig.boardSize;
            chances[0] = 100 - GameManager.Instance.currentConfig.resourcePercent;
            chances[1] = GameManager.Instance.currentConfig.resourcePercent;
        }

        cities = new List<City>();
        board = new GameObject[size, size];
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                GameObject tile = GenerateTileInstance(new Vector2(i, j));
                board[i, j] = tile;
            }
        }
    }
    public void InstantiateInCell(GameObject obj, Vector2 cell, float height)
    {
        obj.transform.position = new Vector3(cell.x * gridOffset, height, cell.y * gridOffset);
    }
    public GameObject GenerateTileInstance(Vector2 pos)
    {
        int num = UnityEngine.Random.Range(0, 100);
        int prev = 0;
        bool found = false;
        int x = 0;

        while (!found)
        {
            if (prev <= num && (prev + chances[x]) > num)
            {
                found = true;
            }
            else
            {
                prev += chances[x];
                x += 1;
            }
        }
        switch (x)
        {
            case 0:
                TileFactoryConfig buildTileConfig = configs.buildableTileConfig;
                buildTileConfig.height = 0;
                buildTileConfig.cell = pos;
                return TileFactory.Instance.CreateTile(buildTileConfig, "Buildable");
            case 1:
                TileFactoryConfig tileFactoryConfig = configs.resourceTileConfig;
                tileFactoryConfig.height = 0;
                tileFactoryConfig.cell = pos;
                return TileFactory.Instance.CreateTile(tileFactoryConfig, "Resource");
            default:
                return null;
        }



    }

    public List<Vector2> findFreeCells()
    {
        List<Vector2> freeCells = new List<Vector2>();
        BuildableTile[] buildableTiles = FindObjectsOfType<BuildableTile>();
        for (int i = 0; i < buildableTiles.Length; i++)
        {
            if (buildableTiles[i].gameObject.transform.position.x > 2 &&
               buildableTiles[i].gameObject.transform.position.z > 2 &&
               buildableTiles[i].gameObject.transform.position.z < (size - 3) &&
               buildableTiles[i].gameObject.transform.position.x < (size - 3))
            {

                freeCells.Add(getCell(buildableTiles[i].gameObject.transform.position));
            }

        }

        return freeCells;
    }
    public BuildableTile[] FindBuildableTile()
    {
        return FindObjectsOfType<BuildableTile>();
    }
    public ResourceTile[] FindResourceTile()
    {
        return FindObjectsOfType<ResourceTile>();
    }
    public FarmTile[] FindFarmTile()
    {
        return FindObjectsOfType<FarmTile>();
    }
    public List<GameObject> GetNeighbours(Vector2 cell)
    {
        List<GameObject> neighbours = new List<GameObject>();


        // Posiciones de las celdas vecinas: abajo, arriba, izquierda, derecha
        Vector2[] neighbourPositions = new Vector2[]
        {
        new Vector2(cell.x, cell.y - 1), // Abajo
        new Vector2(cell.x, cell.y + 1), // Arriba
        new Vector2(cell.x - 1, cell.y), // Izquierda
        new Vector2(cell.x + 1, cell.y)  // Derecha
        };

        foreach (Vector2 pos in neighbourPositions)
        {
            // Comprobamos si la posición de la celda vecina está dentro de los límites del tablero
            if (pos.x >= 0 && pos.x < size && pos.y >= 0 && pos.y < size)
            {
                // Convertimos la posición de la celda al GameObject correspondiente en el tablero
                neighbours.Add(BoardController.Instance.board[(int)pos.x, (int)pos.y]);
            }
        }

        return neighbours;
    }
    public Vector2 mouseToGridCell()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return getCell(hit.collider.gameObject.transform.position);
        }
        else
        {
            return new Vector2(-1, -1);
        }
    }

    public Vector2 getCell(Vector3 position)
    {
        return new Vector2(position.x / gridOffset, position.z / gridOffset);
    }
    public Vector3 getPosition(Vector2 cell)
    {
        {
            return new Vector3(cell.x * gridOffset, 0, cell.y * gridOffset);
        }
    }

    public void ChangeTileColor(Vector2 position, Material newMaterial)
    {
        GameObject tile = board[(int)position.x, (int)position.y];
        tile.GetComponent<Renderer>().material = newMaterial;
    }

    public void StoreOriginalMaterial(Vector2 position)
    {
        if (!originalMaterials.ContainsKey(position))
        {
            GameObject tile = board[(int)position.x, (int)position.y];
            Material originalMaterial = tile.GetComponent<Renderer>().material;
            originalMaterials[position] = originalMaterial;
        }
    }

    public void ColorPath(int unitID, List<Vector2> path, Material pathMaterial)
    {
        currentPaths[unitID] = path;

        foreach (Vector2 tilePos in path)
        {
            ChangeTileColor(tilePos, pathMaterial);
        }
    }

    public void RevertPathColor(int unitID)
    {
        if (currentPaths.ContainsKey(unitID))
        {
            List<Vector2> path = currentPaths[unitID];
            foreach (Vector2 tilePos in path)
            {
                if (originalMaterials.ContainsKey(tilePos))
                {
                    ChangeTileColor(tilePos, originalMaterials[tilePos]);
                }
            }
            currentPaths.Remove(unitID);
        }
    }

    public void ResetOriginalMaterials()
    {
        originalMaterials.Clear();
    }
    public List<Vector2> DrawPathfinding(int unitID, Vector2 start, Vector2 end, Material pathMaterial)
    {
        List<Vector2> path = Pathfinding.AStar(start, end);
        foreach (Vector2 tilePos in path)
        {
            StoreOriginalMaterial(tilePos);
        }
        RevertPathColor(unitID);
        ColorPath(unitID, path, pathMaterial);
        return path;
    }
    public ref GameObject GetTileInCell(Vector2 cell)
    {
        return ref board[(int)cell.x, (int)cell.y];
    }
    public List<GameObject> GetAllTypeInstances(string element)
    {
        List<GameObject> returnList = new List<GameObject>();

        if (element == "Resource")
        {
            foreach (GameObject tile in board)
            {
                ResourceTile check = tile.GetComponent<ResourceTile>();
                if (check != null)
                {
                    returnList.Add(tile);
                }
            }
        }
        else if (element == "Buildable")
        {
            foreach (GameObject tile in board)
            {
                BuildableTile check = tile.GetComponent<BuildableTile>();
                if (check != null)
                {
                    returnList.Add(tile);
                }
            }
        }
        return returnList;
    }
    public void ReplaceTile(Vector2 cell, GameObject tile)
    {
        board[(int)cell.x, (int)cell.y] = tile;
    }
    public FarmTile[] FindFarmTilesInArray()
    {
        List<FarmTile> farmTilesList = new List<FarmTile>();

        int numRows = board.GetLength(0);
        int numColumns = board.GetLength(1);

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                GameObject currentGameObject = board[i, j];
                FarmTile farmTile = currentGameObject.GetComponent<FarmTile>();

                if (farmTile != null)
                {
                    farmTilesList.Add(farmTile);
                }
            }
        }

        return farmTilesList.ToArray();
    }
    private void DFS(int x, int y, City city)
    {
        visited[x, y] = true;
        BuildableTile bt = board[x, y].GetComponent<BuildableTile>();
        if (bt.building != null && bt.building.teamId == city.teamId)
        {
            city.AddBuilding(bt);
        }

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (IsValid(nx, ny) && !visited[nx, ny] && board[nx, ny].GetComponent<Tile>() is BuildableTile)
            {
                if (board[nx, ny].GetComponent<BuildableTile>().building != null)
                {
                    DFS(nx, ny, city);
                }
            }
        }
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < board.GetLength(0) && y < board.GetLength(1);
    }
    public void RecomputeCities()
    {
        // Reset visited array and cities list
        visited = new bool[board.GetLength(0), board.GetLength(1)];
        List<City> auxCities = cities;
        cities.Clear();
        int cityId = 0;
        // Scan the grid for cities
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (!visited[x, y] && board[x, y].GetComponent<Tile>() is BuildableTile)
                {
                    if (board[x, y].GetComponent<BuildableTile>().building != null)
                    {
                        // Found a new city
                        City city = new City(cityId);
                        city.teamId = board[x, y].GetComponent<BuildableTile>().building.teamId;
                        DFS(x, y, city);
                        cities.Add(city);
                        cityId++;
                    }

                }
            }
        }

    }
    public void ResetBoard()
    {
        foreach (GameObject go in board)
        {
            Destroy(go);
        }
        Init();

    }
}