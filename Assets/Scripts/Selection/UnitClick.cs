using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitClick : MonoBehaviour
{
    private Camera cam;

    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask ui;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            RaycastHit hit;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift) && GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].units.Contains(hit.collider.gameObject))
                {
                    GameManager.Instance.ClickShiftSelect(hit.collider.gameObject);
                }
                else if (GameManager.Instance.players[GameManager.Instance.currentPlayerIndex].units.Contains(hit.collider.gameObject))
                {
                    GameManager.Instance.ClickSelect(hit.collider.gameObject);
                }
            }


            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    GameManager.Instance.DeselectAll();
                }
            }
        }

    }
}
