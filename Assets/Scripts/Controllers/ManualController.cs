using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ManualController : Controller
{
    public void Start(Player p)
    {

    }
    public bool Update(Player p)
    {
        if (Input.GetKeyDown(KeyCode.C) || p.selectedUnits.Count <= 0)
        {
            p.SelectNextCurrentUnit();

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BoardController.Instance.ResetOriginalMaterials();

            return p.EndTurn();
        }


        return false;
    }


}