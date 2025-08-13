using System.Collections.Generic;
using UnityEngine;

public sealed class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }

    public List<GameObject> allUnitsList = new();
    public List<GameObject> unitsSelected = new();
    
    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask attackable;
    public bool attackCursorVisible;
    public GameObject groundMarker;
    
    private Camera cam;
    
    //Singleton 
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Debug.Log("UnitSelectionManager initialized!");
        }
    }
    
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                };
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }
            }
        }
        
        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;
                
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }
        
        // Attack Target
        if (unitsSelected.Count > 0 && AtLeastOneOffensiveUnit(unitsSelected))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
            {
                Debug.Log("Enemy Hovered with mouse");

                attackCursorVisible = true; 
                
                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;
                    foreach (var unit in unitsSelected)
                    {
                        if(unit.GetComponent<AttackController>())
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                }
            }
            else
            {
                attackCursorVisible = false;
            }
        }
        
        CursorSelector();
    }

    private bool AtLeastOneOffensiveUnit(List<GameObject> unitsSelected)
    {
        foreach (var unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }
        }
        return false;
    }

    void SelectByClicking(GameObject unit)
    { 
       DeselectAll();
       unitsSelected.Add(unit);
       SelectUnit(unit, true);
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
       unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            SelectUnit(unit, false);
        }
        
        groundMarker.SetActive(false);
        unitsSelected.Clear();
        
    }

    void MultiSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isEnabled)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isEnabled);
    }

    public void DragSelect(GameObject unit)
    {
        
        if (!IsInLayerMask(unit.layer, clickable))
        {
            return; 
        }

        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }

// Función auxiliar para verificar capas
    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        EnableUnitMovement(unit, isSelected);
    }
    
    private void CursorSelector()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Selectable);
        } else if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable) && (unitsSelected.Count > 0 && AtLeastOneOffensiveUnit(unitsSelected)))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Attackable);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground) && unitsSelected.Count > 0)
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Walkable);
        }
        else
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.None);
        }
    }
}