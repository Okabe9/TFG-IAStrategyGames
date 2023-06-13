using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameConfigs : ScriptableObject
{
    public int boardSize;
    public int resourcePercent;
    public int limitTurn;
    public int foodConsumption;
    public int startingFood;
    public int limitBuildings;
}