using System.Collections.Generic;
using UnityEngine;

public class SeleccionSimple : MonoBehaviour
{
    public static SeleccionSimple Instance { get; private set; }
    private readonly List<Peon> seleccionados = new();

    public IReadOnlyList<Peon> Seleccionados => seleccionados;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SeleccionarConClick();
        }
    }

    private void SeleccionarConClick()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out var hit, 500f))
        {
            var peon = hit.collider.GetComponentInParent<Peon>();
            bool add = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if (peon != null)
            {
                if (!add) seleccionados.Clear();
                if (!seleccionados.Contains(peon)) seleccionados.Add(peon);
            }
            else
            {
                if (!add) seleccionados.Clear();
            }
        }
    }
}
