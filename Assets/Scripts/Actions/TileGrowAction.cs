using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileGrowAction : Action<Element>
{
    public string Name { get; set; }

    public TileGrowAction()
    {
        Name = "TileGrow";
    }
    public ActionRet Execute(Element element)
    {
        TileActionRet actionRet = new TileActionRet();
        FarmTile tile = (FarmTile)element;
        if (tile.cropSize < 5)
        {
            tile.cropSize += 0.5f;
        }
        return actionRet;
    }
    public bool isPossible(Element element)
    {
        return true;
    }
}