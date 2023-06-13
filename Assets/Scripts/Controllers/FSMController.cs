using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FSMController : Controller
{
    private Dictionary<string, FSMState<Player>> states = new Dictionary<string, FSMState<Player>>();


    public void Start(Player p)
    {
        states.Add("Expansion", new ExpansionState());
        states.Add("Mantainance", new MantainanceState());
        p.fsm.Configure(p, states["Expansion"]);

    }
    public bool Update(Player p)
    {

        p.fsm.Update();
        p.units.ForEach(unit =>
        {
            unit.GetComponent<Element>().UpdateFSM();
        });
        foreach (KeyValuePair<int, string> entry in DashboardFSM.Instance.roles)
        {
            Debug.Log("Key:" + entry.Key + "Value:" + entry.Value);
        }
        return p.EndTurn();
    }
};
