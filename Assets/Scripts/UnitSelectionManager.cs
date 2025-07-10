using System.Collections.Generic;
using UnityEngine;
public sealed class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }

    public List<GameObject> allUnitsList = new();
    public List<GameObject> unitsSelected = new();
    
    public LayerMask clickable;
    public LayerMask ground;
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
                    MultipleSelection(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }
            }
        }
        
        if (Input.GetMouseButtonDown(1) && allUnitsList.Count > 0)
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
    }

    void SelectByClicking(GameObject unit)
    { 
       DeselectAll();
       unitsSelected.Add(unit);
       TriggerSelectionIndicator(unit, true);
       EnableUnitMovement(unit,true);
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
       unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    void DeselectAll()
    {
        foreach (GameObject unit in unitsSelected)
        {
            TriggerSelectionIndicator(unit, false);
            EnableUnitMovement(unit, false);
        }
        
        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }

    void MultipleSelection(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            TriggerSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
        else
        {
            EnableUnitMovement(unit, false);
            TriggerSelectionIndicator(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isEnabled)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isEnabled);
    }
}
