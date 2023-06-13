using System.Collections.Generic;
using UnityEngine;


public class BuildingProcessAction : Action<Element>
{
    public string Name { get; set; }

    public BuildingProcessAction()
    {
        Name = "BuildingProcess";
    }
    public ActionRet Execute(Element element)
    {
        BuildingActionRet actionRet = new BuildingActionRet();

        if (element as HouseBuilding != null)
        {

            Building b = element as HouseBuilding;
            City c = BoardController.Instance.cities.Find(c => c.Id == b.cityId);
            c.ProcessResources();

        }
        actionRet.completed = true;
        return actionRet;
    }
    public bool isPossible(Element element)
    {
        if (element as HouseBuilding != null)
        {
            Building b = element as HouseBuilding;
            City c = BoardController.Instance.cities.Find(c => c.Id == b.cityId);
            if (c != null)
            {
                if (c.Stash.Unrefined[ResourceType.Food] > 0 || c.Stash.Unrefined[ResourceType.Stone] > 0)
                {
                    return true;
                }
            }


        }
        return false;
    }
}