using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileGatherAction : Action<Element>
{
    public string Name { get; set; }

    public TileGatherAction()
    {
        Name = "TileGather";
    }
    public ActionRet Execute(Element element)
    {
        TileActionRet actionRet = new TileActionRet();
        ResourceTile tile = (ResourceTile)element;
        if (tile.resource != null)
        {
            if (tile.resource.amount > tile.gatherSpeed)
            {
                actionRet.resourceAmount = tile.gatherSpeed;
                tile.resource.amount -= tile.gatherSpeed;
                actionRet.completed = false;
            }
            else
            {
                actionRet.resourceAmount = tile.resource.amount;
                tile.resource.amount = 0;
                tile.resource = null;
                actionRet.completed = true;
            }
        }
        return actionRet;
    }
    public bool isPossible(Element element)
    {
        return true;
    }
}