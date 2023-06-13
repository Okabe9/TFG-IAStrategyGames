using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject actionsPanel;
    public GameObject infoPanelText;
    public GameObject infoPanel;
    public GameObject currentPlayerStatus;
    public GameObject scorePanel;
    public GameObject scorePanelText;
    private List<string> aux = new List<string>();
    private List<string> actions = new List<string>();

    void Start()
    {


    }
    private void Update()
    {
        UpdateCurrentPlayerStatus();
        UpdateScorePanel();
        if (GameManager.Instance.players.Count > 0)
        {
            if (GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].controller as ManualController != null)
            {
                UpdateInfoPanel();

                actions = GameManager.Instance.GetCommonActions();
                if (!actionsEqual(actions, aux))
                {
                    UpdateButtons();
                    aux = actions;
                }
            }
        }

    }
    bool actionsEqual(List<string> a1, List<string> a2)
    {
        if (a1.Count != a2.Count)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < a1.Count; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
    public void UpdateInfoPanel()
    {
        foreach (Transform child in infoPanel.transform)
        {
            Destroy(child.gameObject);
        }
        if (GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].selectedUnits.Count > 0)
        {
            GameObject currentUnit = GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].selectedUnits[^1];
            GameObject newText;
            if (currentUnit.GetComponent<Agent>())
            {
                WorkerAgent a = currentUnit.GetComponent<WorkerAgent>();
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Tipo de unidad: Agent";
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Id del equipo:" + a.teamId;
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Cantidad de comida sin refinar:" + a.inventory.Unrefined[ResourceType.Food];
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Cantidad de comida refinada:" + a.inventory.Refined[ResourceType.Food];
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Cantidad de piedra sin refinar:" + a.inventory.Unrefined[ResourceType.Stone];
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Cantidad de piedra refinada:" + a.inventory.Refined[ResourceType.Stone];
            }
            else
            {
                Building b = currentUnit.GetComponent<Building>();
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Tipo de unidad: Building";
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Id del equipo:" + b.teamId;
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Cantidad de comida sin refinar:" + b.Stash.Unrefined[ResourceType.Food];
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Cantidad de comida refinada:" + b.Stash.Refined[ResourceType.Food];
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Cantidad de piedra sin refinar:" + b.Stash.Unrefined[ResourceType.Stone];
                newText = Instantiate(infoPanelText, infoPanel.transform);
                newText.GetComponent<TextMeshProUGUI>().text = "Cantidad de piedra refinada:" + b.Stash.Refined[ResourceType.Stone];
            }
            infoPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }
    public void UpdateScorePanel()
    {
        foreach (Transform child in scorePanel.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject newText;

        foreach (Player player in GameManager.Instance.players)
        {
            List<GameObject> buildings = player.units.FindAll(unit => unit.GetComponent<Building>() != null);
            newText = Instantiate(scorePanelText, scorePanel.transform);
            newText.GetComponent<TextMeshProUGUI>().text = buildings.Count.ToString();
            if (player.teamInfo.teamId == 0)
            {
                Color redColor;
                if (ColorUtility.TryParseHtmlString("#FF7070", out redColor))
                {
                    newText.GetComponent<TextMeshProUGUI>().color = redColor;
                }
            }
            else
            {
                Color blueColor;
                if (ColorUtility.TryParseHtmlString("#6BF3FF", out blueColor))
                {
                    newText.GetComponent<TextMeshProUGUI>().color = blueColor;
                }
            }
        }
    }
    public void UpdateCurrentPlayerStatus()
    {
        if (!GameManager.Instance.finishedGame)
        {
            currentPlayerStatus.GetComponent<TextMeshProUGUI>().text = "Jugador actual: " + GameManager.Instance.currentPlayerIndex;
            if (GameManager.Instance.currentPlayerIndex == 0)
            {
                Color redColor;
                if (ColorUtility.TryParseHtmlString("#FF7070", out redColor))
                {
                    currentPlayerStatus.GetComponent<TextMeshProUGUI>().color = redColor;
                }
            }
            else
            {
                Color blueColor;
                if (ColorUtility.TryParseHtmlString("#6BF3FF", out blueColor))
                {
                    currentPlayerStatus.GetComponent<TextMeshProUGUI>().color = blueColor;
                }
            }
        }
        else
        {
            currentPlayerStatus.GetComponent<TextMeshProUGUI>().text = "El ganador es: " + GameManager.Instance.winnerId;
            if (GameManager.Instance.winnerId == 0)
            {
                Color redColor;
                if (ColorUtility.TryParseHtmlString("#FF7070", out redColor))
                {
                    currentPlayerStatus.GetComponent<TextMeshProUGUI>().color = redColor;
                }
            }
            else
            {
                Color blueColor;
                if (ColorUtility.TryParseHtmlString("#6BF3FF", out blueColor))
                {
                    currentPlayerStatus.GetComponent<TextMeshProUGUI>().color = blueColor;
                }
            }
        }
    }
    public void UpdateButtons()
    {
        foreach (Transform child in actionsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string action in actions)
        {
            GameObject newButton = Instantiate(buttonPrefab, actionsPanel.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = action;
            newButton.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ExecuteActionUI(action));
        }
    }


}
