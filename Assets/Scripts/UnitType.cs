using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitClass
{
    Building,
    Agent
}
public enum UnitSubclass
{
    House,
    Worker,
    Explorer,
}

public class UnitType : MonoBehaviour
{
    public UnitClass unitClass;
    public UnitSubclass unitSubclass;
}
