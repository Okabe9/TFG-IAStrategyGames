using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DashboardFSM : MonoBehaviour
{
    public Dictionary<int, string> playerStates = new Dictionary<int, string>();
    public Dictionary<int, string> roles = new Dictionary<int, string>();

    private static DashboardFSM _instance;
    public static DashboardFSM Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DashboardFSM>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<DashboardFSM>();
                    singletonObject.name = typeof(DashboardFSM).ToString() + " (DashboardFSM)";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }
}