using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    public Material materialStopped;
    public Material materialWorking;
    public string currentActionName;
    public Action<Element> currentAction { get; set; }
    public List<Action<Element>> Actions { get; protected set; }
    public bool isFsm = false;
    public bool isConfigured = false;

    public Element()
    {
        this.Actions = new List<Action<Element>>();
    }

    public virtual ActionRet ExecuteAction()
    {
        if (currentAction != null)
        {
            ActionRet ret = this.currentAction.Execute(this);
            if (ret.completed)
            {
                this.currentAction = null;
            }
            return ret;
        }
        return null;
    }
    public virtual void SetAction(Action<Element> action)
    {
        this.currentAction = action;
        currentActionName = action.Name;
    }
    public bool isWorking()
    {
        if (currentAction != null)
        {
            return true;
        }
        return false;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public abstract void ConfigureFSM();
    public abstract void UpdateFSM();
}