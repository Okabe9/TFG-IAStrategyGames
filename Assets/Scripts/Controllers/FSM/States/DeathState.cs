using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeathState : FSMState<Agent>
{
    override public void Enter(Agent entity)
    {
        Debug.Log("Enters DeathState");
    }


    override public void Execute(Agent entity)
    {
        Debug.Log("Executes DeathState");
    }


    override public void Exit(Agent entity)
    {
        Debug.Log("Exits DeathState");

    }
}