
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class UnitSelectionBox : MonoBehaviour
{
    Camera myCam;
 
    [SerializeField]
    RectTransform boxVisual;
 
    Rect selectionBox;
 
    Vector2 startPosition;
    Vector2 endPosition;
    bool isDragging = false;
 
    private void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }
 
    private void Update()
    {
        // When Clicked
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
            isDragging = false; // reset
        }
 
        // When Dragging
        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
    
            // Determine if user is actually dragging (to avoid conflict with single click)
            if ((startPosition - endPosition).magnitude > 10f)
            {
                isDragging = true;
                DrawVisual();
                DrawSelection();
            }
        }
 
        // When Releasing
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelectionManager.Instance.DeselectAll();
                }

                SelectUnits();
            }

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
            isDragging = false;
        }
    }
 
    void DrawVisual()
    {
        // Calculate the starting and ending positions of the selection box.
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;
 
        // Calculate the center of the selection box.
        Vector2 boxCenter = (boxStart + boxEnd) / 2;
 
        // Set the position of the visual selection box based on its center.
        boxVisual.position = boxCenter;
 
        // Calculate the size of the selection box in both width and height.
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
 
        // Set the size of the visual selection box based on its calculated size.
        boxVisual.sizeDelta = boxSize;
    }
 
    void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }
 
 
        if (Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }
 
    void SelectUnits()
    {
        foreach (var unit in UnitSelectionManager.Instance.allUnitsList)
        {
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelectionManager.Instance.DragSelect(unit);
            }
        }
    }
}
