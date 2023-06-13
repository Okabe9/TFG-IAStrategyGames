using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileHarvestAction : Action<Element>
{
    public string Name { get; set; }

    public TileHarvestAction()
    {
        Name = "TileHarvest";
    }
    public ActionRet Execute(Element element)
    {
        TileActionRet actionRet = new TileActionRet();
        FarmTile tile = (FarmTile)element;
        if (tile.cropSize > 0)
        {
            actionRet.resourceAmount = tile.cropSize * 10;
            tile.cropSize = 0;
            actionRet.completed = true;
        }
        tile.SetAction(new TileGrowAction());
        return actionRet;
    }
    public bool isPossible(Element element)
    {
        return true;
    }
}