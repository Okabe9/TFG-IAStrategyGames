using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TeamInfo : ScriptableObject
{
    public int teamId;
    public Material teamMaterialStopped;
    public Material teamMaterialWorking;
    public ControllerType controllerType;

}
[System.Serializable]
public enum ControllerType
{
    Manual,
    Random,
    FSM
}