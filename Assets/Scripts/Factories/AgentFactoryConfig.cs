using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AgentFactoryConfig
{
    public GameObject prefab;
    public Vector2 cell;
    public int height;
    public Material teamMaterialWorking;
    public Material teamMaterialStopped;
    public int teamId;
}