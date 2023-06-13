using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileBuildAction : Action<Element>
{
    public string Name { get; set; }

    public TileBuildAction()
    {
        Name = "TileBuild";
    }
    public ActionRet Execute(Element element)
    {
        Debug.Log("Tile construye");
        return new TileActionRet();
    }
    public bool isPossible(Element element)
    {
        return true;

    }
}